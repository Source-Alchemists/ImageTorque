/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Runtime.CompilerServices;

namespace ImageTorque;

/// <summary>
/// Represents a rectangle.
/// </summary>
public readonly record struct Rectangle
{
    private readonly float _degree = 0f;
    private readonly float _radian = 0f;

    /// <summary>
    /// Gets the empty rectangle.
    /// </summary>
    public static readonly Rectangle Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    public Rectangle() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">The x offset.</param>
    /// <param name="y">The y offset.</param>
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
    /// <param name="x">The x offset.</param>
    /// <param name="y">The y offset.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="degree">The rotation degree.</param>
    public Rectangle(int x, int y, int width, int height, float degree) : this(x, y, width, height)
    {
        Degree = degree;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">The x offset.</param>
    /// <param name="y">The y offset.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Rectangle(float x, float y, float width, float height)
    {
        X = Convert.ToInt32(x);
        Y = Convert.ToInt32(y);
        Width = Convert.ToInt32(width);
        Height = Convert.ToInt32(height);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">The x offset.</param>
    /// <param name="y">The y offset.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="degree">The rotation degree.</param>
    public Rectangle(float x, float y, float width, float height, float degree) : this(x, y, width, height)
    {
        Degree = degree;
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
        Degree = rectangle.Degree;
    }

    /// <summary>
    /// Gets the X center position.
    /// </summary>
    public int X { get; init; } = 0;

    /// <summary>
    /// Gets the Y center position.
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

    /// <summary>
    /// Gets the rotation degree.
    /// </summary>
    public float Degree {
        get => _degree;
        init
        {
            _degree = value;
            _radian = value * MathF.PI / 180f;
        }
    }

    /// <summary>
    /// Gets the rotation radian.
    /// </summary>
    public float Radian => _radian;

    /// <summary>
    /// Gets the left position.
    /// </summary>
    public int Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X - (Width / 2) + Width;
    }

    /// <summary>
    /// Gets the right position.
    /// </summary>
    public int Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + (Width / 2) + Width;
    }

    /// <summary>
    /// Gets the top position.
    /// </summary>
    public int Top
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Y - (Height / 2) + Height;
    }

    /// <summary>
    /// Gets the bottom position.
    /// </summary>
    public int Bottom
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Y + (Height / 2) + Height;
    }

    /// <summary>
    /// Intersects the specified rectangles.
    /// </summary>
    /// <param name="rectangleA">The first rectangle.</param>
    /// <param name="rectangleB">The second rectangle.</param>
    /// <returns>The <see cref="Rectangle"/>.</returns>
    public static Rectangle Intersect(Rectangle rectangleA, Rectangle rectangleB)
    {
        int x1 = Math.Max(rectangleA.Left, rectangleB.Left);
        int x2 = Math.Min(rectangleA.Right, rectangleB.Right);
        int y1 = Math.Max(rectangleA.Top, rectangleB.Top);
        int y2 = Math.Min(rectangleA.Bottom, rectangleB.Bottom);

        if (x2 >= x1 && y2 >= y1)
        {
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        return Empty;
    }
}
