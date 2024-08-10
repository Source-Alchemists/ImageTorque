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

namespace ImageTorque.Processing;

internal readonly record struct CropRectangle
{
    public readonly int Height;
    public readonly int Width;
    public readonly int X;
    public readonly int Y;
    public readonly int Top;
    public readonly int Left;
    public readonly float Rotation;
    public readonly float HalfHeight;
    public readonly float HalfWidth;
    public readonly float Cos;
    public readonly float Sin;

    public CropRectangle(Rectangle rectangle)
    {
        Rotation = rectangle.Radian;
        Cos = MathF.Cos(rectangle.Radian);
        Sin = MathF.Sin(rectangle.Radian);
        Width = rectangle.Width;
        Height = rectangle.Height;
        HalfWidth = Width / 2f;
        HalfHeight = Height / 2f;
        X = rectangle.X;
        Y = rectangle.Y;
        Top = rectangle.Top;
        Left = rectangle.Left;
    }
}
