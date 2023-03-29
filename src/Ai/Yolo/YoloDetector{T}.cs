using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ImageTorque.Pixels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a YOLO detector.
/// </summary>
public class YoloDetector<TModel> : IDisposable where TModel : YoloModel
{
    private readonly TModel _model;
    private readonly InferenceSession _inferenceSession = null!;
    private bool _disposedValue;

    private YoloDetector()
    {
        _model = Activator.CreateInstance<TModel>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YoloDetector{TModel}"/> class.
    /// </summary>
    /// <param name="weights">The weights.</param>
    /// <param name="sessionOptions">The session options.</param>
    public YoloDetector(string weights, SessionOptions sessionOptions = null!) : this()
    {
        _inferenceSession = new InferenceSession(File.ReadAllBytes(weights), sessionOptions ?? new SessionOptions());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YoloDetector{TModel}"/> class.
    /// </summary>
    /// <param name="weights">The weights.</param>
    /// <param name="sessionOptions">The session options.</param>
    public YoloDetector(Stream weights, SessionOptions sessionOptions = null!) : this()
    {
        using(var binaryReader = new BinaryReader(weights))
        {
            _inferenceSession = new InferenceSession(binaryReader.ReadBytes((int)weights.Length), sessionOptions ?? new SessionOptions());
        }
    }

    /// <summary>
    /// Predicts objects in the image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <returns>The list of predictions.</returns>
    public List<YoloPrediction> Predict(Image image)
    {
        DenseTensor<float>[] inferenceResult = Inference(_inferenceSession, _model, image);
        List<YoloPrediction> parseResult = ParseOutput(_model, inferenceResult, (image.Width, image.Height));
        return Suppress(_model, parseResult);
    }

    /// <summary>
    /// Clamp a value between a minimum float and maximum float value.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    public static float Clamp(float value, float min, float max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="Yolo"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the <see cref="YoloDetector{TModel}"/> instance.
    /// </summary>
    /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is <see langword="true"/>), or from a finalizer (its value is <see langword="false"/>).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _inferenceSession.Dispose();
            }
            _disposedValue = true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Sigmoid(float value) => 1 / (1 + MathF.Exp(-value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float[] Xywh2xyxy(float[] source)
    {
        float[] result = new float[4];
        result[0] = source[0] - (source[2] / 2f);
        result[1] = source[1] - (source[3] / 2f);
        result[2] = source[0] + (source[2] / 2f);
        result[3] = source[1] + (source[3] / 2f);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Tensor<float> ExtractPixels(TModel model, Image image)
    {
        var tensor = new DenseTensor<float>(new[] { 1, 3, model.Height, model.Width });
        Buffers.ReadOnlyPackedPixelBuffer<Rgb24> packedImageBuffer = image.AsPacked<Rgb24>();

        Parallel.For(0, image.Height, y =>
        {
            ReadOnlySpan<Rgb24> row = packedImageBuffer.GetRow(y);
            for (int x = 0; x < image.Width; x++)
            {
                Rgb24 pixel = row[x];
                tensor[0, 0, y, x] = pixel.R / 255.0F;
                tensor[0, 1, y, x] = pixel.G / 255.0F;
                tensor[0, 2, y, x] = pixel.B / 255.0F;
            }
        });

        return tensor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DenseTensor<float>[] Inference(InferenceSession inferenceSession, TModel model, Image image)
    {
        Image inferenceImage = null!;
        bool resized = false;
        try
        {

            if (image.Width != model.Width || image.Height != model.Height)
            {
                inferenceImage = image.Resize(model.Width, model.Height);
                resized = true;
            }
            else
            {
                inferenceImage = image;
            }

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", ExtractPixels(model, inferenceImage))
            };

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result = inferenceSession.Run(inputs);

            var output = new List<DenseTensor<float>>();

            foreach (string item in model.Outputs)
            {
                output.Add((DenseTensor<float>)result.First(x => x.Name == item).Value);
            };

            return output.ToArray();
        }
        finally
        {
            if (resized)
            {
                inferenceImage?.Dispose();
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<YoloPrediction> ParseOutput(TModel model, DenseTensor<float>[] output, (int Width, int Height) image)
    {
        return model.DetectEnabled ? ParseDetect(model, output[0], image) : ParseSigmoid(model, output, image);
    }

    private static List<YoloPrediction> ParseDetect(TModel model, DenseTensor<float> output, (int Width, int Height) image)
    {
        var result = new ConcurrentBag<YoloPrediction>();
        (float xGain, float yGain) = (model.Width / (float)image.Width, model.Height / (float)image.Height);
        (float xPadding, float yPadding) = ((model.Width - (image.Width * xGain)) / 2, (model.Height - (image.Height * yGain)) / 2);

        Parallel.For(0, (int)output.Length / model.Dimensions, i =>
        {
            if (output[0, i, 4] <= model.Confidence) return;

            Parallel.For(5, model.Dimensions, j => output[0, i, j] *= output[0, i, 4]);

            Parallel.For(5, model.Dimensions, k =>
            {
                if (output[0, i, k] <= model.MulConfidence) return;

                float xMin = (output[0, i, 0] - (output[0, i, 2] / 2) - xPadding) / xGain;
                float yMin = (output[0, i, 1] - (output[0, i, 3] / 2) - yPadding) / yGain;
                float xMax = (output[0, i, 0] + (output[0, i, 2] / 2) - xPadding) / xGain;
                float yMax = (output[0, i, 1] + (output[0, i, 3] / 2) - yPadding) / yGain;

                xMin = Clamp(xMin, 0, image.Width - 0);
                yMin = Clamp(yMin, 0, image.Height - 0);
                xMax = Clamp(xMax, 0, image.Width - 1);
                yMax = Clamp(yMax, 0, image.Height - 1);

                YoloLabel label = model.Labels[k - 5];
                var prediction = new YoloPrediction(label, output[0, i, k], new(xMin, yMin, xMax - xMin, yMax - yMin));

                result.Add(prediction);
            });
        });

        return result.ToList();
    }

    private static List<YoloPrediction> ParseSigmoid(TModel model, DenseTensor<float>[] output, (int Width, int Height) image)
    {
        var result = new ConcurrentBag<YoloPrediction>();
        (float xGain, float yGain) = (model.Width / (float)image.Width, model.Height / (float)image.Height); // x, y gains
        (float xPadding, float yPadding) = ((model.Width - (image.Width * xGain)) / 2, (model.Height - (image.Height * yGain)) / 2); // left, right pads

        int anchorLength = model.Anchors[0].Length;

        Parallel.For(0, output.Length, i =>
        {
            int shapesCount = model.Shapes[i];
            for (int a = 0; a < anchorLength; a++)
            {
                for (int y = 0; y < shapesCount; y++)
                {
                    for (int x = 0; x < shapesCount; x++)
                    {
                        int offset = ((shapesCount * shapesCount * a) + (shapesCount * y) + x) * model.Dimensions;
                        float[] buffer = output[i].Skip(offset).Take(model.Dimensions).Select(Sigmoid).ToArray();
                        if (buffer[4] <= model.Confidence) return;
                        var scores = buffer.Skip(5).Select(b => b * buffer[4]).ToList();
                        float mulConfidence = scores.Max();

                        if (mulConfidence <= model.MulConfidence) return;

                        float rawX = ((buffer[0] * 2) - 0.5f + x) * model.Strides[i];
                        float rawY = ((buffer[1] * 2) - 0.5f + y) * model.Strides[i];

                        float rawW = MathF.Pow(buffer[2] * 2, 2) * model.Anchors[i][a][0];
                        float rawH = MathF.Pow(buffer[3] * 2, 2) * model.Anchors[i][a][1];

                        float[] xyxy = Xywh2xyxy(new[] { rawX, rawY, rawW, rawH });

                        float xMin = Clamp((xyxy[0] - xPadding) / xGain, 0, image.Width - 0);
                        float yMin = Clamp((xyxy[1] - yPadding) / yGain, 0, image.Height - 0);
                        float xMax = Clamp((xyxy[2] - xPadding) / xGain, 0, image.Width - 1);
                        float yMax = Clamp((xyxy[3] - yPadding) / yGain, 0, image.Height - 1);

                        YoloLabel label = model.Labels[scores.IndexOf(mulConfidence)];
                        var prediction = new YoloPrediction(label, mulConfidence, new(xMin, yMin, xMax - xMin, yMax - yMin));

                        result.Add(prediction);
                    }
                }
            }
        });

        return result.ToList();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<YoloPrediction> Suppress(TModel model, List<YoloPrediction> items)
    {
        var result = new List<YoloPrediction>(items);

        foreach (YoloPrediction item in items)
        {
            foreach (YoloPrediction? current in result.Where(current => current != item))
            {
                (Rectangle rect1, Rectangle rect2) = (item.Rectangle, current.Rectangle);

                var intersection = Rectangle.Intersect(rect1, rect2);

                int intArea = CalcArea(intersection);
                int unionArea = CalcArea(rect1) + CalcArea(rect2) - intArea;
                int overlap = intArea / unionArea;

                if (overlap >= model.Overlap)
                {
                    if (item.Score >= current.Score)
                    {
                        result.Remove(current);
                    }
                }
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalcArea(Rectangle rectangle) => rectangle.Width * rectangle.Height;
}
