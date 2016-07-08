using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreensaverFramework
{
    public static class ScreenSaverManager
    {
        public static ScreenSaverIterator getNewScreensaver(int width, int height)
        {
            //return null; //REPLACE THIS WITH A NEW INSTANCE OF YOUR SCREENSAVER CLASS
            return new MandelbrotIterativeDrawer(width, height, -2.5, 1.0, -1.0, 1.0);
            //return new LinesScreensaver(width, height, 20, 6, true, 150);
        }
    }
}
