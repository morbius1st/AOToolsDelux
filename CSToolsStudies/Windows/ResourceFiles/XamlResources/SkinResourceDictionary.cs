#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#endregion

// user name: jeffs
// created:   5/28/2022 2:43:34 PM

namespace CSToolsStudies.Windows.ResourceFiles.XamlResources
{
	public class SkinResourceDictionary : ResourceDictionary
	{
		private Uri jeffSkinSource;
		private Uri fluentSkinSource;

		public Uri JeffSkinSource
		{
			get { return jeffSkinSource; }
			set
			{
				jeffSkinSource = value;
				UpdateSource();
			}
		}

		public Uri FluentSkinSource
		{
			get { return fluentSkinSource; }
			set
			{
				fluentSkinSource = value;
				UpdateSource();
			}
		}

		public void UpdateSource()
		{
			var val = AppRibbon.Skin == Skin.Jeff ? JeffSkinSource : FluentSkinSource;
			if (val != null && base.Source != val)
				base.Source = val;
		}
	}
}