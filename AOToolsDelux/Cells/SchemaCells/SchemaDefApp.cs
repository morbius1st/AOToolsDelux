#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaDefinition;
using static AOTools.Cells.SchemaDefinition.SchemaAppKey;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaCells
{
	public abstract class SchemaDefApp
	{
		protected const string SCHEMA_NAME = "CellsApplication";
		protected const string SCHEMA_DESC = "Excel Cells to Revit Exchange";
		protected const string DEVELOPER_NAME = "CyberStudio";

		// private Guid schemaGuid = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B9470FF");
		private Guid appSchemaGuid;

		public Guid AppSchemaGuid
		{
			get => appSchemaGuid;
			private set => appSchemaGuid = value;
		}

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		private static SchemaFieldDef SubSchemaFieldInfo { get; } =
			new SchemaFieldDef(UNDEFINED, "RootCellsDefinition{0:D2}",
				"subschema for a cells definition", "");

		public static SchemaFieldDef GetSubSchemaDef(int id)
		{
			SchemaFieldDef subDef = SubSchemaFieldInfo;
			subDef.Name = string.Format(subDef.Name, id);
			subDef.Guid = SchemaGuidManager.GetCellGuid(id);

			return subDef;
		}

		public static SchemaDictionaryApp DefaultFields { get; } =
			new SchemaDictionaryApp
			{
				{
					NAME,
					new SchemaFieldDef(NAME, "Name",
						"Name", SCHEMA_NAME)
				},

				{
					DESCRIPTION,
					new SchemaFieldDef(DESCRIPTION, "Description",
						"Description", SCHEMA_DESC)
				},

				{
					VERSION,
					new SchemaFieldDef(VERSION, "Version",
						"Cells Version", "1.0")
				},

				{
					DEVELOPER,
					new SchemaFieldDef(DEVELOPER, "Developer",
						"Developer", DEVELOPER_NAME)
				},

			};
	}
}
