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
	public class CsCheckBoxAp : DependencyObject
	{

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


	}
}