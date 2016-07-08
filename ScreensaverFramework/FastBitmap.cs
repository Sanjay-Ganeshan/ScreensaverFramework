using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreensaverFramework
{
    //This awesome class is directly copied from
    //http://johnparsons.net/index.php/2013/01/01/drawing-the-mandelbrot-fractal-in-c/
    //All credit goes to him
    public class FastBitmap
    {
        public FastBitmap(int width, int height)
        {
            this.Bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public unsafe void SetPixel(int x, int y, Color color)
        {
            BitmapData data = this.Bitmap.LockBits(new Rectangle(0, 0, this.Bitmap.Width, this.Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr scan0 = data.Scan0;

            byte* imagePointer = (byte*)scan0.ToPointer(); // Pointer to first pixel of image
            int offset = (y * data.Stride) + (3 * x); // 3x because we have 24bits/px = 3bytes/px
            byte* px = (imagePointer + offset); // pointer to the pixel we want
            px[0] = color.B; // Red component
            px[1] = color.G; // Green component
            px[2] = color.R; // Blue component

            this.Bitmap.UnlockBits(data); // Set the data again
        }

        public Bitmap Bitmap
        {
            get;
            set;
        }
    }
}
