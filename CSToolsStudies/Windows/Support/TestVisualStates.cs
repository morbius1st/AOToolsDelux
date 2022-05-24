#region + Using Directives

using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

#endregion

// user name: jeffs
// created:   12/30/2020 11:11:08 PM

namespace CSToolsStudies.Windows.Support
{
	public class TestVisualStates : DependencyObject
	{

	#region general - title text

		public static readonly DependencyProperty 
			TitleTextProperty = DependencyProperty.RegisterAttached(
				"TitleText", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTitleText(UIElement e, string value)
		{
			e.SetValue(TitleTextProperty, value);
		}

		public static string GetTitleText(UIElement e)
		{
			return (string) e.GetValue(TitleTextProperty);
		}

	#endregion

	#region general - title text

		public static readonly DependencyProperty 
			MainContentProperty = DependencyProperty.RegisterAttached(
				"MainContent", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetMainContent(UIElement e, string value)
		{
			e.SetValue(MainContentProperty, value);
		}

		public static string GetMainContent(UIElement e)
		{
			return (string) e.GetValue(MainContentProperty);
		}

	#endregion



	#region general - does mouse over

		public static readonly DependencyProperty 
			DoesMouseOverProperty = DependencyProperty.RegisterAttached(
				"DoesMouseOver", typeof(bool), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(false, 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetDoesMouseOver(UIElement e, bool value)
		{
			e.SetValue(DoesMouseOverProperty, value);
		}

		public static bool GetDoesMouseOver(UIElement e)
		{
			return (bool) e.GetValue(DoesMouseOverProperty);
		}

	#endregion



	#region default - background (any)

		public static readonly DependencyProperty 
			BgDefaultProperty = DependencyProperty.RegisterAttached(
				"BgDefault", typeof(SolidColorBrush), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgDefault(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgDefaultProperty, value);
		}

		public static SolidColorBrush GetBgDefault(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgDefaultProperty);
		}

	#endregion

	#region default - foreground (any)

		public static readonly DependencyProperty 
			FgDefaultProperty = DependencyProperty.RegisterAttached(
				"FgDefault", typeof(SolidColorBrush), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgDefault(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgDefaultProperty, value);
		}

		public static SolidColorBrush GetFgDefault(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgDefaultProperty);
		}

	#endregion

	#region default - textblock or textbox text

		public static readonly DependencyProperty 
			TbTextDefaultProperty = DependencyProperty.RegisterAttached(
				"TbTextDefault", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTbTextDefault(UIElement e, string value)
		{
			e.SetValue(TbTextDefaultProperty, value);
		}

		public static string GetTbTextDefault(UIElement e)
		{
			return (string) e.GetValue(TbTextDefaultProperty);
		}

	#endregion

	#region default - icon geometry

		public static readonly DependencyProperty 
			IconGeometryDefaultProperty = DependencyProperty.RegisterAttached(
				"IconGeometryDefault", typeof(Geometry), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new PathGeometry(), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryDefault(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryDefaultProperty, value);
		}

		public static Geometry GetIconGeometryDefault(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryDefaultProperty);
		}

	#endregion


	#region is_selected - background (any)

		public static readonly DependencyProperty 
			BgIsSelectedProperty = DependencyProperty.RegisterAttached(
			"BgIsSelected", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsSelected(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsSelectedProperty, value);
		}

		public static SolidColorBrush GetBgIsSelected(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsSelectedProperty);
		}

	#endregion
		
	#region is_selected - foreground (any)

		public static readonly DependencyProperty 
			FgIsSelectedProperty = DependencyProperty.RegisterAttached(
			"FgIsSelected", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsSelected(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsSelectedProperty, value);
		}

		public static SolidColorBrush GetFgIsSelected(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsSelectedProperty);
		}

	#endregion

	#region is_selected - textblock or textbox text

		public static readonly DependencyProperty 
			TbTextIsSelectedProperty = DependencyProperty.RegisterAttached(
				"TbTextIsSelected", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTbTextIsSelected(UIElement e, string value)
		{
			e.SetValue(TbTextIsSelectedProperty, value);
		}

		public static string GetTbTextIsSelected(UIElement e)
		{
			return (string) e.GetValue(TbTextIsSelectedProperty);
		}

	#endregion

	#region is_selected - icon geometry

		public static readonly DependencyProperty 
			IconGeometryIsSelectedProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsSelected", typeof(Geometry), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new PathGeometry(), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryIsSelected(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryIsSelectedProperty, value);
		}

		public static Geometry GetIconGeometryIsSelected(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryIsSelectedProperty);
		}

	#endregion


	#region is_editing - background (any)

