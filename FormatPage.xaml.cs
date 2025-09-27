using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Linq;

namespace BetterFormat
{
    public sealed partial class FormatPage : Page
    {
        private string _targetDrive;
        public FormatPage()
        {
            this.InitializeComponent();
            this.Loaded += FormatPage_Loaded;
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string drive)
            {
                _targetDrive = drive;
            }
        }

        private void FormatPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateDrives(_targetDrive);
            TargetPartitionComboBox.SelectionChanged += TargetPartitionComboBox_SelectionChanged;
            CancelButton.Click += (s, ev) =>
            {
                var window = (Microsoft.UI.Xaml.Window.Current as Window);
                window?.Close();
            };
            OkButton.Click += OkButton_Click;
        }

        private void PopulateDrives(string initialDrive)
        {
            var drives = DriveInfo.GetDrives();
            TargetPartitionComboBox.ItemsSource = drives;
            TargetPartitionComboBox.DisplayMemberPath = "Name";

            var initialSelection = drives.FirstOrDefault(d => d.Name.StartsWith(initialDrive?.Substring(0, 1) ?? "C", StringComparison.OrdinalIgnoreCase));
            if (initialSelection != null)
            {
                TargetPartitionComboBox.SelectedItem = initialSelection;
            }
            else if (drives.Any())
            {
                TargetPartitionComboBox.SelectedIndex = 0;
            }
        }

        private void TargetPartitionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TargetPartitionComboBox.SelectedItem is DriveInfo selectedDrive)
            {
                LoadDriveInfo(selectedDrive);
            }
        }

        private void LoadDriveInfo(DriveInfo drive)
        {
            PartitionInfoTextBlock.Text = $"大小: {drive.TotalSize / (1024 * 1024 * 1024)} GB, 文件系统: {drive.DriveFormat}";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 格式化逻辑
        }
    }
}