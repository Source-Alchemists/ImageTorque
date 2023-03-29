namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo model.
/// </summary>
public abstract record YoloModel
{
    /// <summary>
    /// The width of the model.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// The height of the model.
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// The number of bytes per pixel.
    /// </summary>
    public int BytePerPixel { get; init; }

    /// <summary>
    /// The number of dimensions.
    /// </summary>
    public int Dimensions { get; init; }

    /// <summary>
    /// The strides of the model.
    /// </summary>
    public int[] Strides { get; init; } = Array.Empty<int>();

    /// <summary>
    /// The shapes of the model.
    /// </summary>
    public int[] Shapes { get; init; } = Array.Empty<int>();

    /// <summary>
    /// The anchors of the model.
    /// </summary>
    public int[][][] Anchors { get; init; } = Array.Empty<int[][]>();

    /// <summary>
    /// The confidence threshold.
    /// </summary>
    public float Confidence { get; init; }

    /// <summary>
    /// The confidence multiplier.
    /// </summary>
    public float MulConfidence { get; init; }

    /// <summary>
    /// The overlap threshold.
    /// </summary>
    public float Overlap { get; init; }

    /// <summary>
    /// The outputs of the model.
    /// </summary>
    public string[] Outputs { get; init; } = Array.Empty<string>();

    /// <summary>
    /// The labels of the model.
    /// </summary>
    public IList<YoloLabel> Labels { get; init; } = new List<YoloLabel>();

    /// <summary>
    /// Whether to use detect or not.
    /// </summary>
    public bool DetectEnabled { get; init; }
}
