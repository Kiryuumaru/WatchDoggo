using DoggoWire.Models;
using DoggoWire.Services;
using System;
using System.Windows.Forms;
using WatchDoggo.Forms;

namespace WatchDoggo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
