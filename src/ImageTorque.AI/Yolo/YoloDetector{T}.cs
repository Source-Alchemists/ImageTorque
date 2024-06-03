using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ImageTorque.Pixels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a YOLO detector.
/// </summary>
public class YoloDetector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TModel> : IDisposable where TModel : YoloModel
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
        using (var binaryReader = new BinaryReader(weights))
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
        Image inferenceImage = null!;
        bool resized = false;
        try
        {

            if (image.Width != _model.Width || image.Height != _model.Height)
            {
                inferenceImage = image.Resize(_model.Width, _model.Height);
                resized = true;
            }
            else
            {
                inferenceImage = image;
            }

            InferenceResult inferenceResult = Inference(_inferenceSession, _model, inferenceImage);
            List<YoloPrediction> parseResult = ParseOutput(_model, inferenceResult, (image.Width, image.Height));
            return Suppress(_model, parseResult);
        }
        finally
        {
            if (resized)
            {
                inferenceImage?.Dispose();
            }
        }
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
    private static Tensor<float> ExtractRgb24PixelsToFloat(TModel model, Image image)
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
    private static Tensor<byte> ExtractRgb24PixelsToByte(TModel model, Image image)
    {
        var tensor = new DenseTensor<byte>(new[] { 1, 3, model.Height, model.Width });
        Buffers.ReadOnlyPackedPixelBuffer<Rgb24> packedImageBuffer = image.AsPacked<Rgb24>();

        Parallel.For(0, image.Height, y =>
        {
            ReadOnlySpan<Rgb24> row = packedImageBuffer.GetRow(y);
            for (int x = 0; x < image.Width; x++)
            {
                Rgb24 pixel = row[x];
                tensor[0, 0, y, x] = pixel.R;
                tensor[0, 1, y, x] = pixel.G;
                tensor[0, 2, y, x] = pixel.B;
            }
        });

        return tensor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Tensor<float> ExtractMono8PixelsToFloat(TModel model, Image image)
    {
        var tensor = new DenseTensor<float>(new[] { 1, 3, model.Height, model.Width });
        Buffers.ReadOnlyPackedPixelBuffer<L8> packedImageBuffer = image.AsPacked<L8>();

        Parallel.For(0, image.Height, y =>
        {
            ReadOnlySpan<L8> row = packedImageBuffer.GetRow(y);
            for (int x = 0; x < image.Width; x++)
            {
                L8 pixel = row[x];
                float fPixel = pixel / 255.0F;
                tensor[0, 0, y, x] = fPixel;
                tensor[0, 1, y, x] = fPixel;
                tensor[0, 2, y, x] = fPixel;
            }
        });

        return tensor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Tensor<byte> ExtractMono8PixelsToByte(TModel model, Image image)
    {
        var tensor = new DenseTensor<byte>(new[] { 1, 3, model.Height, model.Width });
        Buffers.ReadOnlyPackedPixelBuffer<L8> packedImageBuffer = image.AsPacked<L8>();

        Parallel.For(0, image.Height, y =>
        {
            ReadOnlySpan<L8> row = packedImageBuffer.GetRow(y);
            for (int x = 0; x < image.Width; x++)
            {
                L8 pixel = row[x];
                tensor[0, 0, y, x] = pixel;
                tensor[0, 1, y, x] = pixel;
                tensor[0, 2, y, x] = pixel;
            }
        });

        return tensor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InferenceResult Inference(InferenceSession inferenceSession, TModel model, Image image)
    {
        var inputs = new List<NamedOnnxValue>();
        if (model.InputType == typeof(float))
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(model.Input,
                image.IsColor
                    ? ExtractRgb24PixelsToFloat(model, image)
                    : ExtractMono8PixelsToFloat(model, image)));
        }
        else if (model.InputType == typeof(byte))
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(model.Input,
                image.IsColor
                    ? ExtractRgb24PixelsToByte(model, image)
                    : ExtractMono8PixelsToByte(model, image)));
        }
        else
        {
            throw new NotSupportedException($"Tensor data type of {model.InputType} not supported!");
        }

        IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result = inferenceSession.Run(inputs);

        var floatOutput = new List<DenseTensor<float>>();
        var int64Output = new List<DenseTensor<long>>();

        for (int index = 0; index < model.Outputs.Length; index++)
        {
            string item = model.Outputs[index];
            Type itemType = model.OutputTypes[index];
            if (itemType == typeof(float))
            {
                floatOutput.Add((DenseTensor<float>)result.First(x => x.Name == item).Value);
            }
            else if (itemType == typeof(long))
            {
                int64Output.Add((DenseTensor<long>)result.First(x => x.Name == item).Value);
            }
        }

        return new InferenceResult(floatOutput.ToArray(), int64Output.ToArray());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<YoloPrediction> ParseOutput(TModel model, InferenceResult output, (int Width, int Height) image)
    {
        if (model.Outputs.Length == 4)
        {
            return ParseDetectionNMS(model, output, image);
        }
        else if (model.Outputs.Length == 1)
        {
            return ParseDetectNonNMS(model, output.FloatTensors[0], image);
        }
        else
        {
            throw new NotSupportedException($"Model must have NMS encoded ouputs (output length == 4) or non NMS encoded outputs (output length == 1). Got output length == {model.Outputs.Length}");
        }
    }

    /// <summary>
    /// Parse detections.
    /// </summary>
    /// <remarks>If batched output, the coordinates must be relative, else absolute.</remarks>
    /// <param name="model">The model.</param>
    /// <param name="output">The outputs.</param>
    /// <param name="image">The image.</param>
    /// <returns>Yolo detections.</returns>
    private static List<YoloPrediction> ParseDetectNonNMS(TModel model, DenseTensor<float> output, (int Width, int Height) image)
    {
        var result = new List<YoloPrediction>();
        (float xGain, float yGain) = (model.Width / (float)image.Width, model.Height / (float)image.Height);
        (float xPadding, float yPadding) = ((model.Width - (image.Width * xGain)) / 2, (model.Height - (image.Height * yGain)) / 2);

        bool isBatchedOutput = output.Dimensions.Length == 3;
        int dimensions = isBatchedOutput ? output.Dimensions[2] : output.Dimensions[1];

        if (isBatchedOutput)
        {
            ExtractBatchedRelativePredictions(model, output, image, result, xGain, yGain, xPadding, yPadding, dimensions);
        }
        else
        {
            ExtractNonBatchedAbsolutePredictions(model, output, image, result, xGain, yGain, xPadding, yPadding, dimensions);
        }

        return result;
    }

    private static List<YoloPrediction> ParseDetectionNMS(TModel model, InferenceResult inferenceResult, (int Width, int Height) image)
    {
        var result = new List<YoloPrediction>();
        (float xGain, float yGain) = (model.Width / (float)image.Width, model.Height / (float)image.Height);
        (float xPadding, float yPadding) = ((model.Width - (image.Width * xGain)) / 2, (model.Height - (image.Height * yGain)) / 2);

        DenseTensor<long> numPredictionsTensor = inferenceResult.Int64Tensors[0];
        DenseTensor<float> boxesTensor = inferenceResult.FloatTensors[0];
        DenseTensor<float> scoresTensor = inferenceResult.FloatTensors[1];
        DenseTensor<long> classesTensor = inferenceResult.Int64Tensors[1];

        for(int index = 0; index < numPredictionsTensor[0]; index++)
        {
            float xMin = (boxesTensor[0, index, 0] - xPadding) / xGain;
            float yMin = (boxesTensor[0, index, 1] - yPadding) / yGain;
            float xMax = (boxesTensor[0, index, 2] - xPadding) / xGain;
            float yMax = (boxesTensor[0, index, 3] - yPadding) / yGain;
            float score = scoresTensor[0, index];
            long classIndex = classesTensor[0, index];

            xMin = Clamp(xMin, 0, image.Width - 0);
            yMin = Clamp(yMin, 0, image.Height - 0);
            xMax = Clamp(xMax, 0, image.Width - 1);
            yMax = Clamp(yMax, 0, image.Height - 1);

            float width = xMax - xMin;
            float height = yMax - yMin;
            YoloLabel label = model.Labels[(int)classIndex];
            var prediction = new YoloPrediction(label, score, new(xMin + (width / 2), yMin + (height / 2), width, height));
            result.Add(prediction);
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ExtractBatchedRelativePredictions(TModel model, DenseTensor<float> output, (int Width, int Height) image, List<YoloPrediction> result, float xGain, float yGain, float xPadding, float yPadding, int dimensions)
    {
        for (int i = 0; i < output.Length / dimensions; i++)
        {
            float confidence = output[0, i, 4];
            if (confidence <= model.Confidence) continue;

            for (int j = 5; j < dimensions; j++)
            {
                output[0, i, j] *= confidence;
            }

            for (int k = 5; k < dimensions; k++)
            {
                if (output[0, i, k] <= model.MulConfidence) continue;
                float xMin = (output[0, i, 0] - (output[0, i, 2] / 2) - xPadding) / xGain;
                float yMin = (output[0, i, 1] - (output[0, i, 3] / 2) - yPadding) / yGain;
                float xMax = (output[0, i, 0] + (output[0, i, 2] / 2) - xPadding) / xGain;
                float yMax = (output[0, i, 1] + (output[0, i, 3] / 2) - yPadding) / yGain;

                xMin = Clamp(xMin, 0, image.Width - 0);
                yMin = Clamp(yMin, 0, image.Height - 0);
                xMax = Clamp(xMax, 0, image.Width - 1);
                yMax = Clamp(yMax, 0, image.Height - 1);

                float width = xMax - xMin;
                float height = yMax - yMin;
                YoloLabel label = model.Labels[k - 5];
                var prediction = new YoloPrediction(label, output[0, i, k], new(xMin + (width / 2), yMin + (height / 2), width, height));
                result.Add(prediction);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ExtractNonBatchedAbsolutePredictions(TModel model, DenseTensor<float> output, (int Width, int Height) image, List<YoloPrediction> result, float xGain, float yGain, float xPadding, float yPadding, int dimensions)
    {
        for (int i = 0; i < output.Length / dimensions; i++)
        {
            float confidence = output[i, 6];
            if (confidence <= model.Confidence) continue;

            for (int j = 6; j < dimensions; j++)
            {
                output[i, j] *= confidence;
            }

            for (int k = 6; k < dimensions; k++)
            {
                float xMin = (output[i, 1] - xPadding) / xGain;
                float yMin = (output[i, 2] - yPadding) / yGain;
                float xMax = (output[i, 3] - xPadding) / xGain;
                float yMax = (output[i, 4] - yPadding) / yGain;

                xMin = Clamp(xMin, 0, image.Width - 0);
                yMin = Clamp(yMin, 0, image.Height - 0);
                xMax = Clamp(xMax, 0, image.Width - 1);
                yMax = Clamp(yMax, 0, image.Height - 1);

                float width = xMax - xMin;
                float height = yMax - yMin;
                YoloLabel label = model.Labels[k - 6];
                var prediction = new YoloPrediction(label, output[i, k], new(xMin + (width / 2), yMin + (height / 2), width, height));

                result.Add(prediction);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<YoloPrediction> Suppress(TModel model, List<YoloPrediction> items)
    {
        var result = new List<YoloPrediction>(items);

        foreach (YoloPrediction item in items)
        {
            foreach (YoloPrediction? current in items.Where(current => current != item))
            {
                (Rectangle rect1, Rectangle rect2) = (item.Rectangle, current.Rectangle);

                var intersection = Rectangle.Intersect(rect1, rect2);

                int intArea = CalcArea(intersection);
                int unionArea = CalcArea(rect1) + CalcArea(rect2) - intArea;
                float overlap = intArea / (float)unionArea;

                if (overlap >= model.Overlap && item.Score >= current.Score)
                {
                    result.Remove(current);
                }
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalcArea(Rectangle rectangle) => rectangle.Width * rectangle.Height;

    private sealed record InferenceResult(DenseTensor<float>[] FloatTensors, DenseTensor<long>[] Int64Tensors);
}
