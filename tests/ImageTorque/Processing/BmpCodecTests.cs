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

using ImageTorque.Codecs.Bmp;

namespace ImageTorque.Processing.Tests;

public class BmpCodecTests
{
    [Theory]
    [InlineData("./lena8.bmp")]
    [InlineData("./lena24.bmp")]
    public void Test_Load(string path)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        // Act
        var image = Image.Load(stream, configuration);

        // Assert
        Assert.NotNull(image);
    }

    [Theory]
    [InlineData("./lena8.bmp")]
    [InlineData("./lena24.bmp")]
    public void Test_Save(string path)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var image = Image.Load(stream, configuration);
        using var outputStream = new MemoryStream();

        // Act
        image.Save(outputStream, "bmp", configuration);

        // Assert
        Assert.True(outputStream.Length > 0);
    }
}
