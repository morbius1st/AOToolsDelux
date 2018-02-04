using System.Runtime.Serialization;
using AOTools.AppSettings.RevitSettings;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace AOTools.AppSettings.SchemaSettings
{
	[DataContract]
	public class SchemaFieldUnit
	{
		[DataMember(Order = 1)]
		public int Sequence { get; set; }
		[DataMember(Order = 2)]
		public string Name { get; set; }
		[DataMember(Order = 3)]
		public string Desc { get; set; }
		[DataMember(Order = 4)]
		public RevitUnitType UnitType { get; set; }
		[DataMember(Order = 5)]
		public string Guid { get; set; }
		[DataMember(Name = "RevitFieldValue", Order = 6)]
		public dynamic Value { get; set; }

		public SchemaFieldUnit()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			Value = null;
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldUnit(SchemaUsrKey sequence, string name, string desc, dynamic val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Sequence = (int) sequence;
			Name = name;
			Desc = desc;
			Value = val;
			UnitType = unitType;
			Guid = guid;
		}

		public SchemaFieldUnit(SchemaAppKey sequence, string name, string desc, dynamic val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Sequence = (int) sequence;
			Name = name;
			Desc = desc;
			Value = val;
			UnitType = unitType;
			Guid = guid;
		}

		public SchemaFieldUnit(SchemaFieldUnit fi)
		{
			Sequence = fi.Sequence;
			Name = fi.Name;
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
	}



	[DataContract]
	public enum SchemaBoolOpts
	{
		NO = 0,
		YES = 1,
		IGNORE = -1
	}

	public enum SchemaUsrKey
	{
		VERSION_UNIT = 0,
		STYLE_NAME = 1, 
		STYLE_DESC = 2, 
		CAN_BE_ERASED = 3, 
		UNIT_SYSTEM = 4, 
		UNIT_TYPE = 5, 
		ACCURACY = 6, 
		DUT = 7, 
		UST = 8, 
		SUP_SPACE = 9, 
		SUP_LEAD_ZERO = 10, 
		SUP_TRAIL_ZERO = 11, 
		USE_DIG_GRP = 12, 
		USE_PLUS_PREFIX = 13
	}

	public enum SchemaAppKey
	{
		UNDEFINED = -1, 
		VERSION_BASIC = 0,
		USE_OFFICE = 1, 
		AUTO_RESTORE = 2, 
//		COUNT = 3, 
		CURRENT = 4
	}
}