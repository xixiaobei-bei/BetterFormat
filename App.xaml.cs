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
            bool showAbout = true;
            string[] cmdLineArgs = Environment.GetCommandLineArgs();

            // The first argument is the executable path, the second should be our drive.
            if (cmdLineArgs.Length > 1)
            {
                targetDrive = cmdLineArgs[1];
                showAbout = false; // 右键分区打开时不显示关于
            }

            m_window = new MainWindow(targetDrive, showAbout);
            m_window.Activate();
        }
    }
}
