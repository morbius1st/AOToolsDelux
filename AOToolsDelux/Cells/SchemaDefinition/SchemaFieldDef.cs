#region + Using Directives

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

namespace AOTools.Cells.SchemaDefinition
{

	public interface ISchemaFieldDef<TE> where TE : Enum
	{
		TE Key { get; }
		Type ValueType { get; }
		dynamic Value { get; set; }

		ISchemaFieldDef<TE> Clone();
	}
	
	public class SchemaFieldDef<TE> : ISchemaFieldDef<TE>  where TE: Enum 
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

		// [DataMember(Name = "RevitFieldValue", Order = 6)]
		public dynamic Value { get; set; }

		public SchemaFieldDef()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			Value = null;
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldDef(TE sequence, string name, string desc, dynamic val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Key = sequence;
			Sequence = (int)(object) sequence;
			Name = name;
			Desc = desc;
			Value = val;
			ValueType = val.GetType();
			UnitType = unitType;
			Guid = guid;
		}

		public ISchemaFieldDef<TE> Clone()
		{
			SchemaFieldDef<TE> copy = new SchemaFieldDef<TE>();

			copy.Key        = Key;
			copy.Sequence	= Sequence;
			copy.Name		= Name;
			copy.Desc		= Desc;
			copy.ValueType	= ValueType;
			copy.UnitType	= UnitType;
			copy.Guid		= Guid;
			copy.Value		= Value;

			return copy;
		}

		public override string ToString()
		{
			return $"(field def) name| {Name}  type| {ValueType}  value| {Value}";
		}
	}
}
