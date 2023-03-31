namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo model.
/// </summary>
public record YoloModel(
    int Width,
    int Height,
    int BytePerPixel,
    int Dimensions,
    int[][][] Anchors,
    float Confidence,
    float MulConfidence,
    float Overlap,
    string[] Outputs,
    IList<YoloLabel> Labels
);

