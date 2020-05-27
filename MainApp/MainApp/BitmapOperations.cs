using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MainApp
{
    static class BitmapOperations
    {
        private static Rectangle DrawSquare(Bitmap bitmap)
        {
            int fromX = int.MaxValue;
            int toX = int.MinValue;
            int fromY = int.MaxValue;
            int toY = int.MinValue;
            bool empty = true;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    if (pixel.A > 0)
                    {
                        empty = false;

                        if (i < fromX)
                            fromX = i;
                        if (i > toX)
                            toX = i;
                        if (j < fromY)
                            fromY = j;
                        if (j > toY)
                            toY = j;
                    }
                }
            }

            if (empty) return Rectangle.Empty;

            int dx = toX - fromX;
            int dy = toY - fromY;
            int side = Math.Max(dx, dy);

            if (dy > dx)
                fromX -= (side - dx) / 2;
            else
                fromY -= (side - dy) / 2;

            return new Rectangle(fromX, fromY, side, side);
        }

        private static Bitmap Resize(Rectangle rectangle, Bitmap bitmap)
        {
            Bitmap btm = new Bitmap(20, 20);
            Graphics gfx = Graphics.FromImage(btm);
            gfx.CompositingQuality = CompositingQuality.HighQuality;
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, 20, 20);
            gfx.DrawImage(bitmap, rect, rectangle, GraphicsUnit.Pixel);
            return btm;
        }

        public static double[] GetInput(Bitmap bitmap)
        {
            List<double> output = new List<double>();
            Rectangle rectangle = DrawSquare(bitmap);
            Bitmap btm = Resize(rectangle, bitmap);
            Bitmap btm2 = new Bitmap(28, 28);

            for (int i = 0; i < btm.Width; i++)
            {
                for (int j = 0; j < btm.Height; j++)
                {
                    btm2.SetPixel(i + 4, j + 4, btm.GetPixel(i, j));
                }
            }

            for (int y = 0; y < btm2.Height; y++)
            {
                for (int x = 0; x < btm2.Width; x++)
                {
                    output.Add(btm2.GetPixel(x, y).A);
                }
            }
            return output.ToArray();
        }
    }
}
