using System.Runtime.CompilerServices;

namespace ImageTorque.Codecs.Png;

internal static class Adam7
{
    public static readonly int[] FirstRow = [0, 0, 4, 0, 2, 0, 1];
    public static readonly int[] FirstColumn = [0, 4, 0, 2, 0, 1, 0];
    public static readonly int[] RowIncrement = [8, 8, 8, 4, 4, 2, 2];
    public static readonly int[] ColumnIncrement = [8, 8, 4, 4, 2, 2, 1];


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ComputeColumns(int width, int passIndex)
    {
        uint w = (uint)width;

        uint result = passIndex switch
        {
            0 => (w + 7) / 8,
            1 => (w + 3) / 8,
            2 => (w + 3) / 4,
            3 => (w + 1) / 4,
            4 => (w + 1) / 2,
            5 => w / 2,
            6 => w,
            _ => Throw(passIndex)
        };

        return (int)result;

        static uint Throw(int passIndex) => throw new ArgumentException($"Not a valid pass index: {passIndex}");
    }
}
