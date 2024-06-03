namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents a Yolo prediction.
/// </summary>
/// <param name="Label">The label.</param>
/// <param name="Score">The score.</param>
/// <param name="Rectangle">The rectangle.</param>
public record YoloPrediction(YoloLabel Label, float Score, Rectangle Rectangle);
