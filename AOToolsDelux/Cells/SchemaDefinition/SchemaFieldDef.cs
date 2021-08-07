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

	public interface ISchemaFieldDef<TE> : ICopyable<ISchemaFieldDef<TE>> where TE : Enum
	{
		TE Key { get; }
		Type ValueType { get; }
		// ISchemaFieldDef<TE> Clone();
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
		// public dynamic Value { get; set; }
		public IExData ExValue { get; set; }

		public SchemaFieldDef()
		{
			Sequence = -1;
			Name = null;
			Desc = null;
			ExValue = null;
			ValueType = typeof(object);
			UnitType = RevitUnitType.UT_UNDEFINED;
			Guid = null;
		}

		public SchemaFieldDef(TE sequence, string name, string desc, IExData val,
			RevitUnitType unitType = RevitUnitType.UT_UNDEFINED, string guid = "")
		{
			Key = sequence;
			Sequence = (int)(object) sequence;
			Name = name;
			Desc = desc;
			ExValue = val;
			ValueType = val.Type;
			UnitType = unitType;
			Guid = guid;
		}


		public SchemaFieldDef(SchemaFieldDef<TE> fi)
		{
			Key = fi.Key;
			Sequence = fi.Sequence;
			Name = fi.Name;
			Desc = fi.Desc;
			ExValue = fi.ExValue;
			ValueType = fi.ValueType;
			UnitType = fi.UnitType;
			Guid = fi.Guid;
		}

		// // master switch routine
		// public dynamic ExtractValue(Entity e, Field f)
		// {
		// 	return ExtractValue(Value, e, f);
		// }
		//
		// // sub-routine
		// private Entity ExtractValue(Entity key, Entity e, Field f)
		// {
		// 	return e.Get<Entity>(f);
		// }
		//
		// private string ExtractValue(string key, Entity e, Field f)
		// {
		// 	return e.Get<string>(f);
		// }
		//
		// private int ExtractValue(int key, Entity e, Field f)
		// {
		// 	return e.Get<int>(f);
		// }
		//
		// private bool ExtractValue(bool key, Entity e, Field f)
		// {
		// 	return e.Get<bool>(f);
		// }
		//
		// private double ExtractValue(double key, Entity e, Field f)
		// {
		// 	return e.Get<double>(f, DisplayUnitType.DUT_GENERAL);
		// }

		// public SchemaFieldDef<T> Clone()
		// {
		// 	// SchemaFieldDef copy = new SchemaFieldDef(Sequence, Name, Desc, Value, UnitType, Guid);
		// 	return new SchemaFieldDef<T>(this);
		// }

		// public ISchemaFieldDef<TE> Clone()
		// {
		// 	return Copy();
		// }
		//
		public ISchemaFieldDef<TE> Copy()
		{
			return Copy2();
		}

		public SchemaFieldDef<TE> Copy2()
		{
			SchemaFieldDef<TE> copy = new SchemaFieldDef<TE>();

			copy.Key			= Key;
			copy.Sequence		= Sequence;
			copy.Name			= Name;
			copy.Desc			= Desc;
			copy.ValueType		= ValueType;
			copy.UnitType		= UnitType;
			copy.Guid			= Guid;
			copy.ExValue		= ExValue;

			return copy;
		}
	}
}
