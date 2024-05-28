using BenchmarkDotNet.Attributes;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[MinIterationCount(100)]
[MaxIterationCount(200)]
public class PngLoadImage
{
    private FileStream _imageTorqueStream = null!;
    private FileStream _imageSharpStream = null!;
    private Image _imageTorqueImage = null!;
    private SixLabors.ImageSharp.Image _imageSharpImage = null!;

    [IterationSetup(Target = nameof(ImageTorque))]
    public void ImageTorqueIterationSetup()
    {
        _imageTorqueStream = new FileStream("./lena24.png", FileMode.Open);
    }

    [Benchmark]
    public void ImageTorque()
    {
        _imageTorqueImage = Image.Load(_imageTorqueStream);
    }

    [IterationCleanup(Target = nameof(ImageTorque))]
    public void ImageTorqueIterationCleanup()
    {
        _imageTorqueImage?.Dispose();
    }

    [IterationSetup(Target = nameof(ImageSharp))]
    public void ImageSharpIterationSetup()
    {
        _imageSharpStream = new FileStream("./lena24.png", FileMode.Open);
    }

    [Benchmark]
    public void ImageSharp()
    {
        _imageSharpImage = SixLabors.ImageSharp.Image.Load(_imageSharpStream);
    }

    [IterationCleanup(Target = nameof(ImageSharp))]
    public void ImageSharpIterationCleanup()
    {
        _imageSharpImage?.Dispose();
        _imageSharpStream?.Dispose();
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
