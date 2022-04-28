#region + Using Directives

using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

#endregion

// user name: jeffs
// created:   12/30/2020 11:11:08 PM

namespace CSToolsStudies.Windows.Support
{
	public class CsScrollViewerAp : DependencyObject
	{



	#region scroll viewer border thickness

		public static readonly DependencyProperty ScrollViewerBorderThicknessProperty = DependencyProperty.RegisterAttached(
			"ScrollViewerBorderThickness", typeof(Thickness), typeof(CsScrollViewerAp), new PropertyMetadata(new Thickness(0)));

		public static void SetScrollViewerBorderThickness(UIElement e, Thickness value)
		{
			e.SetValue(ScrollViewerBorderThicknessProperty, value);
		}

		public static Thickness GetScrollViewerBorderThickness(UIElement e)
		{
			return (Thickness) e.GetValue(ScrollViewerBorderThicknessProperty);
		}

	#endregion

	#region scroll viewer border color

		public static readonly DependencyProperty ScrollViewerBorderColorProperty = DependencyProperty.RegisterAttached(
			"ScrollViewerBorderColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetScrollViewerBorderColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(ScrollViewerBorderColorProperty, value);
		}

		public static SolidColorBrush GetScrollViewerBorderColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(ScrollViewerBorderColorProperty);
		}

	#endregion

	#region scroll viewer corner radius
	
		public static readonly DependencyProperty ScrollViewerCornerRadiusProperty = DependencyProperty.RegisterAttached(
			"ScrollViewerCornerRadius", typeof(CornerRadius), typeof(CsScrollViewerAp), new PropertyMetadata(new CornerRadius(0)));
	
		public static void SetScrollViewerCornerRadius(UIElement element, CornerRadius value)
		{
			element.SetValue(ScrollViewerCornerRadiusProperty, value);
		}
	
		public static CornerRadius GetScrollViewerCornerRadius(UIElement element)
		{
			return (CornerRadius) element.GetValue(ScrollViewerCornerRadiusProperty);
		}
	
	#endregion


	#region corner rectangle color

		public static readonly DependencyProperty CornerRectColorProperty = DependencyProperty.RegisterAttached(
			"CornerRectColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetCornerRectColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(CornerRectColorProperty, value);
		}

		public static SolidColorBrush GetCornerRectColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(CornerRectColorProperty);
		}

	#endregion

	#region corner rectangle left border color

		public static readonly DependencyProperty CornerRectLeftBdrColorProperty = DependencyProperty.RegisterAttached(
			"CornerRectLeftBdrColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetCornerRectLeftBdrColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(CornerRectLeftBdrColorProperty, value);
		}

		public static SolidColorBrush GetCornerRectLeftBdrColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(CornerRectLeftBdrColorProperty);
		}

	#endregion

	#region corner rectangle top border color

		public static readonly DependencyProperty CornerRectTopBdrColorProperty = DependencyProperty.RegisterAttached(
			"CornerRectTopBdrColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetCornerRectTopBdrColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(CornerRectTopBdrColorProperty, value);
		}

		public static SolidColorBrush GetCornerRectTopBdrColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(CornerRectTopBdrColorProperty);
		}

	#endregion
		
	#region corner rectangle right border color

		public static readonly DependencyProperty CornerRectRightBdrColorProperty = DependencyProperty.RegisterAttached(
			"CornerRectRightBdrColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetCornerRectRightBdrColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(CornerRectRightBdrColorProperty, value);
		}

		public static SolidColorBrush GetCornerRectRightBdrColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(CornerRectRightBdrColorProperty);
		}

	#endregion
				
	#region corner rectangle bottom border color

		public static readonly DependencyProperty CornerRectBottBdrColorProperty = DependencyProperty.RegisterAttached(
			"CornerRectBottBdrColor", typeof(SolidColorBrush), typeof(CsScrollViewerAp), 
			new PropertyMetadata(Brushes.Black));

		public static void SetCornerRectBottBdrColor(UIElement e, SolidColorBrush value)
		{
			e.SetValue(CornerRectBottBdrColorProperty, value);
		}

		public static SolidColorBrush GetCornerRectBottBdrColor(UIElement e)
		{
			return (SolidColorBrush) e.GetValue(CornerRectBottBdrColorProperty);
		}

	#endregion

	#region corner rectangle left border thickness

		public static readonly DependencyProperty CornerRectLeftBdrThicknessProperty = DependencyProperty.RegisterAttached(
			"CornerRectLeftBdrThickness", typeof(double), typeof(CsScrollViewerAp), new PropertyMetadata(0.0));

		public static void SetCornerRectLeftBdrThickness(UIElement e, double value)
		{
			e.SetValue(CornerRectLeftBdrThicknessProperty, value);
		}

		public static double GetCornerRectLeftBdrThickness(UIElement e)
		{
			return (double) e.GetValue(CornerRectLeftBdrThicknessProperty);
		}

	#endregion

	#region corner rectangle top border height

		public static readonly DependencyProperty CornerRectTopBdrThicknessProperty = DependencyProperty.RegisterAttached(
			"CornerRectTopBdrThickness", typeof(double), typeof(CsScrollViewerAp), new PropertyMetadata(0.0));

		public static void SetCornerRectTopBdrThickness(UIElement e, double value)
		{
			e.SetValue(CornerRectTopBdrThicknessProperty, value);
		}

		public static double GetCornerRectTopBdrThickness(UIElement e)
		{
			return (double) e.GetValue(CornerRectTopBdrThicknessProperty);
		}

	#endregion
		
	#region corner rectangle right border height

		public static readonly DependencyProperty CornerRectRightBdrThicknessProperty = DependencyProperty.RegisterAttached(
			"CornerRectRightBdrThickness", typeof(double), typeof(CsScrollViewerAp), 
			new PropertyMetadata(0.0));

		public static void SetCornerRectRightBdrThickness(UIElement e, double value)
		{
			e.SetValue(CornerRectRightBdrThicknessProperty, value);
		}

		public static double GetCornerRectRightBdrThickness(UIElement e)
		{
			return (double) e.GetValue(CornerRectRightBdrThicknessProperty);
		}

	#endregion

	#region corner rectangle bottom border height

		public static readonly DependencyProperty CornerRectBottBdrThicknessProperty = DependencyProperty.RegisterAttached(
			"CornerRectBottBdrThickness", typeof(double), typeof(CsScrollViewerAp), 
			new PropertyMetadata(0.0));

		public static void SetCornerRectBottBdrThickness(UIElement e, double value)
		{
			e.SetValue(CornerRectBottBdrThicknessProperty, value);
		}

		public static double GetCornerRectBottBdrThickness(UIElement e)
		{
			return (double) e.GetValue(CornerRectBottBdrThicknessProperty);
		}

	#endregion



	}
}