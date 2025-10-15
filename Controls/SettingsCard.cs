using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MiFlashForWinUI3;

public sealed partial class SettingsCard : ContentControl
{
	public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(SettingsCard), new PropertyMetadata(string.Empty));

	public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(SettingsCard), new PropertyMetadata(string.Empty));

	public static readonly DependencyProperty IconGlyphProperty = DependencyProperty.Register("IconGlyph", typeof(string), typeof(SettingsCard), new PropertyMetadata("\xE793"));

	public static readonly DependencyProperty RightContentProperty = DependencyProperty.Register("RightContent", typeof(object), typeof(SettingsCard), new PropertyMetadata(null));

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public string Description
	{
		get => (string)GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}


	public string IconGlyph
	{
		get => (string)GetValue(IconGlyphProperty);
		set => SetValue(IconGlyphProperty, value);
	}

	public object RightContent
	{
		get => GetValue(RightContentProperty);
		set => SetValue(RightContentProperty, value);
	}

	public SettingsCard()
	{
		DefaultStyleKey = typeof(SettingsCard);
	}
}