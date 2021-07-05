#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

#endregion


// projname: AOToolsParameterVue.ParameterInfo
// itemname: ParameterInfo
// username: jeffs
// created:  8/16/2019 6:30:46 AM


namespace AOToolsParameterVue.ParameterInfo
{
	// data display method

	// columns
	// style | param   | param   | param   | ...
	//  name | name    | name    | name    | ...
	// rows
	// style | value 1 | value 2 | value 3 | ...


	public class ParameterInfo
	{
		public ObservableCollection<ParameterData> Header { get; set; }
		public ObservableCollection<ParameterValues> Styles { get; set; }

		public ParameterInfo()
		{
			Header = new ObservableCollection<ParameterData>();
			Styles = new ObservableCollection<ParameterValues>();

			TempData();
		}

		private void TempData()
		{
			Header.Add(new ParameterData() { Name = "Style Name"});
			Header.Add(new ParameterData() { Name = "Param 1"});
			Header.Add(new ParameterData() { Name = "Param 2"});
			Header.Add(new ParameterData() { Name = "Param 3"});

			string[] Values = new string[4];

			Styles = new ObservableCollection<ParameterValues>();

//			Styles.Add(new ParameterValues(){Values=new string[] {"Name 1", "value 1.1", "value 1.2", "value 1.3"}});
//			Styles.Add(new ParameterValues(){Values=new string[] {"Name 2", "value 2.1", "value 2.2", "value 2.3"}});
//			Styles.Add(new ParameterValues(){Values=new string[] {"Name 3", "value 3.1", "value 3.2", "value 3.3"}});

			Styles.Add(new ParameterValues(){Values="p1"});
			Styles.Add(new ParameterValues(){Values="p2"});
			Styles.Add(new ParameterValues(){Values="p3"});

		}
	}

	public class ParameterData
	{
		public string Name { get; set; }
		public StorageType StoregeType { get; set; }
		public ParameterType ParameterType { get; set; }
		public UnitType UnitType { get; set; }
		public DisplayUnitType DUT { get; set; }
	}

	public class ParameterValues
	{
		public string Values;

	}


}
