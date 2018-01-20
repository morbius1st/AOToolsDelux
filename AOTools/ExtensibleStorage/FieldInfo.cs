using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace AOTools
{
	public class FieldInfo
	{
		public SchemaKey Key { get; }
		public string Name { get; set; }
		public string Desc { get; }
		public UnitType UnitType { get; }
		public string Guid { get; }
		public dynamic Value { get; set; }


		public FieldInfo(SchemaKey key, string name, string desc, dynamic val, 
			UnitType unitType = UnitType.UT_Undefined, string guid = "")
		{
			Key = key;
			Name = name;
			Desc = desc;
			Value = val;
			UnitType = unitType;
			Guid = guid;
		}

		public FieldInfo(FieldInfo fi)
		{
			Key = fi.Key;
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

	public enum FmtOpt
	{
		NO = 0,
		YES = 1,
		IGNORE = -1
	}

	public abstract class SchemaKey
	{
		public abstract int Value { get; }
	}

	public class SBasicKey : SchemaKey
	{
		public SBasicKey(int value)
		{
			this.Value = value;
		}

		public override int Value { get; }

		public static readonly SBasicKey UNDEFINED = new SBasicKey(-1);
		public static readonly SBasicKey VERSION_BASIC = new SBasicKey(0);
		public static readonly SBasicKey USE_OFFICE = new SBasicKey(1);
		public static readonly SBasicKey AUTO_RESTORE = new SBasicKey(2);
		public static readonly SBasicKey COUNT = new SBasicKey(3);
		public static readonly SBasicKey CURRENT = new SBasicKey(4);
	}

	public class SUnitKey : SchemaKey
	{
		public SUnitKey(int value)
		{
			this.Value = value;
		}

		public override int Value { get; }

		public static readonly SUnitKey VERSION_UNIT = new SUnitKey(0);
		public static readonly SUnitKey STYLE_NAME = new SUnitKey(1);
		public static readonly SUnitKey STYLE_DESC = new SUnitKey(2);
		public static readonly SUnitKey CAN_BE_ERASED = new SUnitKey(3);
		public static readonly SUnitKey UNIT_SYSTEM = new SUnitKey(4);
		public static readonly SUnitKey UNIT_TYPE = new SUnitKey(5);
		public static readonly SUnitKey ACCURACY = new SUnitKey(6);
		public static readonly SUnitKey DUT = new SUnitKey(7);
		public static readonly SUnitKey UST = new SUnitKey(8);
		public static readonly SUnitKey SUP_SPACE = new SUnitKey(9);
		public static readonly SUnitKey SUP_LEAD_ZERO = new SUnitKey(10);
		public static readonly SUnitKey SUP_TRAIL_ZERO = new SUnitKey(11);
		public static readonly SUnitKey USE_DIG_GRP = new SUnitKey(12);
		public static readonly SUnitKey USE_PLUS_PREFIX = new SUnitKey(13);
	}
}