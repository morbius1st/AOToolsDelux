#region using

using System.Diagnostics;
using System.Dynamic;
using System.Windows;
using CSToolsStudies.Windows;
using CSToolsStudies.Windows.ResourceFiles.XamlResources;

#endregion

// projname: CSToolsStudies
// itemname: App
// username: jeffs
// created:  8/29/2021 2:19:26 PM

namespace CSToolsStudies
{
	public enum Skin {Jeff, Fluent}

	public struct WinLocation
	{
		public double Top { get; set; }
		public double left { get; set; }

		public WinLocation(double top, double left)
		{
			Top = top;
			this.left = left;
		}
	}


	public partial class AppRibbon : Application
	{

		public static Skin Skin { get; set; } = Skin.Fluent;

		public AppRibbon()
		{
			InitializeComponent();
		}

		public void ChangeSkin(Skin newskin)
		{
			Skin = newskin;

			foreach (ResourceDictionary dict in Resources.MergedDictionaries)
			{
				if (dict is SkinResourceDictionary skinDict)
				{
					skinDict.UpdateSource();
				}
				else
				{
					dict.Source = dict.Source;
				}
			}
		}


		private void appStart(object sender, StartupEventArgs e)
		{
			WinLocation location = new WinLocation(-1, -1);
			Test1 t1= new Test1(location);
			bool repeat;

			do
			{
				repeat = false;
				t1.ShowDialog();

				if (t1.DialogResult == false &&  t1.Tag != null)
				{
					location = (WinLocation) t1.Tag;
					t1= new Test1(location);
					repeat = true;
					
				}
				
			}
			while (repeat);

			this.Shutdown();
			
		}
	}
}
