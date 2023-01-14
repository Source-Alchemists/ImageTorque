using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal partial class PixelBufferConverter : IProcessor<PixelBufferConvertParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(PixelBufferConvertParameters parameters)
    {
        Type inputType = parameters.Input.GetType();

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<LF>))
        {
            return ConvertMono(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return ConvertMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return ConvertMono16(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return ConvertPackedRgb(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return ConvertPackedRgb24(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return ConvertPackedRgb48(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<LF>))
        {
            return ConvertPlanarRgbFFF(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return ConvertPlanarRgb888(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return ConvertPlanarRgb161616(parameters);
        }

        throw new NotSupportedException($"The input type {inputType} is not supported.");
    }
}
