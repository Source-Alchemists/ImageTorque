using BenchmarkDotNet.Attributes;
using ImageMagick;
using SkiaSharp;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[MinIterationCount(100)]
[MaxIterationCount(200)]
public class PngImageLoad
{
    private Image _imageTorqueImage = null!;
    private SixLabors.ImageSharp.Image _imageSharpImage = null!;
    private SKBitmap _skiaBitmap = null!;
    private MagickImage _magickImage = null!;

    [Benchmark(Baseline = true)]
    public void ImageTorque()
    {
        _imageTorqueImage = Image.Load("./lena24.png");
    }

    [IterationCleanup(Target = nameof(ImageTorque))]
    public void ImageTorqueIterationCleanup()
    {
        _imageTorqueImage?.Dispose();
    }

    [Benchmark]
    public void ImageSharp()
    {
        _imageSharpImage = SixLabors.ImageSharp.Image.Load("./lena24.png");
    }

    [IterationCleanup(Target = nameof(ImageSharp))]
    public void ImageSharpIterationCleanup()
    {
        _imageSharpImage?.Dispose();
    }

    [Benchmark]
    public void SkiaSharp()
    {
        _skiaBitmap = SKBitmap.Decode("./lena24.png");
    }

    [IterationCleanup(Target = nameof(SkiaSharp))]
    public void SkiaSharpCleanup()
    {
        _skiaBitmap?.Dispose();
    }

    [Benchmark]
    public void ImageMagick()
    {
        _magickImage = new MagickImage("./lena24.png");
    }

    [IterationCleanup(Target = nameof(ImageMagick))]
    public void ImageMagickCleanup()
    {
        _magickImage?.Dispose();
    }
}

// BenchmarkDotNet v0.13.12, macOS Sonoma 14.5 (23F79) [Darwin 23.5.0]
// Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
// .NET SDK 8.0.300
//   [Host]     : .NET 8.0.5 (8.0.524.21615), Arm64 RyuJIT AdvSIMD
//   Job-DUPEAW : .NET 8.0.5 (8.0.524.21615), Arm64 RyuJIT AdvSIMD

// InvocationCount=1  MaxIterationCount=200  MinIterationCount=100
// UnrollFactor=1

// | Method      | Mean     | Error     | StdDev    | Median   | Allocated  |
// |------------ |---------:|----------:|----------:|---------:|-----------:|
// | ImageTorque | 6.646 ms | 0.1677 ms | 0.7100 ms | 6.224 ms |   16.59 KB |
// | ImageSharp  | 7.032 ms | 0.0485 ms | 0.1392 ms | 7.005 ms | 1557.11 KB |
