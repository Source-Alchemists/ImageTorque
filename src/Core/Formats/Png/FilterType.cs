namespace ImageTorque.Formats.Png;

/// <summary>
/// <see href="https://www.w3.org/TR/PNG-Filters.html"/>
/// </summary>
internal enum FilterType
{
    None = 0,
    Sub = 1,
    Up = 2,
    Average = 3,
    Paeth = 4
}
