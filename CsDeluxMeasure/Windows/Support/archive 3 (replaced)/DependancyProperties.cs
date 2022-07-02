// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             DependancyProperties.cs
// Created:      2022-04-25 (8:38 PM)

using System.Windows;

namespace CsDeluxMeasure.Windows.Support
{
	public class DependancyProperties : DependencyObject
	{
	#region Generic Bool One

		public static readonly DependencyProperty GenericBoolOneProperty = DependencyProperty.RegisterAttached(
			"GenericBoolOne", typeof(bool), typeof(DependancyProperties),
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

	#region Generic Bool two

		public static readonly DependencyProperty GenericBoolTwoProperty = DependencyProperty.RegisterAttached(
			"GenericBoolTwo", typeof(bool), typeof(DependancyProperties),
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
	}
}