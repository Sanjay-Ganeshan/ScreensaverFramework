using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreensaverFramework
{
    public abstract class ScreenSaverIterator
    {
        protected int width;
        protected int height;
        protected int currentIteration;
        protected int maxIteration;
        protected int frameRate;
        public ScreenSaverIterator(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.currentIteration = 1;
            this.maxIteration = 55;
            this.frameRate = 0;
        }
        public abstract void iterate();
        public abstract Bitmap draw();
        public abstract void setUp();
        public int getCurrentIteration()
        {
            return this.currentIteration;
        }
        public int getMaxIteration()
        {
            return this.maxIteration;
        }
        public int getFrameRate ()
        {
            return frameRate;
        }
    }
}
