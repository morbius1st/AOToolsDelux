#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaDefinition;
using static AOTools.Cells.SchemaDefinition.SchemaRootKey;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDefRoot : ASchemaDef<SchemaRootKey, SchemaDictionaryRoot>
	{
		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";

		public override SchemaRootKey[] KeyOrder { get; set; }

		public override SchemaDictionaryRoot DefaultFields { get; } =
			new SchemaDictionaryRoot
			{
				{
					NAME,
					new SchemaFieldDef<SchemaRootKey>(NAME, "Name",
						"Name", ROOT_SCHEMA_NAME)
				},

				{
					DESCRIPTION,
					new SchemaFieldDef<SchemaRootKey>(DESCRIPTION, "Description",
						"Description", ROOT_SCHEMA_DESC)
				},

				{
					VERSION,
					new SchemaFieldDef<SchemaRootKey>(VERSION, "Version",
						"Cells Version", "1.0")
				},

				{
					DEVELOPER,
					new SchemaFieldDef<SchemaRootKey>(DEVELOPER, "Developer",
						"Developer", ROOT_DEVELOPER_NAME)
				},

				{
					APP_GUID,
					new SchemaFieldDef<SchemaRootKey>(APP_GUID, "UniqueAppGuidString",
						"Unique App Guid String", SchemaGuidManager.AppGuidUniqueString)
				},
			};
	}
}