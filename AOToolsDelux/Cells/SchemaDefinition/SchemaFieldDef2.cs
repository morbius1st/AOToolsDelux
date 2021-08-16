#region + Using Directives

using System;
using AOTools.Cells.SchemaDefinition;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

namespace AOTools.Cells.SchemaDefinition2
{

	public abstract class ISchemaFieldDef2<TE> where TE : Enum 
	{
		public TE Key { get; set; }
		public int Sequence { get; set; }
		public string Name { get; set; }
		public string Desc { get; set; }
		public RevitUnitType UnitType { get; set; }
		public string Guid { get; set; }

		public Type Type { get; set; }

		public string AsString() => As<string>();
		public bool AsBool() => As<bool>();
		public double AsDouble() => As<double>();
		public int AsInt() => As<int>();

		public abstract ISchemaFieldDef2<TE> Clone();

		public TD As<TD> ()
		{
			return ((SchemaFieldDef2<TE, TD>) this).Value;
		}

		public void Set<TD>(TD val)
		{
			((SchemaFieldDef2<TE, TD>) this).Value = val;
		}
	}
	
	public class SchemaFieldDef2<TE, TD> : ISchemaFieldDef2<TE>  where TE: Enum 
	{
		// public TE Key { get; private set; }
		// [DataMember(Order = 1)]
		// public int Sequence { get; set; }

		// [DataMember(Order = 2)]
		// public string Name { get; set; }

		// [DataMember(Order = 3)]
		// public string Desc { get; set; }

		// [DataMember(Order = 4)]
		// public RevitUnitType UnitType { get; set; }

		// [DataMember(Order = 5)]
		// public string Guid { get; set; }

		// public Type Type { get; set; }

		public TD Value { get; set; }

		// // [DataMember(Name = "RevitFieldValue", Order = 6)]
		// public dynamic Value { get; set; }

		public SchemaFieldDef2()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			Value = default;
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldDef2(TE sequence, string name, string desc, TD val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Key = sequence;
			Sequence = (int)(object) sequence;
			Name = name;
			Desc = desc;
			Value = val;
			Type = val.GetType();
			UnitType = unitType;
			Guid = guid;
		}

		public override ISchemaFieldDef2<TE> Clone()
		{
			SchemaFieldDef2<TE, TD> copy = new SchemaFieldDef2<TE, TD>();

			copy.Key        = Key;
			copy.Sequence	= Sequence;
			copy.Name		= Name;
			copy.Desc		= Desc;
			copy.Type		= Type;
			copy.UnitType	= UnitType;
			copy.Guid		= Guid;
			copy.Value		= Value;

			return copy;
		}

		public override string ToString()
		{
			return $"(field def) name| {Name}  type| {Type.Name}  value| {Value}";
		}
	}
}
