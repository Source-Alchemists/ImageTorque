# ImageTorque

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Source-Alchemists_ImageTorque&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Source-Alchemists_ImageTorque)
[![Build and publish](https://github.com/Source-Alchemists/ImageTorque/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/Source-Alchemists/ImageTorque/actions)
[![NuGet](https://img.shields.io/nuget/v/ImageTorque.svg)](https://www.nuget.org/packages/ImageTorque/)
[![MIT License](https://img.shields.io/badge/License-Apache_2.0-blue)](https://github.com/Source-Alchemists/ImageTorque/blob/main/LICENSE)

**Open .NET image processing library.**

ImageTorque is a cross-platform computer vision library.

Built for [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), ImageTorque can be run anywhere - from the edge device all the way up to cloud services.

## Codec providers

ImageTorque uses codec providers to decode and encode images. <br/>
The following codec providers are supported:

| Codec Provider | Supported Formats    | License |
|----------------|----------------------|---------|
| `ImageTorque`  | BMP, PNG (load only) | [Apache 2.0](../../LICENSE) |
| `Magick.NET`   | BMP, JPEG, PNG, TIFF | [Apache 2.0](https://github.com/dlemstra/Magick.NET/blob/main/License.txt) |
| `ImageSharp`   | BMP, JPEG, PNG       | [Six Labors Split License](https://github.com/SixLabors/ImageSharp/blob/main/LICENSE) |

### How to use custom codecs

```csharp
using ImageTorque;
using ImageTorque.Codecs.Png;

// Create a new configuration with PNG codec
var configuration = ConfigurationFactory.Build([
        new PngCodec()
    ]);

// Load image with the new configuration
var image = Image.Load("sample.png", configuration);
```

## Supported pixel types

| Pixel Type | Type   | Description            |
|------------|--------|------------------------|
| `LS`       | float  | 32 bit luminance pixel |
| `L8`       | byte   | 8 bit luminance pixel  |
| `L16`      | ushort | 16 bit luminance pixel |
| `RGB`      | float  | 32 bit color pixel     |
| `RGB24`    | byte   | 24 bit color pixel     |
| `RGB48`    | ushort | 48 bit color pixel     |
