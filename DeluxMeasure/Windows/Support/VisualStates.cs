#region + Using Directives

using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

#endregion

// user name: jeffs
// created:   12/30/2020 11:11:08 PM

namespace DeluxMeasure.Windows.Support
{
	public class VisualStates : DependencyObject
	{
	#region general - title text

		public static readonly DependencyProperty
			TitleTextProperty = DependencyProperty.RegisterAttached(
				"TitleText", typeof(string), typeof(VisualStates),
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
				"MainContent", typeof(string), typeof(VisualStates),
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
				"DoesMouseOver", typeof(bool), typeof(VisualStates),
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

	#region general - is_selected

		public static readonly DependencyProperty
			IsSelectedProperty = DependencyProperty.RegisterAttached(
				"IsSelected", typeof(bool), typeof(VisualStates),
				new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIsSelected(UIElement e, bool value)
		{
			e.SetValue(IsSelectedProperty, value);
		}

		public static bool GetIsSelected(UIElement e)
		{
			return (bool) e.GetValue(IsSelectedProperty);
		}

	#endregion
		
	#region general - is_focused

		public static readonly DependencyProperty
			IsFocusedProperty = DependencyProperty.RegisterAttached(
				"IsFocused", typeof(bool), typeof(VisualStates),
				new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIsFocused(UIElement e, bool value)
		{
			e.SetValue(IsFocusedProperty, value);
		}

		public static bool GetIsFocused(UIElement e)
		{
			return (bool) e.GetValue(IsFocusedProperty);
		}

	#endregion

	#region general - is_readonly

		public static readonly DependencyProperty
			IsReadOnlyProperty = DependencyProperty.RegisterAttached(
				"IsReadOnly", typeof(bool), typeof(VisualStates),
				new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIsReadOnly(UIElement e, bool value)
		{
			e.SetValue(IsReadOnlyProperty, value);
		}

		public static bool GetIsReadOnly(UIElement e)
		{
			return (bool) e.GetValue(IsReadOnlyProperty);
		}

	#endregion

	#region general - is_locked

		public static readonly DependencyProperty
			IsLockedProperty = DependencyProperty.RegisterAttached(
				"IsLocked", typeof(bool), typeof(VisualStates),
				new FrameworkPropertyMetadata(false,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIsLocked(UIElement e, bool value)
		{
			e.SetValue(IsLockedProperty, value);
		}

		public static bool GetIsLocked(UIElement e)
		{
			return (bool) e.GetValue(IsLockedProperty);
		}

	#endregion
		
	#region general - is_goodbad (& null)

		public static readonly DependencyProperty
			IsGoodBadProperty = DependencyProperty.RegisterAttached(
				"IsGoodBad", typeof(bool?), typeof(VisualStates),
				new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIsGoodBad(UIElement e, bool? value)
		{
			e.SetValue(IsGoodBadProperty, value);
		}

		public static bool? GetIsGoodBad(UIElement e)
		{
			return (bool?) e.GetValue(IsGoodBadProperty);
		}

	#endregion


		// color properties

		// disabled

	#region disabled - background (any)

		public static readonly DependencyProperty
			BgDisabledProperty = DependencyProperty.RegisterAttached(
				"BgDisabled", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgDisabled(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgDisabledProperty, value);
		}

		public static SolidColorBrush GetBgDisabled(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgDisabledProperty);
		}

	#endregion

	#region disabled - foreground (any)

		public static readonly DependencyProperty
			FgDisabledProperty = DependencyProperty.RegisterAttached(
				"FgDisabled", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgDisabled(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgDisabledProperty, value);
		}

		public static SolidColorBrush GetFgDisabled(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgDisabledProperty);
		}

	#endregion

	#region disabled - borderbrush

		public static readonly DependencyProperty
			BdrDisabledProperty = DependencyProperty.RegisterAttached(
				"BdrDisabled", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrDisabled(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrDisabledProperty, value);
		}

		public static SolidColorBrush GetBdrDisabled(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrDisabledProperty);
		}

	#endregion

	#region disabled - icon geometry

		public static readonly DependencyProperty
			IconGeomDisabledProperty = DependencyProperty.RegisterAttached(
				"IconGeomDisabled", typeof(Geometry), typeof(VisualStates),
				new FrameworkPropertyMetadata(new PathGeometry(),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeomDisabled(UIElement e, Geometry value)
		{
			e.SetValue(IconGeomDisabledProperty, value);
		}

		public static Geometry GetIconGeomDisabled(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeomDisabledProperty);
		}

	#endregion

		// not selected (enabled / not selected)

	#region not_sel - background (any)

		public static readonly DependencyProperty
			BgNotSelProperty = DependencyProperty.RegisterAttached(
				"BgNotSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgNotSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgNotSelProperty, value);
		}

		public static SolidColorBrush GetBgNotSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgNotSelProperty);
		}

	#endregion

	#region not_sel - foreground (any)

		public static readonly DependencyProperty
			FgNotSelProperty = DependencyProperty.RegisterAttached(
				"FgNotSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgNotSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgNotSelProperty, value);
		}

		public static SolidColorBrush GetFgNotSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgNotSelProperty);
		}

	#endregion

	#region not_sel - borderbrush

		public static readonly DependencyProperty
			BdrNotSelProperty = DependencyProperty.RegisterAttached(
				"BdrNotSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrNotSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrNotSelProperty, value);
		}

		public static SolidColorBrush GetBdrNotSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrNotSelProperty);
		}

