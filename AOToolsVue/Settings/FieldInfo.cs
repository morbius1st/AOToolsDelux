using System.Runtime.Serialization;

namespace AOToolsVue.Settings
{
	[DataContract]
	public class FieldInfo
	{
		[DataMember(Order = 1)]
		public int Sequence { get; set; }
		[DataMember(Order = 2)]
		public string Name { get; set; }
		[DataMember(Order = 3)]
		public string Desc { get; set; }
		[DataMember(Order = 4)]
		public int UnitType { get; set; }
		[DataMember(Order = 5)]
		public string Guid { get; set; }
		[DataMember(Name = "RevitFieldValue", Order = 6)]
		public dynamic Value { get; set; }


		public FieldInfo(SUKey sequence, string name, string desc, dynamic val, 
			int unitType = 1, string guid = "")
		{
			Sequence = (int) sequence;
			Name = name;
			Desc = desc;
			Value = val;
			UnitType = unitType;
			Guid = guid;
		}

		public FieldInfo(FieldInfo fi)
		{
			Sequence = fi.Sequence;
			Name = fi.Name;
			Desc = fi.Desc;
			Value = fi.Value;
			UnitType = fi.UnitType;
			Guid = fi.Guid;
		}
//
//		// master switch routine
//		public dynamic ExtractValue(Entity e, Field f)
//		{
//			return ExtractValue(Value, e, f);
//		}
//		// sub-routine
//		private Entity ExtractValue(Entity key, Entity e, Field f)
//		{
//			return e.Get<Entity>(f);
//		}
//
//		private string ExtractValue(string key, Entity e, Field f)
//		{
//			return e.Get<string>(f);
//		}
//			
//		private int ExtractValue(int key, Entity e, Field f)
//		{
//			return e.Get<int>(f);
//		}
//			
//		private bool ExtractValue(bool key, Entity e, Field f)
//		{
//			return e.Get<bool>(f);
//		}
//			
//		private double ExtractValue(double key, Entity e, Field f)
//		{
//			return e.Get<double>(f, DisplayUnitType.DUT_GENERAL);
//		}
	}

	[DataContract]
	public enum FmtOpt
	{
		NO = 0,
		YES = 1,
		IGNORE = -1
	}

	[DataContract]
	public abstract class SItemKey
	{
		[DataMember]
		public abstract int Key { get; set; }
	}

	[DataContract]
	public class SKey : SItemKey
	{
		[DataMember]
		public override int Key { get; set; }

		public SKey(int key)
		{
			Key = key;
		}

		public static SKey Key1 = new SKey(1);
		public static SKey Key2 = new SKey(2);
		public static SKey Key3 = new SKey(3);
		public static SKey Key4 = new SKey(4);
	}

	[DataContract]
	public abstract class SchemaKey
	{
		[DataMember]
		public abstract int Key { get; set; }

	}

	public enum SUKey
	{
		eVERSION_UNIT = 0,
		eSTYLE_NAME = 1,
		eSTYLE_DESC = 2,
		eCAN_BE_ERASED = 3
	}


	[DataContract]
	public class SUnitKey : SchemaKey
	{
		[DataMember]
		public override int Key { get; set; }

		public SUnitKey(int key)
		{
			this.Key = key;
		}

		public static SUnitKey VERSION_UNIT = new SUnitKey(0);
		public static SUnitKey STYLE_NAME = new SUnitKey(1);
		public static SUnitKey STYLE_DESC = new SUnitKey(2);
		public static SUnitKey CAN_BE_ERASED = new SUnitKey(3);
		public static SUnitKey UNIT_SYSTEM = new SUnitKey(4);
		public static SUnitKey UNIT_TYPE = new SUnitKey(5);
		public static SUnitKey ACCURACY = new SUnitKey(6);
		public static SUnitKey DUT = new SUnitKey(7);
		public static SUnitKey UST = new SUnitKey(8);
		public static SUnitKey SUP_SPACE = new SUnitKey(9);
		public static SUnitKey SUP_LEAD_ZERO = new SUnitKey(10);
		public static SUnitKey SUP_TRAIL_ZERO = new SUnitKey(11);
		public static SUnitKey USE_DIG_GRP = new SUnitKey(12);
		public static SUnitKey USE_PLUS_PREFIX = new SUnitKey(13);
	}



	//	[DataContract]
	//	public abstract class SchemaKey
	//	{
	//		[DataMember]
	//		public int Value { get; set; }
	//
	//	}
	//
	//	[DataContract]
	//	public class SBasicKey : SchemaKey
	//	{
	//		public SBasicKey(int value)
	//		{
	//			this.Value = value;
	//		}
	//
	////		[DataMember]
	////		public override int Value { get; set; }
	//
	//		public static readonly SBasicKey UNDEFINED = new SBasicKey(-1);
	//		public static readonly SBasicKey VERSION_BASIC = new SBasicKey(0);
	//		public static readonly SBasicKey USE_OFFICE = new SBasicKey(1);
	//		public static readonly SBasicKey AUTO_RESTORE = new SBasicKey(2);
	//		public static readonly SBasicKey COUNT = new SBasicKey(3);
	//		public static readonly SBasicKey CURRENT = new SBasicKey(4);
	//	}
	//
	//	[DataContract]
	//	public class SUnitKey : SchemaKey
	//	{
	//		public SUnitKey(int value)
	//		{
	//			this.Value = value;
	//		}

	//		[DataMember]
	//		public override int Value { get; set; }
	//	}
}