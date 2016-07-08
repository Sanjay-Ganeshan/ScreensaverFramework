using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ScreensaverFramework
{
    //The code to draw and compute the Mandelbrot set was adapted from the code on this site here:
    //http://johnparsons.net/index.php/2013/01/01/drawing-the-mandelbrot-fractal-in-c/
    //No infringment intended.
    class MandelbrotIterativeDrawer : ScreenSaverIterator
    {
        Complex[,] z;
        Complex[,] c;
        bool[,] keepGoing;
        int[,] iterationsTillBroke;
        double rMin, rMax, iMin, iMax;

        int numColorsInBig = 30;
        List<Color> bigColorPalette;

        public MandelbrotIterativeDrawer(int width, int height, double rMin, double rMax, double iMin, double iMax): base(width, height)
        {
            this.rMin = rMin;
            this.rMax = rMax;
            this.iMin = iMin;
            this.iMax = iMax;
            this.maxIteration = 55;
        }

        public override void setUp()
        {
            c = new Complex[this.width, this.height];
            z = new Complex[this.width, this.height];
            keepGoing = new bool[this.width, this.height];
            iterationsTillBroke = new int[this.width, this.height];

            double rScale = (Math.Abs(rMin) + Math.Abs(rMax)) / this.width; // Amount to move each pixel in the real numbers
            double iScale = (Math.Abs(iMin) + Math.Abs(iMax)) / this.height; // Amount to move each pixel in the imaginary numbers

            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    c[i, j] = new Complex(i * rScale + rMin, j * iScale + iMin); // Scaled complex number
                    z[i, j] = new Complex(i * rScale + rMin, j * iScale + iMin); // Scaled complex number
                    keepGoing[i, j] = true;
                    iterationsTillBroke[i, j] = -1;
                }
            }
            currentIteration = 1;
            bigColorPalette = GenerateBigColorPalette();
        }

        public override void iterate()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (keepGoing[i, j])
                    {
                        if (z[i, j].Magnitude >= 2.0)
                        {
                            keepGoing[i, j] = false;
                            iterationsTillBroke[i, j] = currentIteration;
                        }
                        else
                        {
                            z[i, j] = c[i, j] + Complex.Pow(z[i, j], 2);
                        }
                    }
                }
            }
            currentIteration++;
        }

        private List<Color> GenerateColorPalette()
        {
            List<Color> retVal = new List<Color>();
            int numToMake = currentIteration + 1;
            int numBlack = numToMake / 5;
            int numColored = numToMake - numBlack;
            int degreesOfColor = 360 / numColored;
            for (int i = 0; i < numBlack; i++)
            {
                retVal.Add(Color.Black);
            }
            for (int i = 0; i <= numColored; i++)
            {
                double v = 0.1 + 0.9 / numColored * i;
                retVal.Add(AHSV(255, i * degreesOfColor, 1, v));
            }

            return retVal;
        }

        private List<Color> GenerateBigColorPalette()
        {
            List<Color> retVal = new List<Color>();

            for (int i = 0; i <= 360; i += 360 / numColorsInBig)
            {
                retVal.Add(AHSV(255, i));
            }

            return retVal;
        }

        public override Bitmap draw()
        {
            FastBitmap myBitmap = new FastBitmap(width, height);
            List<Color> palette = GenerateColorPalette();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!keepGoing[i, j])
                    {
                        myBitmap.SetPixel(i, j, palette[iterationsTillBroke[i, j] - 1]);
                    }
                    else
                    {
                        //myBitmap.SetPixel(i, j, palette[currentIteration]);
                    }
                }
            }
            return myBitmap.Bitmap;
        }

        private Color AHSV(int a, int h, double s = 1, double v = 1)
        {
            if (h < 0) h = 0;
            if (h > 360) h = 360;
            if (s < 0) s = 0;
            if (s > 1) s = 1;
            if (v < 0) v = 0;
            if (v > 1) v = 1;
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = v - c;
            double rp = 0.0, gp = 0.0, bp = 0.0;
            switch (h / 60)
            {
                case 0:
                    rp = c;
                    gp = x;
                    bp = 0;
                    break;
                case 1:
                    rp = x;
                    gp = c;
                    bp = 0;
                    break;
                case 2:
                    rp = 0;
                    gp = c;
                    bp = x;
                    break;
                case 3:
                    rp = 0;
                    gp = x;
                    bp = c;
                    break;
                case 4:
                    rp = x;
                    gp = 0;
                    bp = c;
                    break;
                case 5:
                    rp = c;
                    gp = 0;
                    bp = x;
                    break;
            }
            int r = Math.Max(0, Math.Min(255, (int)((rp + m) * 255.0)));
            int g = Math.Max(0, Math.Min(255, (int)((gp + m) * 255.0)));
            int b = Math.Max(0, Math.Min(255, (int)((bp + m) * 255.0)));
            return Color.FromArgb(a, r, g, b);
        }

    }
}
