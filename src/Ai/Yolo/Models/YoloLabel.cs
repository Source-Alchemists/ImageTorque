namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo label.
/// </summary>
public record YoloLabel
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