	#endregion

	#region not_sel - icon geometry

		public static readonly DependencyProperty
			IconGeometryNotSelProperty = DependencyProperty.RegisterAttached(
				"IconGeometryNotSel", typeof(Geometry), typeof(VisualStates),
				new FrameworkPropertyMetadata(new PathGeometry(),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryNotSel(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryNotSelProperty, value);
		}

		public static Geometry GetIconGeometryNotSel(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryNotSelProperty);
		}

	#endregion


	#region not_sel - mouse over background

		public static readonly DependencyProperty
			BgNotSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"BgNotSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgNotSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgNotSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetBgNotSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgNotSelMouseOverProperty);
		}

	#endregion

	#region not_sel - mouse over foreground

		public static readonly DependencyProperty
			FgNotSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"FgNotSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgNotSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgNotSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetFgNotSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgNotSelMouseOverProperty);
		}

	#endregion

	#region not_sel - mouse over borderbrush

		public static readonly DependencyProperty
			BdrNotSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"BdrNotSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrNotSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrNotSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetBdrNotSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrNotSelMouseOverProperty);
		}

	#endregion


		// enabled & selected

	#region is_sel- background (any)

		public static readonly DependencyProperty
			BgIsSelProperty = DependencyProperty.RegisterAttached(
				"BgIsSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsSelProperty, value);
		}

		public static SolidColorBrush GetBgIsSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsSelProperty);
		}

	#endregion

	#region is_sel - foreground (any)

		public static readonly DependencyProperty
			FgIsSelProperty = DependencyProperty.RegisterAttached(
				"FgIsSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsSelProperty, value);
		}

		public static SolidColorBrush GetFgIsSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsSelProperty);
		}

	#endregion
		
	#region is_sel - borderbrush

		public static readonly DependencyProperty
			BdrIsSelProperty = DependencyProperty.RegisterAttached(
				"BdrIsSel", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsSel(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsSelProperty, value);
		}

		public static SolidColorBrush GetBdrIsSel(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsSelProperty);
		}

	#endregion

	#region is_sel - icon geometry

		public static readonly DependencyProperty
			IconGeometryIsSelProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsSel", typeof(Geometry), typeof(VisualStates),
				new FrameworkPropertyMetadata(new PathGeometry(),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryIsSel(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryIsSelProperty, value);
		}

		public static Geometry GetIconGeometryIsSel(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryIsSelProperty);
		}

	#endregion


	#region is_sel - mouse over background

		public static readonly DependencyProperty
			BgIsSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"BgIsSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetBgIsSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsSelMouseOverProperty);
		}

	#endregion

	#region is_sel - mouse over foreground

		public static readonly DependencyProperty
			FgIsSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"FgIsSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetFgIsSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsSelMouseOverProperty);
		}

	#endregion

	#region is_sel - mouse over borderbrush

		public static readonly DependencyProperty
			BdrIsSelMouseOverProperty = DependencyProperty.RegisterAttached(
				"BdrIsSelMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsSelMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsSelMouseOverProperty, value);
		}

		public static SolidColorBrush GetBdrIsSelMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsSelMouseOverProperty);
		}

	#endregion


		// enabled & selected & focused

	#region is_editing - background (any)

		public static readonly DependencyProperty
			BgIsEditingProperty = DependencyProperty.RegisterAttached(
				"BgIsEditing", typeof(SolidColorBrush), typeof(VisualStates),
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

	#region is_editing - foreground (any)

		public static readonly DependencyProperty
			FgIsEditingProperty = DependencyProperty.RegisterAttached(
				"FgIsEditing", typeof(SolidColorBrush), typeof(VisualStates),
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

	#region is_editing - borderbrush

		public static readonly DependencyProperty
			BdrIsEditingProperty = DependencyProperty.RegisterAttached(
				"BdrIsEditing", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsEditing(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsEditingProperty, value);
		}

		public static SolidColorBrush GetBdrIsEditing(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsEditingProperty);
		}

	#endregion

	#region is_editing - icon geometry

		public static readonly DependencyProperty
			IconGeometryIsEditingProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsEditing", typeof(Geometry), typeof(VisualStates),
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


	#region is_editing - mouse over background

		public static readonly DependencyProperty
			BgIsEditingMouseOverProperty = DependencyProperty.RegisterAttached(
				"BgIsEditingMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsEditingMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsEditingMouseOverProperty, value);
		}

		public static SolidColorBrush GetBgIsEditingMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsEditingMouseOverProperty);
		}

	#endregion

	#region is_editing - mouse over foreground

		public static readonly DependencyProperty
			FgIsEditingMouseOverProperty = DependencyProperty.RegisterAttached(
				"FgIsEditingMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsEditingMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsEditingMouseOverProperty, value);
		}

		public static SolidColorBrush GetFgIsEditingMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsEditingMouseOverProperty);
		}

	#endregion

	#region is_editing - mouse over borderbrush

		public static readonly DependencyProperty
			BdrIsEditingMouseOverProperty = DependencyProperty.RegisterAttached(
				"BdrIsEditingMouseOver", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsEditingMouseOver(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsEditingMouseOverProperty, value);
		}

		public static SolidColorBrush GetBdrIsEditingMouseOver(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsEditingMouseOverProperty);
		}

	#endregion

		// is read only

	#region is_readonly - background (any)

		public static readonly DependencyProperty
			BgIsReadOnlyProperty = DependencyProperty.RegisterAttached(
				"BgIsReadOnly", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgIsReadOnly(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgIsReadOnlyProperty, value);
		}

		public static SolidColorBrush GetBgIsReadOnly(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgIsReadOnlyProperty);
		}

	#endregion

	#region is_readonly - foreground (any)

		public static readonly DependencyProperty
			FgIsReadOnlyProperty = DependencyProperty.RegisterAttached(
				"FgIsReadOnly", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgIsReadOnly(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgIsReadOnlyProperty, value);
		}

		public static SolidColorBrush GetFgIsReadOnly(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgIsReadOnlyProperty);
		}

	#endregion
		
	#region is_readonly - borderbrush

		public static readonly DependencyProperty
			BdrIsReadOnlyProperty = DependencyProperty.RegisterAttached(
				"BdrIsReadOnly", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsReadOnly(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsReadOnlyProperty, value);
		}

		public static SolidColorBrush GetBdrIsReadOnly(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsReadOnlyProperty);
		}

	#endregion

	#region is_readonly - icon geometry

		public static readonly DependencyProperty
			IconGeometryIsReadOnlyProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsReadOnly", typeof(Geometry), typeof(VisualStates),
				new FrameworkPropertyMetadata(new PathGeometry(),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetIconGeometryIsReadOnly(UIElement e, Geometry value)
		{
			e.SetValue(IconGeometryIsReadOnlyProperty, value);
		}

		public static Geometry GetIconGeometryIsReadOnly(UIElement e)
		{
			return (Geometry) e.GetValue(IconGeometryIsReadOnlyProperty);
		}

	#endregion

		// is locked

	#region is_locked - background (any)

		public static readonly DependencyProperty
			BgIsLockedProperty = DependencyProperty.RegisterAttached(
				"BgIsLocked", typeof(SolidColorBrush), typeof(VisualStates),
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
				"FgIsLocked", typeof(SolidColorBrush), typeof(VisualStates),
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

	#region is_locked - borderbrush

		public static readonly DependencyProperty
			BdrIsLockedProperty = DependencyProperty.RegisterAttached(
				"BdrIsLocked", typeof(SolidColorBrush), typeof(VisualStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrIsLocked(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrIsLockedProperty, value);
		}

		public static SolidColorBrush GetBdrIsLocked(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrIsLockedProperty);
		}

	#endregion

	#region is_locked - icon geometry

		public static readonly DependencyProperty
			IconGeometryIsLockedProperty = DependencyProperty.RegisterAttached(
				"IconGeometryIsLocked", typeof(Geometry), typeof(VisualStates),
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