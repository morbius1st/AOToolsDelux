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
	public class CsComboxBoxAp : DependencyObject
	{

	#region DropDownWidth

		public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.RegisterAttached(
			"DropDownWidth", typeof(double), typeof(CsComboxBoxAp), new PropertyMetadata(100.0));

		public static void SetDropDownWidth(UIElement e, double value)
		{
			e.SetValue(DropDownWidthProperty, value);
		}

		public static double GetDropDownWidth(UIElement e)
		{
			return (double) e.GetValue(DropDownWidthProperty);
		}

	#endregion

	#region MouseOverBrush

		public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached(
			"MouseOverBrush", typeof(SolidColorBrush), typeof(CsComboxBoxAp), new PropertyMetadata(default(SolidColorBrush)));

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
			"DropDownBrush", typeof(SolidColorBrush), typeof(CsComboxBoxAp), new PropertyMetadata(default(SolidColorBrush)));

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
			"NotEnabledBrush", typeof(SolidColorBrush), typeof(CsComboxBoxAp), new PropertyMetadata(default(SolidColorBrush)));

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
