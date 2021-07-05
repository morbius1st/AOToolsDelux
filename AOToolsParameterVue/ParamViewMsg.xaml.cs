using System.Windows;

namespace AOToolsParameterVue
{
	/// <summary>
	/// Interaction logic for View.xaml
	/// </summary>
	public partial class ParamViewMsg : Window
	{
		public ParameterInfo.ParameterInfo ParamInfo { get; set; } = new ParameterInfo.ParameterInfo();

		public ParamViewMsg()
		{
			InitializeComponent();

			

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
