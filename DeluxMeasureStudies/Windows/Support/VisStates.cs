#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DeluxMeasure.Windows.Support;

#endregion

// user name: jeffs
// created:   6/12/2022 4:41:14 PM

namespace DeluxMeasureStudies.Windows.Support
{
	public class VisStates
	{

	#region generic outer background

		public static readonly DependencyProperty
			BgOuterGenericProperty = DependencyProperty.RegisterAttached(
				"BgOuterGeneric", typeof(SolidColorBrush), typeof(VisStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgOuterGeneric(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgOuterGenericProperty, value);
		}

		public static SolidColorBrush GetBgOuterGeneric(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgOuterGenericProperty);
		}

	#endregion

	#region generic Inner background

		public static readonly DependencyProperty
			BgInnerGenericProperty = DependencyProperty.RegisterAttached(
				"BgInnerGeneric", typeof(SolidColorBrush), typeof(VisStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBgInnerGeneric(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BgInnerGenericProperty, value);
		}

		public static SolidColorBrush GetBgInnerGeneric(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BgInnerGenericProperty);
		}

	#endregion

		
	#region generic foreground

		public static readonly DependencyProperty
			FgGenericProperty = DependencyProperty.RegisterAttached(
				"FgGeneric", typeof(SolidColorBrush), typeof(VisStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetFgGeneric(UIElement e, SolidColorBrush value)
		{
			e.SetValue(FgGenericProperty, value);
		}

		public static SolidColorBrush GetFgGeneric(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(FgGenericProperty);
		}

	#endregion

		
	#region generic outer border

		public static readonly DependencyProperty
			BdrOuterGenericProperty = DependencyProperty.RegisterAttached(
				"BdrOuterGeneric", typeof(SolidColorBrush), typeof(VisStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrOuterGeneric(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrOuterGenericProperty, value);
		}

		public static SolidColorBrush GetBdrOuterGeneric(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrOuterGenericProperty);
		}

	#endregion
		
	#region generic inner border

		public static readonly DependencyProperty
			BdrInnerGenericProperty = DependencyProperty.RegisterAttached(
				"BdrInnerGeneric", typeof(SolidColorBrush), typeof(VisStates),
				new FrameworkPropertyMetadata(new SolidColorBrush(Colors.IndianRed),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetBdrInnerGeneric(UIElement e, SolidColorBrush value)
		{
			e.SetValue(BdrInnerGenericProperty, value);
		}

		public static SolidColorBrush GetBdrInnerGeneric(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(BdrInnerGenericProperty);
		}

	#endregion

				
	#region generic string

		public static readonly DependencyProperty
			StringGenericProperty = DependencyProperty.RegisterAttached(
				"StringGeneric", typeof(string), typeof(VisStates),
				new FrameworkPropertyMetadata("",
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetStringGeneric(UIElement e, string value)
		{
			e.SetValue(StringGenericProperty, value);
		}

		public static string GetStringGeneric(UIElement e)
		{
			return (string) e.GetValue(StringGenericProperty);
		}

	#endregion
		
	#region generic double

		public static readonly DependencyProperty
			DoubleGenericProperty = DependencyProperty.RegisterAttached(
				"DoubleGeneric", typeof(double), typeof(VisStates),
				new FrameworkPropertyMetadata(0.0,
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetDoubleGeneric(UIElement e, double value)
		{
			e.SetValue(DoubleGenericProperty, value);
		}

		public static double GetDoubleGeneric(UIElement e)
		{
			return (double) e.GetValue(DoubleGenericProperty);
		}

	#endregion
				
	#region generic thickness

		public static readonly DependencyProperty
			ThicknessOuterGenericProperty = DependencyProperty.RegisterAttached(
				"ThicknessOuterGeneric", typeof(Thickness), typeof(VisStates),
				new FrameworkPropertyMetadata(new Thickness(0),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetThicknessOuterGeneric(UIElement e, Thickness value)
		{
			e.SetValue(ThicknessOuterGenericProperty, value);
		}

		public static Thickness GetThicknessOuterGeneric(UIElement e)
		{
			return (Thickness) e.GetValue(ThicknessOuterGenericProperty);
		}

	#endregion
		
	#region generic thickness

		public static readonly DependencyProperty
			ThicknessInnerGenericProperty = DependencyProperty.RegisterAttached(
				"ThicknessInnerGeneric", typeof(Thickness), typeof(VisStates),
				new FrameworkPropertyMetadata(new Thickness(0),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetThicknessInnerGeneric(UIElement e, Thickness value)
		{
			e.SetValue(ThicknessInnerGenericProperty, value);
		}

		public static Thickness GetThicknessInnerGeneric(UIElement e)
		{
			return (Thickness) e.GetValue(ThicknessInnerGenericProperty);
		}

	#endregion

				
	#region generic cornerradius

		public static readonly DependencyProperty
			CornerRadiusOuterGenericProperty = DependencyProperty.RegisterAttached(
				"CornerRadiusOuterGeneric", typeof(CornerRadius), typeof(VisStates),
				new FrameworkPropertyMetadata(new CornerRadius(-1),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCornerRadiusOuterGeneric(UIElement e, CornerRadius value)
		{
			e.SetValue(CornerRadiusOuterGenericProperty, value);
		}

		public static CornerRadius GetCornerRadiusOuterGeneric(UIElement e)
		{
			return (CornerRadius) e.GetValue(CornerRadiusOuterGenericProperty);
		}

	#endregion
		
	#region generic cornerradius

		public static readonly DependencyProperty
			CornerRadiusInnerGenericProperty = DependencyProperty.RegisterAttached(
				"CornerRadiusInnerGeneric", typeof(CornerRadius), typeof(VisStates),
				new FrameworkPropertyMetadata(new CornerRadius(-1),
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCornerRadiusInnerGeneric(UIElement e, CornerRadius value)
		{
			e.SetValue(CornerRadiusInnerGenericProperty, value);
		}

		public static CornerRadius GetCornerRadiusInnerGeneric(UIElement e)
		{
			return (CornerRadius) e.GetValue(CornerRadiusInnerGenericProperty);
		}

	#endregion


	}
}
