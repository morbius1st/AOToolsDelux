﻿#region + Using Directives

using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

#endregion

// user name: jeffs
// created:   12/30/2020 11:11:08 PM

namespace CsDeluxMeasure.Windows.Support
{
	public class CsCheckBoxAp : DependencyObject
	{

		
	#region double - check box box size

		public static readonly DependencyProperty CheckBoxBoxSizeProperty = DependencyProperty.RegisterAttached(
			"CheckBoxBoxSize", typeof(double), typeof(CsCheckBoxAp), 
			new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCheckBoxBoxSize(UIElement e, double value)
		{
			e.SetValue(CheckBoxBoxSizeProperty, value);
		}

		public static double GetCheckBoxBoxSize(UIElement e)
		{
			return (double) e.GetValue(CheckBoxBoxSizeProperty);
		}

	#endregion


	#region checkbox box margin

		public static readonly DependencyProperty 
			CheckBoxBoxMarginProperty = DependencyProperty.RegisterAttached(
			"CheckBoxBoxMargin", typeof(Thickness), 
			typeof(CsCheckBoxAp), new FrameworkPropertyMetadata(new Thickness(0), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCheckBoxBoxMargin(UIElement e, Thickness value)
		{
			e.SetValue(CheckBoxBoxMarginProperty, value);
		}

		public static Thickness GetCheckBoxBoxMargin(UIElement e)
		{
			return (Thickness) e.GetValue(CheckBoxBoxMarginProperty);
		}

	#endregion

	#region checkbox check margin

		public static readonly DependencyProperty 
			CheckBoxCheckMarginProperty = DependencyProperty.RegisterAttached(
				"CheckBoxCheckMargin", typeof(Thickness), 
				typeof(CsCheckBoxAp), new FrameworkPropertyMetadata(new Thickness(0), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCheckBoxCheckMargin(UIElement e, Thickness value)
		{
			e.SetValue(CheckBoxCheckMarginProperty, value);
		}

		public static Thickness GetCheckBoxCheckMargin(UIElement e)
		{
			return (Thickness) e.GetValue(CheckBoxCheckMarginProperty);
		}

	#endregion

	#region content margin

		public static readonly DependencyProperty 
			CheckBoxContentMarginProperty = DependencyProperty.RegisterAttached(
				"CheckBoxContentMargin", typeof(Thickness), 
				typeof(CsCheckBoxAp), new FrameworkPropertyMetadata(new Thickness(0), 
					FrameworkPropertyMetadataOptions.Inherits));

		public static void SetCheckBoxContentMargin(UIElement e, Thickness value)
		{
			e.SetValue(CheckBoxContentMarginProperty, value);
		}

		public static Thickness GetCheckBoxContentMargin(UIElement e)
		{
			return (Thickness) e.GetValue(CheckBoxContentMarginProperty);
		}

	#endregion


	}
}