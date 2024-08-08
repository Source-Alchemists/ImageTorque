using BenchmarkDotNet.Attributes;
using ImageMagick;
using ImageTorque.Codecs.Png;
using SkiaSharp;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[MinIterationCount(100)]
[MaxIterationCount(200)]
public class PngImageLoad
{
    private const string BENCHMARK_IMAGE_PATH = "./lena24.png";
    private readonly Configuration _configuration = ConfigurationFactory.Build([new PngCodec()]);
    private Image _imageTorqueImage = null!;
    private SixLabors.ImageSharp.Image _imageSharpImage = null!;
    private SKBitmap _skiaBitmap = null!;
    private MagickImage _magickImage = null!;

    [Benchmark(Baseline = true)]
    public void ImageTorque()
    {
        _imageTorqueImage = Image.Load(BENCHMARK_IMAGE_PATH, _configuration);
    }

    [IterationCleanup(Target = nameof(ImageTorque))]
    public void ImageTorqueIterationCleanup()
    {
        _imageTorqueImage?.Dispose();
    }

    [Benchmark]
    public void ImageSharp()
    {
        _imageSharpImage = SixLabors.ImageSharp.Image.Load(BENCHMARK_IMAGE_PATH);
    }

    [IterationCleanup(Target = nameof(ImageSharp))]
    public void ImageSharpIterationCleanup()
    {
        _imageSharpImage?.Dispose();
    }

    [Benchmark]
    public void SkiaSharp()
    {
        _skiaBitmap = SKBitmap.Decode(BENCHMARK_IMAGE_PATH);
    }

    [IterationCleanup(Target = nameof(SkiaSharp))]
    public void SkiaSharpCleanup()
    {
        _skiaBitmap?.Dispose();
    }

    [Benchmark]
    public void ImageMagick()
    {
        _magickImage = new MagickImage(BENCHMARK_IMAGE_PATH);
    }

    [IterationCleanup(Target = nameof(ImageMagick))]
    public void ImageMagickCleanup()
    {
        _magickImage?.Dispose();
    }
}

// * Summary *
// BenchmarkDotNet v0.13.12, macOS Sonoma 14.5 (23F79) [Darwin 23.5.0]
// Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
// .NET SDK 8.0.300
//   [Host]     : .NET 8.0.5 (8.0.524.21615), Arm64 RyuJIT AdvSIMD
//   Job-INYWVL : .NET 8.0.5 (8.0.524.21615), Arm64 RyuJIT AdvSIMD

// InvocationCount=1  MaxIterationCount=200  MinIterationCount=100
// UnrollFactor=1

// | Method      | Mean     | Error     | StdDev    | Median   | Ratio | RatioSD | Allocated  | Alloc Ratio |
// |------------ |---------:|----------:|----------:|---------:|------:|--------:|-----------:|------------:|
// | ImageTorque | 6.369 ms | 0.0576 ms | 0.1557 ms | 6.379 ms |  1.00 |    0.00 |   18.66 KB |        1.00 |
// | ImageSharp  | 7.098 ms | 0.0608 ms | 0.1763 ms | 7.048 ms |  1.12 |    0.04 | 1558.51 KB |       83.54 |
// | SkiaSharp   | 4.212 ms | 0.0375 ms | 0.1095 ms | 4.208 ms |  0.66 |    0.03 |    1.78 KB |        0.10 |
// | ImageMagick | 5.796 ms | 0.0381 ms | 0.1113 ms | 5.775 ms |  0.91 |    0.03 |    3.92 KB |        0.21 |
