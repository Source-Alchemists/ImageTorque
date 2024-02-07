namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo model.
/// </summary>
public record YoloModel(
    int Width,
    int Height,
    int BytePerPixel,
    int[][][] Anchors,
    float Confidence,
    float MulConfidence,
    float Overlap,
    string Input,
    Type InputType,
    string[] Outputs,
    Type[] OutputTypes,
    IList<YoloLabel> Labels
);

