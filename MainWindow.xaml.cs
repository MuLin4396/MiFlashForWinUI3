using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.Storage.Pickers;
using MiFlashForWinUI3.Helper;
using WinRT.Interop;

namespace MiFlashForWinUI3
{
	public sealed partial class MainWindow
	{
		private const int MainWindowMinWidth = 800;

		private const int MainWindowMinHeight = 450;

		private const int DPI = 96;

		public ObservableCollection<InventoryItem> InventoryItems { get; set; }

		private readonly MainWindow _current;

		private readonly AppWindow _appWindow;

		private readonly OverlappedPresenter _overlappedPresenter;

		private string _romUrl = "";

		public MainWindow()
		{
			InitializeComponent();

			InventoryItems = new ObservableCollection<InventoryItem>();

			SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

			_appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));

			_overlappedPresenter = (OverlappedPresenter)_appWindow.Presenter;

			_overlappedPresenter.SetBorderAndTitleBar(true, false);

			ExtendsContentIntoTitleBar = true;

			SetTitleBar(AppTitleBar);

			_current = this;

			_appWindow.Changed += AppWindow_Changed;

			Closed += (s, e) => _appWindow.Changed -= AppWindow_Changed;
		}

		private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
		{
			if (_overlappedPresenter.State is OverlappedPresenterState.Maximized) WindowMaximiseIcon.Glyph     = "\uE656";
			else if (_overlappedPresenter.State is OverlappedPresenterState.Restored) WindowMaximiseIcon.Glyph = "\uE655";

			int x = ScreenDPIHelper.DpiX;

			int MinWidth  = (MainWindowMinWidth * x) / DPI;
			int MinHeight = (MainWindowMinHeight * x) / DPI;

			TextBlock11111.Text = ($"设定窗口最小分辨率: {MainWindowMinWidth} x {MainWindowMinHeight}\n" + $"实际窗口最小分辨率: {MinWidth} x {MinHeight}\n" + $"工作区域分辨率: {ScreenDPIHelper.WorkingArea.Width} x {ScreenDPIHelper.WorkingArea.Height}\n" + $"桌面分辨率: {ScreenDPIHelper.DesktopResolution.Width} x {ScreenDPIHelper.DesktopResolution.Height}\n" + $"X轴DPI: {ScreenDPIHelper.DpiX}\n" + $"Y轴DPI: {ScreenDPIHelper.DpiY}\n" + $"X轴缩放比例: {ScreenDPIHelper.ScaleX:F2}\n" + $"Y轴缩放比例: {ScreenDPIHelper.ScaleY:F2}");

			SetWindowMaxMin(MinWidth, MinHeight);
		}

		private void ShowDpiInfo(object sender, RoutedEventArgs e)
		{
			var    displayInfo = DisplayInformation.GetForCurrentView();
			double dpi         = displayInfo.RawDpiX;

			TextBlock11111.Text = $"DPI: {dpi}\n缩放: scale:P0";
		}

		private void WindowsButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button? senderButton = sender as Button;
			string? Tag          = senderButton.Tag as string;

			switch (Tag)
			{
				// case "WindowTaskbar":
				case "WindowClose": Close(); break;
				case "WindowMinimise": _overlappedPresenter.Minimize(); break;
				case "WindowMaximise" when _overlappedPresenter.State == OverlappedPresenterState.Restored: _overlappedPresenter.Maximize(); break;
				case "WindowMaximise" when _overlappedPresenter.State == OverlappedPresenterState.Maximized: _overlappedPresenter.Restore(); break;
				case "WindowTheme":
					if (_current.Content is FrameworkElement rootElement) rootElement.RequestedTheme = (rootElement.ActualTheme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light);
					break;
			}
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

		private void SetWindowMaxMin(int minWidth = 0, int minHeight = 0, int maxWidth = 0, int maxHeight = 0)
		{
			_overlappedPresenter.PreferredMinimumWidth  = minWidth != 0 ? minWidth : _overlappedPresenter.PreferredMinimumWidth;
			_overlappedPresenter.PreferredMinimumHeight = minHeight != 0 ? minHeight : _overlappedPresenter.PreferredMinimumHeight;
			_overlappedPresenter.PreferredMaximumWidth  = maxWidth != 0 ? maxWidth : _overlappedPresenter.PreferredMaximumWidth;
			_overlappedPresenter.PreferredMaximumHeight = maxHeight != 0 ? maxHeight : _overlappedPresenter.PreferredMaximumHeight;
		}

		private void AddInventoryItem(object sender, RoutedEventArgs e)
		{
			var a = new Random();

			var newItem = new InventoryItem { ID = $"{InventoryItems.Count + 1}", Device = "8fd7d29", Load = a.Next(1, 100), Time = "45", State = "state" };
			// , Results = "results" 

			InventoryItems.Add(newItem);
		}

		private void delInventoryItem2(object sender, RoutedEventArgs e)
		{
			InventoryItems.Clear();
		}

		private async void TextClear(object sender, RoutedEventArgs e)
		{
			await TextClear1();

			string a = "Clear End";

			await TextWrite1(a);
		}

		private async void TextWrite(object sender, RoutedEventArgs e)
		{
			await TextClear1();

			string a = "Awdadwad\nAwadwa\n\tAdawdwd";

			await TextWrite1(a);
		}

		private async Task TextClear1()
		{
			// 检查文本是否为空
			while (!string.IsNullOrEmpty(TextBlock11111.Text))
			{
				// 逐字删除（从右向左删除最后一个字符）
				TextBlock11111.Text = TextBlock11111.Text.Substring(0, TextBlock11111.Text.Length - 1);
				await Task.Delay(10);
			}
		}

		private async Task TextWrite1(string text)
		{
			string        a           = text;
			StringBuilder currentText = new StringBuilder();

			for (int i = 0; i < a.Length; i++)
			{
				currentText.Append(a[i]);
				TextBlock11111.Text = currentText.ToString();
				await Task.Delay(10);
			}
		}

		private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ComboBoxmode.SelectedItem is ComboBoxItem selectedItem) SettingsCardMode.Description = "Bat: " + selectedItem.Tag?.ToString() ?? string.Empty;
		}

		private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			aButton.IsEnabled = false;

			var picker = new FolderPicker(aButton.XamlRoot.ContentIslandEnvironment.AppWindowId);

			picker.CommitButtonText       = "选择文件";
			picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			picker.ViewMode               = PickerViewMode.List;

			var folder = await picker.PickSingleFolderAsync();

			if (folder != null)
			{
				SettingsCard111.Description = "Picked: " + folder.Path;

				_romUrl = folder.Path;

				aButton.Visibility = Visibility.Collapsed;
				bButton.IsEnabled  = true;
				bButton.Visibility = Visibility.Visible;
			}
			else
			{
				_romUrl = "";

				SettingsCard111.Description = "选择包地址啊";

				aButton.IsEnabled = true;
			}

			// SettingsCard111.Description = folder != null ? "Picked: " + folder.Path : "选择包地址啊";
		}

		private void BButton_OnClick(object sender, RoutedEventArgs e)
		{
			ClearRomUrl();
		}

		private void ClearRomUrl()
		{
			aButton.IsEnabled  = true;
			aButton.Visibility = Visibility.Visible;

			bButton.IsEnabled  = false;
			bButton.Visibility = Visibility.Collapsed;

			SettingsCard111.Description = "选择包地址啊";
		}

		private void ClearFlashMode()
		{
			SettingsCardMode.Description       = "选择一下啊";
			ComboBoxmode.SelectedIndex         = -1;
			SettingsCardQualcomm.SelectedIndex = -1;
		}

		private void MenuFlyoutItem_OnClick(object sender, RoutedEventArgs e)
		{
			ClearFlashMode();
			ClearRomUrl();
		}

		private void MenuFlyoutItem1_OnClick(object sender, RoutedEventArgs e)
		{
			Border11.Visibility = Border11.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
		}
	}

	public class InventoryItem
	{
		public string ID     { get; set; } = string.Empty;
		public string Device { get; set; } = string.Empty;
		public int    Load   { get; set; }
		public string Time   { get; set; } = string.Empty;

		public string State { get; set; } = string.Empty;
		// public string Results { get; set; } = string.Empty;
	}
}