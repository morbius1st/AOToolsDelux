﻿#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;

#endregion

// username: jeffs
// created:  2/6/2022 7:04:42 PM

namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{
	public class FieldsTemp<TE, TD> : AFieldsMembers<TE> where TE : Enum
	{
		public override KeyValuePair<string, int> Type { get; }

		public override TE Key { get; protected set; }

		public override int Sequence { get; protected set; }

		public  override string Name { get; protected set; }

		public  override string Desc { get; protected set; }

		public  override FieldUnitType UnitType { get; protected set; }

		public  override string Guid { get; protected set; }

		public  override System.Type ValueType { get; protected set; }

		public  override string ValueString => this.Value.ToString();

		public TD Value { get; set; }

		public  override SchemaFieldDisplayLevel DisplayLevel { get; protected set; }

		public  override string DisplayOrder { get; protected set; }

		public override ColData ColDisplayData { get; protected set; }

		public FieldsTemp()
		{
			this.Sequence = -1;
			this.Name = (string) null;
			this.Desc = (string) null;
			this.Value = default (TD);
			this.UnitType = FieldUnitType.UT_UNDEFINED;
			this.Guid = (string) null;
			this.DisplayLevel = SchemaFieldDisplayLevel.DL_DEBUG;
			this.DisplayOrder = (string) null;
			this.ColDisplayData = null;
		}

		public FieldsTemp(
			TE sequence,
			string name,
			string desc,
			TD val,
			SchemaFieldDisplayLevel displayLevel,
			string dispOrder,
			ColData colDisplayData,
			FieldUnitType unitType = FieldUnitType.UT_UNDEFINED,
			string guid = "")
		{
			this.Key = sequence;
			this.Sequence = (int) (object) sequence;
			this.Name = name;
			this.Desc = desc;
			this.DisplayLevel = displayLevel;
			this.DisplayOrder = dispOrder;
			this.ColDisplayData = colDisplayData;

			this.Value = val;
			this.ValueType = val.GetType();
			this.UnitType = unitType;
			this.Guid = guid;
		}

		public override AFieldsMembers<TE> Clone() => (AFieldsMembers<TE>) new FieldsTemp<TE, TD>()
		{
			Key = this.Key,
			Sequence = this.Sequence,
			Name = this.Name,
			Desc = this.Desc,
			DisplayLevel = this.DisplayLevel,
			DisplayOrder = this.DisplayOrder,
			ColDisplayData = new ColData(this.ColDisplayData.ColWidth, this.ColDisplayData.TitleWidth, this.ColDisplayData.Just[0], this.ColDisplayData.Just[1]),
			ValueType = this.ValueType,
			UnitType = this.UnitType,
			Guid = this.Guid,
			Value = this.Value
		};

		public override string ToString() => string.Format("(field def) name| {0}  type| {1}  value| {2}", (object) this.Name, (object) this.ValueType, (object) this.Value);
	}
}


/*


    public string[] showFieldsData(Dictionary<FieldColumns, ColData> Header)
    {
      string[] strArray1 = new string[Header.Count];
      int num = 0;
      foreach (KeyValuePair<FieldColumns, ColData> keyValuePair in Header)
      {
        string[] strArray2 = strArray1;
        int index = num++;
        // ISSUE: reference to a compiler-generated field
        if (FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (FieldsTemp<TE, TD>)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (FieldsTemp<TE, TD>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__0.Target((CallSite) FieldsTemp<TE, TD>.\u003C\u003Eo__49.\u003C\u003Ep__0, this[keyValuePair.Key]);
        string str = target((CallSite) p1, obj);
        strArray2[index] = str;
      }
      return strArray1;
    }


public class FieldsTemp<TE, TD> : AFieldsMembers<TE> where TE : Enum, new()
{
	#region private fields



	#endregion

	#region ctor

	public FieldsTemp() { }

	#endregion

	#region public properties



	#endregion

	#region private properties



	#endregion

	#region public methods



	#endregion

	#region private methods

		public KeyValuePair<string, int> Type { get; }
		public TE Key { get; }
		public int Sequence { get; }
		public string Name { get; }
		public string Desc { get; }
		public FieldUnitType UnitType { get; }
		public string Guid { get; }
		public Type ValueType { get; }
		public string ValueString { get; }
		public SchemaFieldDisplayLevel DisplayLevel { get; }
		public string DisplayOrder { get; }
		public int DisplayWidth { get; }

	#endregion

	#region event consuming



	#endregion

	#region event publishing



	#endregion

	#region system overrides

	public override string ToString()
	{
		return "this is FieldsTemp";
	}

	#endregion



		public AFieldsMembers<TE> Clone()
		{
			return null;
		}
}

*/