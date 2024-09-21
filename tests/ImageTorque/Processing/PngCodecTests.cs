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

using ImageTorque.Codecs.Png;

namespace ImageTorque.Processing.Tests;

public class PngCodecTests
{
    [Theory]
    [InlineData("./lena8.png")]
    [InlineData("./lena16.png")]
    [InlineData("./lena24.png")]
    [InlineData("./lena48.png")]
    public void Test_Load(string path)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new PngCodec()]);
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        // Act
        var image = Image.Load(stream, configuration);

        // Assert
        Assert.NotNull(image);
    }
}
