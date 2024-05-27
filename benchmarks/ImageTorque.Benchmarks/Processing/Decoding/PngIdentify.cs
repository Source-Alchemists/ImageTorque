using BenchmarkDotNet.Attributes;
using ImageTorque.Formats.Png;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class PngIdentify
{
    private FileStream _imageTorqueStream = null!;
    private FileStream _imageSharpStream = null!;
    private PngFormat _imageTorquePngFormat = new PngFormat();

    [GlobalSetup]
    public void Setup()
    {
        _imageTorqueStream = new FileStream("./lena24.png", FileMode.Open);
        _imageSharpStream = new FileStream("./lena24.png", FileMode.Open);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _imageTorqueStream?.Dispose();
        _imageSharpStream?.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _imageTorqueStream.Position = 0;
        _imageSharpStream.Position = 0;
    }

    [Benchmark]
    public void ImageTorque()
    {
        _ = _imageTorquePngFormat.Decoder.Identify(_imageTorqueStream);
    }

    [Benchmark]
    public void ImageSharp()
    {
        _ = SixLabors.ImageSharp.Image.Identify(_imageSharpStream);
    }
}
