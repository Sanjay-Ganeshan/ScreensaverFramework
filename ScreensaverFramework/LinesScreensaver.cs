using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ScreensaverFramework
{
    class LinesScreensaver : ScreenSaverIterator
    {
        Bitmap theBitmap;
        Graphics g;
        int numLinesPerSide;
        int currWidth;
        int currHeight;
        int currZeroX;
        int currZeroY;
        int currentSectionIteration;
        bool isDecreasingInSize;
        bool shouldAlternate;
        List<List<Pen>> pensToDrawWith;

        public LinesScreensaver(int width, int height, int numLinesPerSide, int numTimesToDraw, bool shouldAlternate = true, int colorHueOffset = 0): base(width,height)
        {
            this.numLinesPerSide = numLinesPerSide;
            if(shouldAlternate)
                this.maxIteration = numLinesPerSide * numTimesToDraw * 2;
            else
                this.maxIteration = numLinesPerSide * numTimesToDraw;
            this.shouldAlternate = shouldAlternate;
            this.frameRate = 30;
            List<Color> palette;
            List<Pen> pens;
            pensToDrawWith = new List<List<Pen>>();
            for (int i = 3; i > 0; i--)
            {
                palette = generateColorPalette(numLinesPerSide, colorHueOffset, 1.0 - (0.3 * i));
                pens = new List<Pen>();
                foreach (Color c in palette)
                {
                    pens.Add(new Pen(c, i));
                }
                pensToDrawWith.Add(pens);
            }
            
        }

        public override Bitmap draw()
        {
            return this.theBitmap;
        }

        public override void iterate()
        {
            foreach (List<Pen> penSet in pensToDrawWith)
            {
                Pen thePen = penSet[currentSectionIteration];
                float xD = (float)(this.currWidth) / this.numLinesPerSide;
                float yD = (float)(this.currHeight) / this.numLinesPerSide;
                if (this.isDecreasingInSize)
                {
                    g.DrawLine(thePen, currZeroX, currZeroY + yD * currentSectionIteration, currZeroX + xD * currentSectionIteration, currZeroY + currHeight);
                    g.DrawLine(thePen, currZeroX + currWidth, currZeroY + yD * (numLinesPerSide - currentSectionIteration), currZeroX + currWidth - xD * (numLinesPerSide - currentSectionIteration), currZeroY + currHeight);
                    g.DrawLine(thePen, currZeroX, currZeroY + currHeight - yD * (numLinesPerSide - currentSectionIteration), currZeroX + xD * (numLinesPerSide - currentSectionIteration), currZeroY);
                    g.DrawLine(thePen, currZeroX + currWidth, currZeroY + currHeight - yD * (currentSectionIteration), currZeroX + currWidth - xD * (currentSectionIteration), currZeroY);
                }
                else
                {
                    g.DrawLine(thePen, currZeroX, currZeroY + yD * (numLinesPerSide - currentSectionIteration), currZeroX + xD * (numLinesPerSide - currentSectionIteration), currZeroY + currHeight);
                    g.DrawLine(thePen, currZeroX + currWidth, currZeroY + yD * currentSectionIteration, currZeroX + currWidth - xD * currentSectionIteration, currZeroY + currHeight);
                    g.DrawLine(thePen, currZeroX, currZeroY + currHeight - yD * currentSectionIteration, currZeroX + xD * currentSectionIteration, currZeroY);
                    g.DrawLine(thePen, currZeroX + currWidth, currZeroY + currHeight - yD * (numLinesPerSide - currentSectionIteration), currZeroX + currWidth - xD * (numLinesPerSide - currentSectionIteration), currZeroY);
                }
            }
            currentIteration++;
            currentSectionIteration++;
            
            if(currentSectionIteration == numLinesPerSide)
            {
                if (this.shouldAlternate && this.currentIteration == this.maxIteration / 2)
                {
                    g.Clear(Color.Black);
                    isDecreasingInSize = !isDecreasingInSize;
                    currentSectionIteration = 0;
                }
                else {
                    if (isDecreasingInSize)
                    {
                        currentSectionIteration = 0;
                        currWidth /= 2;
                        currHeight /= 2;
                        this.currZeroX += currWidth / 2;
                        this.currZeroY += currHeight / 2;
                    }
                    else
                    {
                        currentSectionIteration = 0;
                        this.currZeroX -= currWidth / 2;
                        this.currZeroY -= currHeight / 2;
                        currWidth *= 2;
                        currHeight *= 2;
                    }
                }
            }

        }

        public override void setUp()
        {
            if(g != null)
            {
                g.Dispose();
                g = null;
            }
            if(theBitmap != null)
            {
                theBitmap.Dispose();
                theBitmap = null;
            }
            currentIteration = 0;
            theBitmap = new Bitmap(this.width, this.height);
            currZeroX = 0;
            currZeroY = 0;
            currWidth = width;
            currHeight = height;
            isDecreasingInSize = true;
            this.currentSectionIteration = 0;
            g = Graphics.FromImage(theBitmap);
            g.Clear(Color.Black);
        }

        private List<Color> generateColorPalette(int numColors, int offset = 0, double v = 1)
        {
            List<Color> palette = new List<Color>();
            int currH = offset;
            int addPerI = 360 / (numColors);
            for(int i = 0; i < numColors; i++)
            {
                palette.Add(AHSV(255, currH,1,v));
                currH += addPerI;
                currH %= 360;
            }
            return palette;
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
