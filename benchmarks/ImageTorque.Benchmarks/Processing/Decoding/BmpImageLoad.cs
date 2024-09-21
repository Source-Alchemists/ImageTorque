/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using BenchmarkDotNet.Attributes;
using ImageMagick;
using SkiaSharp;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
[MinIterationCount(100)]
[MaxIterationCount(200)]
public class BmpImageLoad
{
    private const string BENCHMARK_IMAGE_PATH = "./lena24.bmp";
    private readonly IConfiguration _configuration = ConfigurationFactory.Build([new Codecs.Bmp.BmpCodec()]);
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
//   Job-VLATEC : .NET 8.0.5 (8.0.524.21615), Arm64 RyuJIT AdvSIMD

// InvocationCount=1  MaxIterationCount=200  MinIterationCount=100
// UnrollFactor=1

// | Method      | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Allocated  | Alloc Ratio |
// |------------ |---------:|---------:|---------:|---------:|------:|--------:|-----------:|------------:|
// | ImageTorque | 493.8 us | 11.86 us | 49.31 us | 485.4 us |  1.00 |    0.00 |    7.39 KB |        1.00 |
// | ImageSharp  | 543.4 us | 13.24 us | 54.47 us | 528.9 us |  1.11 |    0.14 | 1042.61 KB |      141.07 |
// | SkiaSharp   | 336.0 us |  6.72 us | 27.55 us | 329.9 us |  0.69 |    0.09 |    2.44 KB |        0.33 |
// | ImageMagick | 420.8 us |  8.43 us | 35.13 us | 415.2 us |  0.86 |    0.12 |    3.92 KB |        0.53 |
