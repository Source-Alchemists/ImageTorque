namespace ImageTorque;

/// <summary>
/// Represents a rectangle.
/// </summary>
public record struct Rectangle
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    public Rectangle() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="rectangle">The rectangle to copy.</param>
    public Rectangle(Rectangle rectangle)
    {
        X = rectangle.X;
        Y = rectangle.Y;
        Width = rectangle.Width;
        Height = rectangle.Height;
    }

    /// <summary>
    /// Gets the x offset.
    /// </summary>
    public int X { get; init; } = 0;

    /// <summary>
    /// Gets the y offset.
    /// </summary>
    public int Y { get; init; } = 0;

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; init; } = 0;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; init; } = 0;
}
