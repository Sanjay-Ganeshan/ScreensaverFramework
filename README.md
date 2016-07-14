# ScreensaverFramework

##Intro
A framework for creating screensavers, incorporates the following functionality:

1. Handles the resizing and displaying of a screensaver
2. Closes the screensaver if the user moves their mouse or clicks
2. Makes creating a screensaver possible just by implementing a class with a draw() and an iterate() method
3. Compiles to a .exe file that can easily be changed to a .scr file

Features to come:

1. Configuration functionality
2. Automatic saving to Screensavers folder

It also includes a couple demo screensavers
+ Lines draws several colorful straight lines that form an illusion of a curve. The lines begin large enough to cover the screen, then gradually reduce in size.
+ Mandelbrot draws a Mandelbrot set, coloring each point based on the number of iterations until it is proven not to be in the set \(See [Mandelbrot on Wikipedia][] for more information\)

An example of the lines program running:
![Lines]

An example of the Mandelbrot program running:
![Mandelbrot]

[Lines]:  
./Lines.gif
[Mandelbrot]:  
./Mandelbrot.gif
[Mandelbrot on Wikipedia]:  https://en.wikipedia.org/wiki/Mandelbrot_set

##Requirements
Visual Studio, with tools for WPF (Windows Forms) desktop application development

##How to Use
1. First, fork/download this repository, and open the solution
2. Then, create a class, and subclass `ScreenSaverIterator`
3. Make a constructor for your class that includes at least width & height, and call the base constructor
4. Change the `maxIteration` and `frameRate` fields to the maximum number of frames to display before resetting your screensaver, and the number of frames per second (or 0 for as fast as possible)
5. Implement the `setUp()` function, which is run every time your screensaver is reset (once when it is beginning to display, then again every time `currentIteration` hits `maxIteration`)
6. Implement the `iterate()` function, which runs every frame, before draw. Put any update logic. If you need the current iteration, use the field `this.currentIteration`. 
**Be sure to include `this.currentIteration++` In order to track your current iteration!**
7. Implement the `draw()` function, which returns a Bitmap to be drawn. (Add any drawing logic here)
8. Modify the function `getNewScreensaver` in `ScreenSaverManager.cs` so that it constructs and returns an instance of your class with the given width, height, and any other parameters it needs (Configuration to be added later)
9. Run the program in Release mode
10. Move the `ScreensaverFramework.exe` file to some other location on your desktop (If it has dependancies, copy the whole folder)
11. Rename the .exe file so that it has the extension `.scr` (This is the extension for Windows screensavers)
12. Right click the new file, and click *Install* from the options menu.
13. Make sure that your screensaver is enabled. 
14. Enjoy looking like a pro.

You can use the following template, if you don't feel like writing this:

    public class MyScreenSaver: ScreenSaverIterator {
        public MyScreenSaver(width, height, ...): base(width,height) {
            this.maxIteration = 60;
            this.frameRate = 60;
        }
        public override void iterate() {
            //Put some update logic here
            this.currentIteration++;
        }
        public override Bitmap draw() {
            //Put some drawing logic here
            return new Bitmap(this.width, this.height);
        }
        public override void setUp() {
            //Put any resetting code here
        }
    }
    
##Tips

+ If you're drawing primitive things, use a Graphics object to modify your bitmap.
+ If you find an issue, raise it on the Issues tab
+ If you're getting errors about unsafe code, you have to enable it in your build properties. The managed (safe) Bitmap 
is extremely slow to set pixels, so the unsafe version is a lot better.


##Credits
The code for FastBitmap comes from [this](http://johnparsons.net/index.php/2013/01/01/drawing-the-mandelbrot-fractal-in-c/) website.
The Mandelbrot drawing code was also based on the code from that site.
