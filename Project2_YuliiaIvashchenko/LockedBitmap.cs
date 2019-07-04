using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Project2_YuliiaIvashchenko
{
    class LockedBitmap
    {
        public LockedBitmap(string filename)
        {
            if (bit == null)
            {
                bit = new Bitmap(filename);
                r = new Rectangle(new Point(0, 0), bit.Size);
            }
        }
        public LockedBitmap(int width, int height)
        {
            if (bit == null)
            {
                bit = new Bitmap(width, height);
                r = new Rectangle(new Point(0, 0), bit.Size);
            }
        }
        public LockedBitmap(Bitmap bitmap)
        {
            if (bit == null)
            {
                bit = new Bitmap(bitmap);
                r = new Rectangle(new Point(0, 0), bit.Size);
            }
        }
        public static implicit operator LockedBitmap(Bitmap bitmap)
        {
            return new LockedBitmap(bitmap);
        }
        public void LockBits()
        {
            bData = bit.LockBits(r, System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);

            ptr = bData.Scan0;
            pixels = new byte[Math.Abs(bData.Stride) * bit.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, Math.Abs(bData.Stride) * bit.Height);
        }
        public void UnlockBits()
        {
            ptr = bData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, Math.Abs(bData.Stride) * bit.Height);

            bit.UnlockBits(bData);
        }
        public Color GetPixel(int row, int col)
        {
            int channel = System.Drawing.Bitmap.GetPixelFormatSize(bData.PixelFormat);
            int pixel = (row + col * bit.Width) * (channel / 8);

            int red = 0;
            int green = 0;
            int blue = 0;
            int alpha = 0;

            if (channel == 32)
            {
                blue = pixels[pixel];
                green = pixels[pixel + 1];
                red = pixels[pixel + 2];
                alpha = pixels[pixel + 3];
            }

            else if (channel == 24)
            {
                blue = pixels[pixel];
                green = pixels[pixel + 1];
                red = pixels[pixel + 2];
            }

            else if (channel == 16)
            {
                blue = pixels[pixel];
                green = pixels[pixel + 1];
            }

            else if (channel == 8)
            {
                blue = pixels[pixel];
            }

            return (channel != 8) ? Color.FromArgb(red, green, blue) : Color.FromArgb(blue, blue, blue);
        }
        public void SetPixel(int row, int col, Color clr)
        {
            int channel = System.Drawing.Bitmap.GetPixelFormatSize(bData.PixelFormat);
            int pixel = (row + col * bit.Width) * (channel / 8);

            if (channel == 32)
            {
                pixels[pixel] = clr.B;
                pixels[pixel + 1] = clr.G;
                pixels[pixel + 2] = clr.R;
                pixels[pixel + 3] = clr.A;
            }

            else if (channel == 24)
            {
                pixels[pixel] = clr.B;
                pixels[pixel + 1] = clr.G;
                pixels[pixel + 2] = clr.R;
            }

            else if (channel == 16)
            {
                pixels[pixel] = clr.B;
                pixels[pixel + 1] = clr.G;
            }

            else if (channel == 8)
            {
                pixels[pixel] = clr.B;
            }
        }

        public int width { get { return bit.Width; } }
        public int height { get { return bit.Height; } }

        public Bitmap bit = null;
        private Rectangle r;
        private IntPtr ptr;
        private byte[] pixels = null;
        private BitmapData bData = null;
    }
}
