#region + Using Directives

using System;
using AOTools.Cells.ExStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

namespace AOTools.Cells.SchemaDefinition
{

	// public interface ISchemaFieldDef
	// {
	// 	dynamic Value { get; set; }
	// }

	public interface  ISchemaFieldDef<TE> where TE : Enum
	{
		TE Key { get; }

		Type ValueType { get; }

		ISchemaFieldDef<TE> Clone();
	}
	
	public class SchemaFieldDef<TE, TD> : ISchemaFieldDef<TE> where TE: Enum 
	{
		public TE Key { get; private set; }
		// [DataMember(Order = 1)]
		public int Sequence { get; set; }

		// [DataMember(Order = 2)]
		public string Name { get; set; }

		// [DataMember(Order = 3)]
		public string Desc { get; set; }

		// [DataMember(Order = 4)]
		public RevitUnitType UnitType { get; set; }

		// [DataMember(Order = 5)]
		public string Guid { get; set; }

		public Type ValueType { get; set; }

		// public ExData<TD> DefaultValue { get; set; }

		// // [DataMember(Name = "RevitFieldValue", Order = 6)]
		// public dynamic Value { get; set; }

		public SchemaFieldDef()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			ValueType = typeof(object);
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldDef(TE sequence, string name, string desc, /*ExData<TD> value,*/
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Key = sequence;
			Sequence = (int)(object) sequence;
			Name = name;
			Desc = desc;
			ValueType = typeof(TD);
			UnitType = unitType;
			Guid = guid;
		}


		public SchemaFieldDef(SchemaFieldDef<TE, TD> fi)
		{
			Key = fi.Key;
			Sequence = fi.Sequence;
			Name = fi.Name;
			Desc = fi.Desc;
			ValueType = fi.ValueType;
			UnitType = fi.UnitType;
			Guid = fi.Guid;
		}

		public ISchemaFieldDef<TE> Clone()
		{
			return Copy();
		}

		public SchemaFieldDef<TE, TD> Copy()
		{
			SchemaFieldDef<TE, TD> copy = new SchemaFieldDef<TE, TD>();

			Key = copy.Key;
			Sequence = copy.Sequence;
			Name = copy.Name;
			Desc = copy.Desc;
			ValueType = copy.ValueType;
			UnitType = copy.UnitType;
			Guid = copy.Guid;

			return copy;
		}
	}
}
