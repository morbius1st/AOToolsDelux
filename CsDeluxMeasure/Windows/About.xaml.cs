using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
// using SharedCode;
// using SharedCode.Resources;
using UtilityLibrary;


namespace CsDeluxMeasure.Windows

{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	///
	public partial class About : Window
	{
		private string version;
		private string year;
		private string programName;
		private string copyRight;

		public About(Window w)
		{
			version = CsUtilities.AssemblyVersion;
			year = GetAssemblyCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark ?? "none";
			programName = GetAssemblyCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "none";
			copyRight =GetAssemblyCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "none";

			InitializeComponent();

			this.Owner = w;
		}

	#region + properties

		public string Version => version;
		public string Year => year;
		public string CopyRight => copyRight;
		public string ProgramName => programName;


		public double ParentLeft
		{
			set
			{
				if (value >= 0 )
					this.Left = value + 50.0;
			}
		}

		public double ParentTop
		{
			set
			{
				if (value >= 0)
					this.Top = value + 50.0;
			}
		}

	#endregion

		internal static T GetAssemblyCustomAttribute<T>() where T : Attribute
		{
			object[] att = Assembly.GetExecutingAssembly()
			.GetCustomAttributes(typeof(T), false);
			if (att.Length > 0)
			{
				return ((T)att[0]);
			}

			return null;
		}


		private void btnPrivacy_Click(object sender, RoutedEventArgs e)
		{
			Privacy p = new Privacy(this);
			p.ShowDialog();
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		public Uri WebSite
		{
			get
			{
				// return new Uri(AppStrings.R_CyberStudioAddr);
				return new Uri("https://www.cyberstudioapps.com/");
			}
		}
	}
}