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

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if (pixel.A > 0)
                    {
                        empty = false;

                        if (x < fromX)
                            fromX = x;
                        if (x > toX)
                            toX = x;
                        if (y < fromY)
                            fromY = y;
                        if (y > toY)
                            toY = y;
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

            for (int x = 0; x < btm.Width; x++)
            {
                for (int y = 0; y < btm.Height; y++)
                {
                    btm2.SetPixel(x + 4, y + 4, btm.GetPixel(x, y));
                }
            }

            for (int x = 0; x < btm2.Width; x++)
            {
                for (int y = 0; y < btm2.Height; y++)
                {
                    output.Add(btm2.GetPixel(y, x).A);
                }
            }
            return output.ToArray();
        }
    }
}
