using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreensaverFramework
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                // Get the 2 character command line argument
                string arg = args[0].ToLowerInvariant().Trim().Substring(0, 2);
                switch (arg)
                {
                    case "/c":
                        // Show the options dialog
                        ShowOptions();
                        break;
                    case "/p":
                        // Don't do anything for preview
                        break;
                    case "/s":
                        // Show screensaver form
                        ShowScreenSaver();
                        break;
                    case "/d":
                        // Show screensver in debug mode
                        ShowScreenSaver();
                        break;
                    default:
                        MessageBox.Show("Invalid command line argument: " + arg, "Invalid Command Line Argument", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            else
            {
                // If no arguments were passed in, show the screensaver
                ShowScreenSaver();
            }
        }
        static void ShowScreenSaver()
        {
            Form1 screenSaver = new Form1();
            Application.Run(screenSaver);
        }
        static void ShowOptions()
        {
            MessageBox.Show("No configurable options");
            Application.Exit();
        }
    }
}
