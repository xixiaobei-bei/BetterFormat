using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Windowing;
using WinRT.Interop;

namespace BetterFormat
{
    public sealed partial class MainWindow : Window
    {
        private string _targetDrive;
        private bool _showAbout = true;

        public MainWindow(string targetDrive, bool showAbout = true)
        {
            this.InitializeComponent();
            Title = "BetterFormat";
            _targetDrive = targetDrive;
            _showAbout = showAbout;
            SetWindowIcon();

            MainNavView.SelectionChanged += MainNavView_SelectionChanged;
            MainWindow_Loaded(this, null);
        }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 控制关于菜单项显示
            if (!_showAbout && AboutMenuItem != null)
            {
                AboutMenuItem.Visibility = Visibility.Collapsed;
            }
            // 默认选中格式化
            foreach (var item in MainNavView.MenuItems)
            {
                if (item is NavigationViewItem navItem && navItem.Tag?.ToString() == "Format")
                {
                    MainNavView.SelectedItem = navItem;
                    break;
                }
            }
        }

        private void MainNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag?.ToString())
                {
                    case "Partition":
                        ContentFrame.Navigate(typeof(PartitionPage));
                        break;
                    case "Format":
                        ContentFrame.Navigate(typeof(FormatPage), _targetDrive);
                        break;
                    case "About":
                        ContentFrame.Navigate(typeof(AboutPage));
                        break;
                }
            }
        }
        private void SetWindowIcon()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            var wndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(wndId);
            appWindow.SetIcon("icon.ico");
        }
    }
}
