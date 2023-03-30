using ImageTorque.AI.Yolo;
using ImageTorque.Pixels;

namespace ImageTorque.AI.Tests;

public class YoloDetectorTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        using var detector = new YoloDetector<YoloP5Model>("./resources/code_detector.onnx");
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
        6,
        AnchorsP5D640.Anchors,
        0.20f,
        0.25f,
        0.45f,
        new[] { "output" },
        new List<YoloLabel> { new(0, "code") }
    );
}
