#region + Using Directives

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

namespace AOTools.Cells.SchemaDefinition
{

	public interface ISchemaFieldDef
	{
		dynamic Value { get; set; }
	}
	
	public class SchemaFieldDef : ISchemaFieldDef  /*where T: Enum */
	{
		// // [DataMember(Order = 1)]
		// public int Sequence { get; set; }

		// [DataMember(Order = 2)]
		public string FieldName { get; set; }

		// [DataMember(Order = 3)]
		public string Desc { get; set; }

		// [DataMember(Order = 4)]
		public RevitUnitType UnitType { get; set; }

		// [DataMember(Order = 5)]
		public string Guid { get; set; }

		// [DataMember(Name = "RevitFieldValue", Order = 6)]
		public dynamic Value { get; set; }

		public SchemaFieldDef()
		{
			// Sequence = -1;
			FieldName = null;
			Desc = null;
			Value = null;
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		// public SchemaFieldDef(SchemaAppKey sequence, string name, string desc, dynamic val,
		// 	RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		// {
		// 	Sequence = (int) sequence;
		// 	Name = name;
		// 	Desc = desc;
		// 	Value = val;
		// 	UnitType = unitType;
		// 	Guid = guid;
		// }
		//
		// public SchemaFieldDef(SchemaCellKey sequence, string name, string desc, dynamic val,
		// 	RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		// {
		// 	Sequence = (int) sequence;
		// 	Name = name;
		// 	Desc = desc;
		// 	Value = val;
		// 	UnitType = unitType;
		// 	Guid = guid;
		// }

		
		public SchemaFieldDef( /*T sequence,*/ string fieldName, string desc, 
			dynamic val, RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, 
			string guid = "")
		{
			// Sequence = (int)(object) sequence;
			FieldName = fieldName;
			Desc = desc;
			Value = val;
			UnitType = unitType;
			Guid = guid;
		}


		public SchemaFieldDef(SchemaFieldDef fi)
		{
			// Sequence = fi.Sequence;
			FieldName = fi.FieldName;
			Desc = fi.Desc;
			Value = fi.Value;
			UnitType = fi.UnitType;
			Guid = fi.Guid;
		}

		// master switch routine
		public dynamic ExtractValue(Entity e, Field f)
		{
			return ExtractValue(Value, e, f);
		}

		// sub-routine
		private Entity ExtractValue(Entity key, Entity e, Field f)
		{
			return e.Get<Entity>(f);
		}

		private string ExtractValue(string key, Entity e, Field f)
		{
			return e.Get<string>(f);
		}

		private int ExtractValue(int key, Entity e, Field f)
		{
			return e.Get<int>(f);
		}

		private bool ExtractValue(bool key, Entity e, Field f)
		{
			return e.Get<bool>(f);
		}

		private double ExtractValue(double key, Entity e, Field f)
		{
			return e.Get<double>(f, DisplayUnitType.DUT_GENERAL);
		}

		public SchemaFieldDef Clone()
		{
			// SchemaFieldDef copy = new SchemaFieldDef(Sequence, Name, Desc, Value, UnitType, Guid);
			return new SchemaFieldDef(this);
		}
	}
}
