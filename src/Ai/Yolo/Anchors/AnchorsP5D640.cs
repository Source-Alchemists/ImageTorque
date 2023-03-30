namespace ImageTorque.AI.Yolo;

/// <summary>
/// Represents the anchors for P5 with image dimension 640.
/// </summary>
public readonly struct AnchorsP5D640
{
    /// <summary>
    /// The anchors.
    /// </summary>
    public static int[][][] Anchors { get; } = new[] {
        new[] { new[] { 010, 13 }, new[] { 016, 030 }, new[] { 033, 023 } },
        new[] { new[] { 030, 61 }, new[] { 062, 045 }, new[] { 059, 119 } },
        new[] { new[] { 116, 90 }, new[] { 156, 198 }, new[] { 373, 326 } }
    };
}
