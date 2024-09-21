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

using ImageTorque.AI.Yolo;

namespace ImageTorque.AI.Tests;

public class YoloDetectorTests
{
    [Fact]
    public void Test_Prediction_YoloP5()
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.JpegCodec()]);
        using var detector = new YoloDetector<YoloP5Model>("./code_detector.onnx");
        using var image = Image.Load("./pzn_3.jpeg", configuration);

        // Act
        List<YoloPrediction> result = detector.Predict(image);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Test_Prediction_YoloV7()
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.JpegCodec()]);
        using var detector = new YoloDetector<YoloV7Model>("./code_detector_v7.onnx");
        using var image = Image.Load("./pzn_3.jpeg", configuration);

        // Act
        List<YoloPrediction> result = detector.Predict(image);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Test_Prediction_YoloNas_Quant()
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.JpegCodec()]);
        using var detector = new YoloDetector<YoloNasQuantModel>("./code_detector_nas_quant.onnx");
        using var image = Image.Load("./pzn_3.jpeg", configuration);

        // Act
        List<YoloPrediction> result = detector.Predict(image);

        // Assert
        Assert.Equal(3, result.Count);
    }

    private record YoloP5Model() : YoloModel(
        640,
        640,
        3,
        AnchorsP5D640.Anchors,
        0.20f,
        0.25f,
        0.45f,
        "images",
        typeof(float),
        new[] { "output" },
        new[] { typeof(float) },
        new List<YoloLabel> { new(0, "code") }
    );

    private record YoloV7Model(): YoloModel(
      640,
      640,
      3,
      AnchorsP5D640.Anchors,
      0.20f,
      0.00f,
      0.45f,
      "images",
      typeof(float),
      new[] { "output" },
      new[] { typeof(float) },
      new List<YoloLabel> { new(0, "code") }
    );

    private record YoloNasQuantModel(): YoloModel(
      640,
      640,
      3,
      AnchorsP5D640.Anchors,
      0.20f,
      0.00f,
      0.45f,
      "input",
      typeof(byte),
      new[] { "graph2_num_predictions", "graph2_pred_boxes", "graph2_pred_scores", "graph2_pred_classes" },
      new[] { typeof(long), typeof(float), typeof(float), typeof(long) },
      new List<YoloLabel> { new(0, "code") }
    );
}