		public static readonly DependencyProperty 
			BgIsEditingProperty = DependencyProperty.RegisterAttached(
			"BgIsEditing", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsEditingProperty, value);
		}

		public static SolidColorBrush GetBgIsEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsEditingProperty);
		}

	#endregion

	#region is_editing - mouse over background

		public static readonly DependencyProperty 
			MoIsEditingProperty = DependencyProperty.RegisterAttached(
			"MoIsEditing", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetMoIsEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(MoIsEditingProperty, value);
		}

		public static SolidColorBrush GetMoIsEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(MoIsEditingProperty);
		}

	#endregion
		
	#region is_editing - foreground (any)

		public static readonly DependencyProperty 
			FgIsEditingProperty = DependencyProperty.RegisterAttached(
			"FgIsEditing", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsEditingProperty, value);
		}

		public static SolidColorBrush GetFgIsEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsEditingProperty);
		}

	#endregion



	#region is_editing - textblock or textbox text

		public static readonly DependencyProperty 
			TbTextIsEditingProperty = DependencyProperty.RegisterAttached(
				"TbTextIsEditing", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTbTextIsEditing(UIElement e, string value)
		{
			e.SetValue(TbTextIsEditingProperty, value);
		}

		public static string GetTbTextIsEditing(UIElement e)
		{
			return (string) e.GetValue(TbTextIsEditingProperty);
		}

	#endregion

	#region is_editing - icon geometry

		public static readonly DependencyProperty 
			IconGeometryIsEditingProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsEditing", typeof(Geometry), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new PathGeometry(), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryIsEditing(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryIsEditingProperty, value);
		}

		public static Geometry GetIconGeometryIsEditing(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryIsEditingProperty);
		}

	#endregion


	#region no_editing - background (any)

		public static readonly DependencyProperty 
			BgNoEditingProperty = DependencyProperty.RegisterAttached(
			"BgNoEditing", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgNoEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgNoEditingProperty, value);
		}

		public static SolidColorBrush GetBgNoEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgNoEditingProperty);
		}

	#endregion
		
	#region no_editing - foreground (any)

		public static readonly DependencyProperty 
			FgNoEditingProperty = DependencyProperty.RegisterAttached(
			"FgNoEditing", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgNoEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgNoEditingProperty, value);
		}

		public static SolidColorBrush GetFgNoEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgNoEditingProperty);
		}

	#endregion

	#region no_editing - textblock or textbox text

		public static readonly DependencyProperty 
			TbTextNoEditingProperty = DependencyProperty.RegisterAttached(
				"TbTextNoEditing", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTbTextNoEditing(UIElement e, string value)
		{
			e.SetValue(TbTextNoEditingProperty, value);
		}

		public static string GetTbTextNoEditing(UIElement e)
		{
			return (string) e.GetValue(TbTextNoEditingProperty);
		}

	#endregion

	#region no_editing - icon geometry

		public static readonly DependencyProperty 
			IconGeometryNoEditingProperty = DependencyProperty.RegisterAttached(
				"IconGeometryNoEditing", typeof(Geometry), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new PathGeometry(), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryNoEditing(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryNoEditingProperty, value);
		}

		public static Geometry GetIconGeometryNoEditing(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryNoEditingProperty);
		}

	#endregion


	#region is_locked - background (any)

		public static readonly DependencyProperty 
			BgIsLockedProperty = DependencyProperty.RegisterAttached(
			"BgIsLocked", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsLocked(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsLockedProperty, value);
		}

		public static SolidColorBrush GetBgIsLocked(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsLockedProperty);
		}

	#endregion
		
	#region is_locked - foreground (any)

		public static readonly DependencyProperty 
			FgIsLockedProperty = DependencyProperty.RegisterAttached(
			"FgIsLocked", typeof(SolidColorBrush), typeof(TestVisualStates), 
			new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsLocked(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsLockedProperty, value);
		}

		public static SolidColorBrush GetFgIsLocked(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsLockedProperty);
		}

	#endregion

	#region is_locked - textblock or textbox text

		public static readonly DependencyProperty 
			TbTextIsLockedProperty = DependencyProperty.RegisterAttached(
				"TbTextIsLocked", typeof(string), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata("", 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTbTextIsLocked(UIElement e, string value)
		{
			e.SetValue(TbTextIsLockedProperty, value);
		}

		public static string GetTbTextIsLocked(UIElement e)
		{
			return (string) e.GetValue(TbTextIsLockedProperty);
		}

	#endregion

	#region is_locked - icon geometry

		public static readonly DependencyProperty 
			IconGeometryIsLockedProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsLocked", typeof(Geometry), typeof(TestVisualStates), 
				new FrameworkPropertyMetadata(new PathGeometry(), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryIsLocked(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryIsLockedProperty, value);
		}

		public static Geometry GetIconGeometryIsLocked(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryIsLockedProperty);
		}

	#endregion



	}
}