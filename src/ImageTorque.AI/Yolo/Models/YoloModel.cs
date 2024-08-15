namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo model.
/// </summary>
public record YoloModel
{

    /// <summary>
    /// Gets the width of the YoloModel.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the Yolo model.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the number of bytes per pixel.
    /// </summary>
    public int BytePerPixel { get; }

    /// <summary>
    /// Gets the anchors used in the YOLO model.
    /// </summary>
    /// <value>
    /// The anchors represented as a 3-dimensional array of integers.
    /// </value>
    public int[][][] Anchors { get; }

    /// <summary>
    /// Gets the confidence level of the YOLO model.
    /// </summary>
    public float Confidence { get; }

    /// <summary>
    /// Gets the multiplication confidence value.
    /// </summary>
    public float MulConfidence { get; }

    /// <summary>
    /// Gets the overlap value for the YoloModel.
    /// </summary>
    public float Overlap { get; }

    /// <summary>
    /// Gets the input for the YoloModel.
    /// </summary>
    public string Input { get; }

    /// <summary>
    /// Gets the type of the input for the Yolo model.
    /// </summary>
    public Type InputType { get; }

    /// <summary>
    /// Gets the array of output strings.
    /// </summary>
    public string[] Outputs { get; }

    /// <summary>
    /// Gets the array of types representing the output of the YOLO model.
    /// </summary>
    public Type[] OutputTypes { get; }

    /// <summary>
    /// Gets the list of Yolo labels.
    /// </summary>
    public IList<YoloLabel> Labels { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="YoloModel"/> class.
    /// </summary>
    /// <param name="width">The width of the input image.</param>
    /// <param name="height">The height of the input image.</param>
    /// <param name="bytePerPixel">The number of bytes per pixel in the input image.</param>
    /// <param name="anchors">The anchor boxes used for object detection.</param>
    /// <param name="confidence">The confidence threshold for object detection.</param>
    /// <param name="mulConfidence">The multiplier for confidence threshold during non-maximum suppression.</param>
    /// <param name="overlap">The overlap threshold for non-maximum suppression.</param>
    /// <param name="input">The name of the input layer.</param>
    /// <param name="inputType">The type of the input layer.</param>
    /// <param name="outputs">The names of the output layers.</param>
    /// <param name="outputTypes">The types of the output layers.</param>
    /// <param name="labels">The list of labels for object detection.</param>

    #pragma warning disable S2368 // Make this constructor private or simplify its parameters to not use multidimensional/jagged arrays.
    #pragma warning disable S107 // Methods should not have too many parameters
    public YoloModel(int width,
                    int height,
                    int bytePerPixel,
                    int[][][] anchors,
                    float confidence,
                    float mulConfidence,
                    float overlap,
                    string input,
                    Type inputType,
                    string[] outputs,
                    Type[] outputTypes,
                    IList<YoloLabel> labels)
    #pragma warning restore S2368 // Make this constructor private or simplify its parameters to not use multidimensional/jagged arrays.
    #pragma warning restore S107 // Methods should not have too many parameters
    {
        Width = width;
        Height = height;
        BytePerPixel = bytePerPixel;
        Anchors = anchors;
        Confidence = confidence;
        MulConfidence = mulConfidence;
        Overlap = overlap;
        Input = input;
        InputType = inputType;
        Outputs = outputs;
        OutputTypes = outputTypes;
        Labels = labels;
    }
};

