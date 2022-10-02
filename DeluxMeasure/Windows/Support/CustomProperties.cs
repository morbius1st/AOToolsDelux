#region + Using Directives
using System; 
using System.Windows;
using System.Windows.Media;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace DeluxMeasure.Windows.Support
{
	public class CustomProperties : DependencyObject
	{
	#region GenericBoolOne

		public static readonly DependencyProperty GenericBoolOneProperty = DependencyProperty.RegisterAttached(
			"GenericBoolOne", typeof(bool), typeof(CustomProperties),
			new PropertyMetadata(false));

		public static void SetGenericBoolOne(UIElement e, bool value)
		{
			e.SetValue(GenericBoolOneProperty, value);
		}

		public static bool GetGenericBoolOne(UIElement e)
		{
			return (bool) e.GetValue(GenericBoolOneProperty);
		}

	#endregion

	#region GenericBoolOne

		public static readonly DependencyProperty GenericBoolTwoProperty = DependencyProperty.RegisterAttached(
			"GenericBoolTwo", typeof(bool), typeof(CustomProperties),
			new PropertyMetadata(false));

		public static void SetGenericBoolTwo(UIElement e, bool value)
		{
			e.SetValue(GenericBoolTwoProperty, value);
		}

		public static bool GetGenericBoolTwo(UIElement e)
		{
			return (bool) e.GetValue(GenericBoolTwoProperty);
		}

	#endregion

		
	#region GenericStringOne

		public static readonly DependencyProperty GenericStringOneProperty = DependencyProperty.RegisterAttached(
			"GenericStringOne", typeof(string), typeof(CustomProperties), new PropertyMetadata(""));

		public static void SetGenericStringOne(UIElement e, string value)
		{
			e.SetValue(GenericStringOneProperty, value);
		}

		public static string GetGenericStringOne(UIElement e)
		{
			return (string) e.GetValue(GenericStringOneProperty);
		}

	#endregion

	#region GenericDoubleOne

		public static readonly DependencyProperty GenericDoubleOneProperty = DependencyProperty.RegisterAttached(
			"GenericDoubleOne", typeof(double), typeof(CustomProperties),
			new FrameworkPropertyMetadata(-1.0, 
				FrameworkPropertyMetadataOptions.AffectsMeasure | 
				FrameworkPropertyMetadataOptions.Inherits |
				FrameworkPropertyMetadataOptions.AffectsParentMeasure |
				FrameworkPropertyMetadataOptions.AffectsRender));

		public static void SetGenericDoubleOne(UIElement e, double value)
		{
			e.SetValue(GenericDoubleOneProperty, value);
		}

		public static double GetGenericDoubleOne(UIElement e)
		{
			return (double) e.GetValue(GenericDoubleOneProperty);
		}

	#endregion


	// #region DropDownWidthAdjustment
	//
	// 	public static readonly DependencyProperty DropDownWidthAdjustmentProperty = DependencyProperty.RegisterAttached(
	// 		"DropDownWidthAdjustment", typeof(double), typeof(CustomProperties), new PropertyMetadata(0.0));
	//
	// 	public static void SetDropDownWidthAdjustment(UIElement e, double value)
	// 	{
	// 		e.SetValue(DropDownWidthAdjustmentProperty, value);
	// 	}
	//
	// 	public static double GetDropDownWidthAdjustment(UIElement e)
	// 	{
	// 		return (double) e.GetValue(DropDownWidthAdjustmentProperty);
	// 	}
	//
	// #endregion
	//
	// #region DropDownMinWidth
	//
	// 	public static readonly DependencyProperty DropDownMinWidthProperty = DependencyProperty.RegisterAttached(
	// 		"DropDownMinWidth", typeof(double), typeof(CustomProperties), new PropertyMetadata(100.0));
	//
	// 	public static void SetDropDownMinWidth(UIElement e, double value)
	// 	{
	// 		e.SetValue(DropDownMinWidthProperty, value);
	// 	}
	//
	// 	public static double GetDropDownMinWidth(UIElement e)
	// 	{
	// 		return (double) e.GetValue(DropDownMinWidthProperty);
	// 	}
	//
	// #endregion
	//
	// #region MouseOverBrush
	//
	// 	public static readonly DependencyProperty MouseOverBgBrushProperty = DependencyProperty.RegisterAttached(
	// 		"MouseOverBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));
	//
	// 	public static void SetMouseOverBgBrush(UIElement e, SolidColorBrush value)
	// 	{
	// 		e.SetValue(MouseOverBgBrushProperty, value);
	// 	}
	//
	// 	public static SolidColorBrush GetMouseOverBgBrush(UIElement e)
	// 	{
	// 		return (SolidColorBrush) e.GetValue(MouseOverBgBrushProperty);
	// 	}
	//
	// #endregion
	// 	
	// #region DropDownBrush
	//
	// 	public static readonly DependencyProperty  DropDownBrushProperty = DependencyProperty.RegisterAttached(
	// 		"DropDownBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));
	//
	// 	public static void SetDropDownBrushh(UIElement e, SolidColorBrush value)
	// 	{
	// 		e.SetValue(DropDownBrushProperty, value);
	// 	}
	//
	// 	public static SolidColorBrush GetDropDownBrush(UIElement e)
	// 	{
	// 		return (SolidColorBrush) e.GetValue(DropDownBrushProperty);
	// 	}
	//
	// #endregion
	//
	// #region NotEnabledBrush
	//
	// 	public static readonly DependencyProperty NotEnabledBrushProperty = DependencyProperty.RegisterAttached(
	// 		"NotEnabledBrush", typeof(SolidColorBrush), typeof(CustomProperties), new PropertyMetadata(default(SolidColorBrush)));
	//
	// 	public static void SetNotEnabledBrush(UIElement e, SolidColorBrush value)
	// 	{
	// 		e.SetValue(NotEnabledBrushProperty, value);
	// 	}
	//
	// 	public static SolidColorBrush GetNotEnabledBrush(UIElement e)
	// 	{
	// 		return (SolidColorBrush) e.GetValue(NotEnabledBrushProperty);
	// 	}
	//
	// #endregion
	//

	}
}
