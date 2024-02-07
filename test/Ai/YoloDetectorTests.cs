using ImageTorque.AI.Yolo;

namespace ImageTorque.AI.Tests;

public class YoloDetectorTests
{
    [Fact]
    public void Test_Prediction_YoloP5()
    {
        // Arrange
        using var detector = new YoloDetector<YoloP5Model>("./resources/code_detector.onnx");
        using var image = Image.Load("./resources/pzn_3.jpeg");

        // Act
        List<YoloPrediction> result = detector.Predict(image);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Test_Prediction_YoloV7()
    {
        // Arrange
        using var detector = new YoloDetector<YoloV7Model>("./resources/code_detector_v7.onnx");
        using var image = Image.Load("./resources/pzn_3.jpeg");

        // Act
        List<YoloPrediction> result = detector.Predict(image);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Test_Prediction_YoloNas_Quant()
    {
        // Arrange
        using var detector = new YoloDetector<YoloNasQuantModel>("./resources/code_detector_nas_quant.onnx");
        using var image = Image.Load("./resources/pzn_3.jpeg");

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
