namespace ImageTorque.Processing;

internal record struct CropRectangle
{
    public readonly double Cos;
    public readonly double HalfHeight;
    public readonly double HalfWidth;
    public readonly int Height;
    public readonly double Rotation;
    public readonly double Sin;
    public readonly int Width;
    public readonly double X;
    public readonly double Y;

    private const double Degrees360 = 2d * Math.PI;
    public CropRectangle(Rectangle rectangle)
    {
        Width = (int)Math.Ceiling(System.Convert.ToDouble(rectangle.Width));
        Height = (int)Math.Ceiling(System.Convert.ToDouble(rectangle.Height));
        HalfWidth = Width / 2d;
        HalfHeight = Height / 2d;

        X = rectangle.X;
        Y = rectangle.Y;

        // ToDo: Implement if we need the angle for the rectangle.
        // Rotation = NormalizeAngle(System.Convert.ToDouble(rectangle.Arc));
        // Cos = Math.Cos(rectangle.Arc);
        // Sin = Math.Sin(rectangle.Arc);
        Rotation = 0;
        Cos = 0;
        Sin = 0;
    }

    private static double NormalizeAngle(double arc)
    {
        return (arc % Degrees360 + Degrees360) % Degrees360;
    }
}
