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
