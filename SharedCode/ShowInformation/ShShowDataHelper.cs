#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;

#endregion

// username: jeffs
// created:  2/9/2022 10:41:59 PM

namespace SharedCode.ShowInformation
{
	public class ShShowDataHelper
	{
	#region private fields

	#endregion

	#region ctor

		public ShShowDataHelper() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static Tuple<List<TSk>,
			Dictionary<TSk, ColData>,
			Dictionary<TSk, string>,
			List<List<Dictionary<TSk, string>>>
			>  DataColHdr<TSk>(ADataTempBase<TSk> data) where TSk : Enum, new()
		{
			List<TSk> colOrder = new List<TSk>();
			Dictionary<TSk, ColData> colHdr = new Dictionary<TSk, ColData>();
			Dictionary<TSk, string> colInfo = new Dictionary<TSk, string>();
			List<List<Dictionary<TSk, string>>> colData = new List<List<Dictionary<TSk, string>>>();


			foreach (KeyValuePair<TSk, AFieldsMembers<TSk>> kvp in data.Fields)
			{
				AFieldsMembers<TSk> a = kvp.Value;

				colOrder.Add(kvp.Key);

				colHdr.Add(kvp.Key, new ColData(a.ColDisplayData.ColWidth, a.ColDisplayData.TitleWidth,
					a.ColDisplayData.Just[0], a.ColDisplayData.Just[1]));

				colInfo.Add(kvp.Key, a.Desc);
			}

			for (int i = 0; i < data.DataIndexMaxAllowed; i++)
			{
				Dictionary<TSk, string> cData = new Dictionary<TSk, string>();
				List<Dictionary<TSk, string>> cDataList = new List<Dictionary<TSk, string>>();

				data.DataIndex = i;

				foreach (KeyValuePair<TSk, AFieldsMembers<TSk>> kvp in data.Fields)
				{
					cData.Add(kvp.Key, data[kvp.Key].ValueString);
				}

				cDataList.Add(cData);
				colData.Add(cDataList);
			}

			return new Tuple<List<TSk>,
				Dictionary<TSk, ColData>,
				Dictionary<TSk, string>,
				List<List<Dictionary<TSk, string>>>>(colOrder, colHdr, colInfo, colData);
		}

	#endregion

	#region private methods

	#endregion
	}
}