using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MiFlashForWinUI3;

public sealed partial class BarTitle : ContentControl
{
	public static readonly DependencyProperty IconGlyphProperty = DependencyProperty.Register("IconGlyph", typeof(string), typeof(BarTitle), new PropertyMetadata("\xE793"));

	public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(BarTitle), new PropertyMetadata(string.Empty));

	public static readonly DependencyProperty RightContentProperty = DependencyProperty.Register("RightContent", typeof(object), typeof(BarTitle), new PropertyMetadata(null));

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
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

	public BarTitle()
	{
		DefaultStyleKey = typeof(BarTitle);
	}
}