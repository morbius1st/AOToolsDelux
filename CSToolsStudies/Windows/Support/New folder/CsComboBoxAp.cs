#region + Using Directives
using System; 
using System.Windows;
using System.Windows.Media;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;

#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace CSToolsStudies.Windows.Support
{
	public class CsComboBoxAp : DependencyObject
	{

	#region DropDownWidthAdjustment

		public static readonly DependencyProperty DropDownWidthAdjustmentProperty = DependencyProperty.RegisterAttached(
			"DropDownWidthAdjustment", typeof(double), typeof(CsComboBoxAp), new PropertyMetadata(0.0));

		public static void SetDropDownWidthAdjustment(UIElement e, double value)
		{
			e.SetValue(DropDownWidthAdjustmentProperty, value);
		}

		public static double GetDropDownWidthAdjustment(UIElement e)
		{
			return (double) e.GetValue(DropDownWidthAdjustmentProperty);
		}

	#endregion

	#region DropDownMaxWidth

		public static readonly DependencyProperty DropDownMaxWidthProperty = DependencyProperty.RegisterAttached(
			"DropDownMaxWidth", typeof(double), typeof(CsComboBoxAp), new PropertyMetadata(Double.PositiveInfinity));

		public static void SetDropDownMaxWidth(UIElement e, double value)
		{
			e.SetValue(DropDownMaxWidthProperty, value);
		}

		public static double GetDropDownMaxWidth(UIElement e)
		{
			return (double) e.GetValue(DropDownMaxWidthProperty);
		}

	#endregion

	#region MouseOverBrush

		public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached(
			"MouseOverBrush", typeof(SolidColorBrush), typeof(CsComboBoxAp), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetMouseOverBrush(UIElement e, SolidColorBrush value)
		{
			e.SetValue(MouseOverBrushProperty, value);
		}

		public static SolidColorBrush GetMouseOverBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(MouseOverBrushProperty);
		}

	#endregion
		
	#region DropDownBrush

		public static readonly DependencyProperty  DropDownBrushProperty = DependencyProperty.RegisterAttached(
			"DropDownBrush", typeof(SolidColorBrush), typeof(CsComboBoxAp), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetDropDownBrushh(UIElement e, SolidColorBrush value)
		{
			e.SetValue(DropDownBrushProperty, value);
		}

		public static SolidColorBrush GetDropDownBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(DropDownBrushProperty);
		}

	#endregion

	#region NotEnabledBrush

		public static readonly DependencyProperty NotEnabledBrushProperty = DependencyProperty.RegisterAttached(
			"NotEnabledBrush", typeof(SolidColorBrush), typeof(CsComboBoxAp), new PropertyMetadata(default(SolidColorBrush)));

		public static void SetNotEnabledBrush(UIElement e, SolidColorBrush value)
		{
			e.SetValue(NotEnabledBrushProperty, value);
		}

		public static SolidColorBrush GetNotEnabledBrush(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(NotEnabledBrushProperty);
		}

	#endregion


	}
}
