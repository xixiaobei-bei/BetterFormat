using Microsoft.UI.Xaml;
using System;
using System.Linq;

namespace BetterFormat
{
    public partial class App : Application
    {
        private Window m_window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            string targetDrive = "C:\\"; // Default value
            string[] cmdLineArgs = Environment.GetCommandLineArgs();

            // The first argument is the executable path, the second should be our drive.
            if (cmdLineArgs.Length > 1)
            {
                targetDrive = cmdLineArgs[1];
            }

            m_window = new MainWindow(targetDrive);
            m_window.Activate();
        }
    }
}
