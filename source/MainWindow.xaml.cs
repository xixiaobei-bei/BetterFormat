using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;

namespace BetterFormat
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow(string targetDrive)
        {
            this.InitializeComponent();
            Title = "BetterFormat";
            
            SetWindowIcon();
            PopulateDrives(targetDrive);
            TargetPartitionComboBox.SelectionChanged += TargetPartitionComboBox_SelectionChanged;

            CancelButton.Click += (s, e) => this.Close();
            OkButton.Click += OkButton_Click;
        }

        private void SetWindowIcon()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(wndId);
            appWindow.SetIcon("icon.ico");
        }

        private void PopulateDrives(string initialDrive)
        {
            var drives = DriveInfo.GetDrives();
            TargetPartitionComboBox.ItemsSource = drives;
            TargetPartitionComboBox.DisplayMemberPath = "Name";

            var initialSelection = drives.FirstOrDefault(d => d.Name.StartsWith(initialDrive.Substring(0, 1), StringComparison.OrdinalIgnoreCase));
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

        private void LoadDriveInfo(DriveInfo driveInfo)
        {
            try
            {
                if (driveInfo.IsReady)
                {
                    string sizeInfo = $"大小: {driveInfo.TotalSize / (1024 * 1024 * 1024)} GB";
                    string fsInfo = $"文件系统: {driveInfo.DriveFormat}";
                    PartitionInfoTextBlock.Text = $"{sizeInfo}, {fsInfo}";
                    VolumeLabelTextBox.Text = driveInfo.VolumeLabel;

                    // Set default file system
                    foreach (ComboBoxItem item in FileSystemComboBox.Items)
                    {
                        if (item.Content.ToString().Equals(driveInfo.DriveFormat, StringComparison.OrdinalIgnoreCase))
                        {
                            FileSystemComboBox.SelectedItem = item;
                            break;
                        }
                    }
                    OkButton.IsEnabled = true;
                }
                else
                {
                    PartitionInfoTextBlock.Text = "驱动器未就绪。";
                    OkButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                PartitionInfoTextBlock.Text = $"错误: {ex.Message}";
                OkButton.IsEnabled = false;
            }
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDrive = (TargetPartitionComboBox.SelectedItem as DriveInfo)?.Name;
            if (selectedDrive == null) return;

            var fileSystem = (FileSystemComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var volumeLabel = VolumeLabelTextBox.Text;
            var quickFormat = QuickFormatCheckBox.IsChecked ?? false;
            var allocationUnitSizeStr = (AllocationUnitComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(fileSystem) || string.IsNullOrEmpty(allocationUnitSizeStr))
            {
                await ShowMessage("请选择文件系统和分配单元大小。");
                return;
            }

            uint allocationUnitSize = allocationUnitSizeStr == "默认值" ? 0 : uint.Parse(allocationUnitSizeStr);

            var confirmationDialog = new ContentDialog
            {
                Title = "警告",
                Content = $"格式化将清除驱动器 {selectedDrive} 上的所有数据。确定要继续吗？",
                PrimaryButtonText = "确认",
                CloseButtonText = "取消",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await confirmationDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                SetControlsEnabled(false);
                try
                {
                    await FormatHelper.FormatDriveAsync(selectedDrive, fileSystem, volumeLabel, quickFormat, allocationUnitSize);
                    await ShowMessage("格式化成功！");
                    this.Close();
                }
                catch (Exception ex)
                {
                    await ShowMessage($"格式化失败: {ex.Message}");
                    SetControlsEnabled(true);
                }
            }
        }

        private void SetControlsEnabled(bool isEnabled)
        {
            FileSystemComboBox.IsEnabled = isEnabled;
            AllocationUnitComboBox.IsEnabled = isEnabled;
            VolumeLabelTextBox.IsEnabled = isEnabled;
            QuickFormatCheckBox.IsEnabled = isEnabled;
            OkButton.IsEnabled = isEnabled;
            CancelButton.IsEnabled = isEnabled;
        }

        private async Task ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Content = message,
                CloseButtonText = "好的",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
