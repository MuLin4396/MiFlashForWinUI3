using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.Storage.Pickers;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinRT.Interop;

namespace MiFlashForWinUI3
{
	public sealed partial class MainWindow : Window
	{
		public ObservableCollection<InventoryItem> InventoryItems { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

			if (AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this))).Presenter is OverlappedPresenter overlappedPresenter)
			{
				overlappedPresenter.SetBorderAndTitleBar(true, false);
			}

			ExtendsContentIntoTitleBar = true;

			AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

			SetTitleBar(AppTitleBar);

			InventoryItems = new ObservableCollection<InventoryItem>(GenerateInitialInventoryItems());
		}

		private IEnumerable<InventoryItem> GenerateInitialInventoryItems()
		{
			return new[]
			{
				new InventoryItem
				{
					ID      = "114514abc",
					Device  = "Xiaomi 15",
					Load    = 30,
					Time    = "66s",
					State   = "flash img",
					Results = "刷机中",
				}
			};
		}

		private string GetAppTitleFromSystem()
		{
			string appTitle = Package.Current.DisplayName;
			return appTitle;
		}

		private string GetVersionForBuild()
		{
			string name = Package.Current.PublisherDisplayName;
		#if DEBUG
			string build = "Debug";
		#elif RELEASE
			string build = "Release";
		#endif
			return $"{name} · {build}";
		}

		private void ChooseSocComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedItem = (ChooseSocComboBox.SelectedItem as ComboBoxItem)?.Tag as string;

			if (selectedItem == "Qualcomm")
			{
				QualcommGrid.Visibility = Visibility.Visible;
				MTKGrid.Visibility      = Visibility.Collapsed;
			}
			else if (selectedItem == "MTK")
			{
				QualcommGrid.Visibility = Visibility.Collapsed;
				MTKGrid.Visibility      = Visibility.Visible;
			}

			TextBox_Clear();
		}

		private async void QualcommButton_OnClick(object sender, RoutedEventArgs e)
		{
			QualcommButton.IsEnabled = false;

			var openPicker = new FolderPicker();
			var hWnd       = WindowNative.GetWindowHandle(new MainWindow());

			InitializeWithWindow.Initialize(openPicker, hWnd);

			openPicker.ViewMode               = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.Downloads;

			var folder = await openPicker.PickSingleFolderAsync();

			if (folder != null)
			{
				QualcommTextBox.Text = folder.Path;
			}
			else
			{
				QualcommTextBox.Text = "未选择文件";
			}

			QualcommButton.IsEnabled = true;
		}

		//todo 暂时搁置
		private async void MTKButton_OnClick(object sender, RoutedEventArgs e)
		{
			var senderButton = sender as Button;
			senderButton.IsEnabled = false;

			var openPicker = new FileOpenPicker();
			var hWnd       = WindowNative.GetWindowHandle(new MainWindow());

			InitializeWithWindow.Initialize(openPicker, hWnd);

			openPicker.ViewMode               = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.Downloads;

			openPicker.FileTypeFilter.Add((string)senderButton.Tag);

			var file = await openPicker.PickSingleFileAsync();

			if (file != null)
			{
				if (senderButton.Tag.ToString() == "img")
				{
					// TextBox.Text = folder.Path;
				}
			}
			else
			{
				// TextBox.Text = "未选择文件";
			}

			senderButton.IsEnabled = true;
		}

		//todo 暂时搁置
		private void TextBox_Clear()
		{
			QualcommTextBox.Text = string.Empty;
			// TextBox.Text         = string.Empty;
		}
	}

	public class InventoryItem
	{
		public string ID      { get; set; } = string.Empty;
		public string Device  { get; set; } = string.Empty;
		public int    Load    { get; set; }
		public string Time    { get; set; } = string.Empty;
		public string State   { get; set; } = string.Empty;
		public string Results { get; set; } = string.Empty;
	}
}