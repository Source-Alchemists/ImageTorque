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
public class JpegImageLoad
{
    private const string BENCHMARK_IMAGE_PATH = "./lena24.jpg";
    private readonly IConfiguration _configuration = ConfigurationFactory.Build([new Codecs.ImageMagick.JpegCodec()]);
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
