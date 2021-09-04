#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

#endregion

// user name: jeffs
// created:   8/28/2021 9:54:13 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions
{
	public class SchemaCellDataField<TD> : SchemaDataFieldDef<SchemaCellKey, TD>
	{
		public SchemaCellDataField(TD value, ISchemaFieldDef<SchemaCellKey> fieldDef)
		{
			Value = value;
			FieldDef = fieldDef;
			ValueType = typeof(TD);
		}
	}

	public class SchemaRootAppDataField<TD> : SchemaDataFieldDef<SchemaRootAppKey, TD>
	{
		public SchemaRootAppDataField(TD value, ISchemaFieldDef<SchemaRootAppKey> fieldDef)
		{
			Value = value;
			FieldDef = fieldDef;
			ValueType = typeof(TD);
		}
	}

	
	public class SchemaAppDataField<TD> : SchemaDataFieldDef<SchemaAppKey, TD>
	{
		public SchemaAppDataField(TD value, ISchemaFieldDef<SchemaAppKey> fieldDef)
		{
			Value = value;
			FieldDef = fieldDef;
			ValueType = typeof(TD);
		}
	}

		
	public class SchemaRootDataField<TD> : SchemaDataFieldDef<SchemaRootKey, TD>
	{
		public SchemaRootDataField(TD value, ISchemaFieldDef<SchemaRootKey> fieldDef)
		{
			Value = value;
			FieldDef = fieldDef;
			ValueType = typeof(TD);
		}
	}


	public class SchemaDataFieldDef<TE, TD> : ASchemaDataFieldDef<TE> where TE : Enum
	{
		public override TE Key { get; protected set; }

		public TD Value { get; set; }

		public override ISchemaFieldDef<TE> FieldDef { get; protected set; }

		public override string ValueString => Value.ToString();

		public override Type ValueType { get; protected set; }

		public override ASchemaDataFieldDef<TE> Clone()
		{
			SchemaDataFieldDef<TE, TD> copy = new SchemaDataFieldDef<TE, TD>();

			copy.Key = Key;
			copy.FieldDef = FieldDef;
			copy.Value = Value;

			return copy;
		}

	}

	public abstract class ASchemaDataFieldDef<TE> where TE : Enum
	{
		public abstract TE Key { get; protected set; }
		public abstract ISchemaFieldDef<TE> FieldDef { get; protected set; }
		public abstract string ValueString { get; }
		public abstract Type ValueType { get; protected set; }

		public abstract ASchemaDataFieldDef<TE> Clone();

		
		public string AsString()
		{
			return As<string>();
		}

		public double AsDouble()
		{
			return As<double>();
		}

		public int AsInteger()
		{
			return As<int>();
		}
				
		public bool AsBoolean()
		{
			return As<bool>();
		}


		public TT As<TT>()
		{
			return ((SchemaDataFieldDef<TE, TT>) this).Value;
		}

		public static TT As<TT>(ASchemaDataFieldDef<TE> id)
		{
			return ((SchemaDataFieldDef<TE, TT>) id).Value;
		}

		public static string AsS(ASchemaDataFieldDef<TE> id)
		{
			return ((SchemaDataFieldDef<TE, string>) id).Value;
		}

		public static double AsD(ASchemaDataFieldDef<TE> id)
		{
			return ((SchemaDataFieldDef<TE, double>) id).Value;
		}
		
		public static int AsI(ASchemaDataFieldDef<TE> id)
		{
			return ((SchemaDataFieldDef<TE, int>) id).Value;
		}

		public static bool AsB(ASchemaDataFieldDef<TE> id)
		{
			return ((SchemaDataFieldDef<TE, bool>) id).Value;
		}

	}

}
