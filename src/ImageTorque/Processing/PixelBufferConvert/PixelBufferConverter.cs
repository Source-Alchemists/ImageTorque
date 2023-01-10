using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

public partial class PixelBufferConverter : IProcessor<PixelBufferConvertParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(PixelBufferConvertParameters parameters)
    {
        Type inputType = parameters.Input.GetType();

        IPixelBuffer result = null!;
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono>))
        {
            result = ConvertMono(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono8>))
        {
            result = ConvertMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono16>))
        {
            result = ConvertMono16(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            result = ConvertPackedRgb(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            result = ConvertPackedRgb24(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            result = ConvertPackedRgb48(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<RgbFFF>))
        {
            result = ConvertPlanarRgbFFF(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb888>))
        {
            result = ConvertPlanarRgb888(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb161616>))
        {
            result = ConvertPlanarRgb161616(parameters);
        }

        if (result == null)
        {
            throw new NotSupportedException($"The input type {inputType} is not supported.");
        }

        return result;
    }
}
