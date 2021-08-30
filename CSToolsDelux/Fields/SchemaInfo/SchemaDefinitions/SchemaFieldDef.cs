#region + Using Directives

using System;

#endregion

// user name: jeffs
// created:   7/3/2021 7:16:58 AM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{

	public class SchemaFieldApp<TD> : SchemaFieldDef<TD, SchemaAppKey>
	{
		public SchemaFieldApp(SchemaAppKey sequence, string name, string desc, TD val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "") :
			base(sequence, name, desc, val, unitType, guid) {}
	}

	public class SchemaFieldRoot<TD> : SchemaFieldDef<TD, SchemaRootKey>
	{
		public SchemaFieldRoot(SchemaRootKey sequence, string name, string desc, TD val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "") :
			base(sequence, name, desc, val, unitType, guid) {}
	}

	public interface ISchemaFieldDef<TE> where TE : Enum
	{
		TE Key { get; }
		int Sequence { get;}
		string Name { get;}
		string Desc { get;}
		RevitUnitType UnitType { get;}
		string Guid { get; }
		string ValueString { get; }
		Type ValueType { get; }

		ISchemaFieldDef<TE> Clone();
	}

	public class SchemaFieldDef<TD, TE> : ISchemaFieldDef<TE> where TE: Enum 
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

		public string ValueString => Value.ToString();

		// [DataMember(Name = "RevitFieldValue", Order = 6)]
		public TD Value { get; set; }

		public SchemaFieldDef()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			Value = default;
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldDef(TE sequence, string name, string desc, TD val,
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
			SchemaFieldDef<TD, TE> copy = new SchemaFieldDef<TD, TE>();

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
