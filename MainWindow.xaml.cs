using Windows.ApplicationModel;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace App2
{
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

			ExtendsContentIntoTitleBar = true;

			AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

			SetTitleBar(AppTitleBar);
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
			var selectedItem = ChooseSocComboBox.SelectedItem as string;

			if (selectedItem == "Qualcomm")
			{
				QualcommGrid.Visibility = Visibility.Visible;
				MTKGrid.Visibility = Visibility.Collapsed;
			}
			else if (selectedItem == "MTK")
			{
				QualcommGrid.Visibility = Visibility.Collapsed;
				MTKGrid.Visibility = Visibility.Visible;
			}
		}
	}
}