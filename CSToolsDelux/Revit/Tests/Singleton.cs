#region + Using Directives

using System.Collections.Generic;

#endregion

// user name: jeffs
// created:   9/6/2021 6:58:58 AM

namespace CSToolsDelux.Revit.Tests
{
	public class Singleton
	{
		private static Dictionary<string, Singleton> data;

		static Singleton()
		{
			data = new Dictionary<string, Singleton>();
		}

		private Singleton() { }

		public static Singleton Get(string docName)
		{
			if (!data.ContainsKey(docName))
			{
				Singleton s = new Singleton();
				s.docName = docName;
				data.Add(docName, s);
			}

			return data[docName];
		}

		private int i1 = 2;

		private string s1 = "singleton";

		private double d1 = 1.0;

		internal string docName;

		public string DocName
		{
			get => docName;
			private set { docName = value; }
		}

		public int I1
		{
			get => i1;
			set => i1 = value;
		}

		public string S1
		{
			get => s1;
			set => s1 = value;
		}

		public double D1
		{
			get => d1;
			set => d1 = value;
		}
	}
}