using System;
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

		private AppWindow appWindow;

		private OverlappedPresenter overlappedPresenter;

		public MainWindow()
		{
			InitializeComponent();

			InventoryItems = new ObservableCollection<InventoryItem>();

			SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

			appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));

			overlappedPresenter = (OverlappedPresenter)appWindow.Presenter;

			overlappedPresenter.SetBorderAndTitleBar(true, false);

			ExtendsContentIntoTitleBar = true;

			SetTitleBar(AppTitleBar);

			appWindow.Changed += AppWindow_Changed;
		}

		private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
		{
			if (overlappedPresenter.State is OverlappedPresenterState.Maximized) WindowMaximiseIcon.Glyph     = "\uE656";
			else if (overlappedPresenter.State is OverlappedPresenterState.Restored) WindowMaximiseIcon.Glyph = "\uE655";
		}

		private void WindowsButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button? senderButton = sender as Button;
			string? Tag          = senderButton.Tag as string;

			if (Tag == "WindowClose") Close();
			else if (Tag == "WindowMinimise") overlappedPresenter.Minimize();
			else if (Tag == "WindowMaximise")
				if (overlappedPresenter.State == OverlappedPresenterState.Restored) overlappedPresenter.Maximize();
				else if (overlappedPresenter.State == OverlappedPresenterState.Maximized) overlappedPresenter.Restore();
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

			QualcommTextBox.Text = string.Empty;
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

		public void AddInventoryItem(object sender, RoutedEventArgs e)
		{
			var a = new Random();

			var newItem = new InventoryItem
			{
				ID    = "2ad", Device    = "device", Load = a.Next(1, 100), Time = "45",
				State = "state", Results = "results"
			};

			InventoryItems.Add(newItem);
		}

		/// Tip 联发科
		// private async void MTKButton_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	var senderButton = sender as Button;
		// 	senderButton.IsEnabled = false;
		//
		// 	var openPicker = new FileOpenPicker();
		// 	var hWnd       = WindowNative.GetWindowHandle(new MainWindow());
		//
		// 	InitializeWithWindow.Initialize(openPicker, hWnd);
		//
		// 	openPicker.ViewMode               = PickerViewMode.Thumbnail;
		// 	openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
		//
		// 	openPicker.FileTypeFilter.Add((string)senderButton.Tag);
		//
		// 	var file = await openPicker.PickSingleFileAsync();
		//
		// 	if (file != null)
		// 	{
		// 		if (senderButton.Tag.ToString() == "img")
		// 		{
		// 			// TextBox.Text = folder.Path;
		// 		}
		// 	}
		// 	else
		// 	{
		// 		// TextBox.Text = "未选择文件";
		// 	}
		//
		// 	senderButton.IsEnabled = true;
		// }
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