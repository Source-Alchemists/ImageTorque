using BenchmarkDotNet.Attributes;
using ImageTorque.Codecs.Png;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class PngIdentify
{
    private FileStream _imageTorqueStream = null!;
    private FileStream _imageSharpStream = null!;
    private PngDecoder _decoder = new PngDecoder();

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

    [Benchmark(Baseline = true)]
    public void ImageTorque()
    {
        _ = _decoder.Identify(_imageTorqueStream);
    }

    [Benchmark]
    public void ImageSharp()
    {
        _ = SixLabors.ImageSharp.Image.Identify(_imageSharpStream);
    }
}
