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
using ImageTorque.Codecs.Png;

namespace ImageTorque.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class PngIdentify
{
    private readonly IConfiguration _configuration = ConfigurationFactory.Build([new PngCodec()]);
    private FileStream _imageTorqueStream = null!;
    private FileStream _imageSharpStream = null!;

    [GlobalSetup]
    public void Setup()
    {
        _imageTorqueStream = new FileStream("./lena24.png", FileMode.Open, FileAccess.Read);
        _imageSharpStream = new FileStream("./lena24.png", FileMode.Open, FileAccess.Read);
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
        _ = PngDecoder.Identify(_imageTorqueStream, _configuration);
    }

    [Benchmark]
    public void ImageSharp()
    {
        _ = SixLabors.ImageSharp.Image.Identify(_imageSharpStream);
    }
}
