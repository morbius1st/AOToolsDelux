#region + Using Directives
using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

// defines a SINGLE field

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{

	// definition of a single schema field

	public class SchemaFieldDef<TE, TD> : ISchemaFieldDef<TE> where TE: Enum 
	{
		// defines the type
		public KeyValuePair<string, int> Type { get; }

		public TE Key { get; private set; }
		// [DataMember(Order = 1)]
		public int Sequence { get; set; }

		// [DataMember(Order = 2)]
		public string Name { get; set; }

		// [DataMember(Order = 3)]
		public string Desc { get; set; }

		// [DataMember(Order = 4)]
		public FieldUnitType UnitType { get; set; }

		// [DataMember(Order = 5)]
		public string Guid { get; set; }

		public Type ValueType { get; set; }

		public string ValueString => Value.ToString();

		// [DataMember(Name = "RevitFieldValue", Order = 6)]
		public TD Value { get; set; }

		public SchemaFieldDisplayLevel DisplayLevel { get; set; }

		// initial order to list in a chart
		public string DisplayOrder { get; set; }

		// initial display box width
		public int DisplayWidth { get; set; }

		public SchemaFieldDef()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			Value = default;
			UnitType = FieldUnitType.UT_UNDEFINED;
			Guid = null;
			DisplayLevel = SchemaFieldDisplayLevel.DL_DEBUG;
			DisplayOrder = null;
			DisplayWidth = -1;
		}

		public SchemaFieldDef(TE sequence,
			string name,
			string desc,
			TD val,
			SchemaFieldDisplayLevel displayLevel,
			string dispOrder,
			int dispWidth,
			FieldUnitType unitType = FieldUnitType.UT_UNDEFINED,
			string guid = "")
		{
			Key = sequence;
			Sequence = (int)(object) sequence;
			Name = name;
			Desc = desc;
			DisplayLevel = displayLevel;
			DisplayOrder = dispOrder;
			DisplayWidth = dispWidth;
			Value = val;
			ValueType = val.GetType();
			UnitType = unitType;
			Guid = guid;
		}

		public ISchemaFieldDef<TE> Clone()
		{
			SchemaFieldDef<TE, TD> copy = new SchemaFieldDef<TE, TD>();

			copy.Key          = Key;
			copy.Sequence	  = Sequence;
			copy.Name		  = Name;
			copy.Desc		  = Desc;
			copy.DisplayLevel = DisplayLevel;
			copy.DisplayOrder = DisplayOrder;
			copy.DisplayWidth = DisplayWidth;
			copy.ValueType	  = ValueType;
			copy.UnitType	  = UnitType;
			copy.Guid		  = Guid;
			copy.Value		  = Value;

			return copy;
		}

		public override string ToString()
		{
			return $"(field def) name| {Name}  type| {ValueType}  value| {Value}";
		}
	}

	/*
public class RootFields<TD> : SchemaFieldDef<SchemaRootKey, TD>
{
	public RootFields(SchemaRootKey sequence, string name, string desc, TD val,
		string dispOrder, int dispWidth,
		FieldUnitType unitType = FieldUnitType.UT_UNDEFINED, string guid = "") :
		base(sequence, name, desc, val, TODO, dispOrder, dispWidth, unitType, guid) {}
}


public class CellFields<TD> : SchemaFieldDef<SchemaCellKey, TD>
{
	public CellFields(SchemaCellKey sequence, string name, string desc, TD val,
		string dispOrder, int dispWidth,
		FieldUnitType unitType = FieldUnitType.UT_UNDEFINED, string guid = "") :
		base(sequence, name, desc, val, TODO, dispOrder, dispWidth, unitType, guid) {}
}

	
public class LockFields<TD> : SchemaFieldDef<SchemaLockKey, TD>
{
	public LockFields(SchemaLockKey sequence, string name, string desc, TD val,
		string dispOrder, int dispWidth,
		FieldUnitType unitType = FieldUnitType.UT_UNDEFINED, string guid = "") :
		base(sequence, name, desc, val, TODO, dispOrder, dispWidth, unitType, guid) {}
}

*/



}
