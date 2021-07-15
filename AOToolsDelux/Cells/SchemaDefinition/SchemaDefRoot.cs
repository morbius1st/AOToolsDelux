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
	public class SchemaDefRoot : ASchemaDef<SchemaRootKey>
	{
		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";

		// public static string[] FIELD_NAMES;

		private SchemaDefRoot()
		{
			FIELD_NAMES = new string[5];
			FIELD_NAMES[(int) RK_NAME]        = "Name";
			FIELD_NAMES[(int) RK_DESCRIPTION] = "Description";
			FIELD_NAMES[(int) RK_VERSION]     = "Version";
			FIELD_NAMES[(int) RK_DEVELOPER]   = "Developer";
			FIELD_NAMES[(int) RK_APP_GUID]    = "UniqueAppGuidString";
		}

		public static SchemaDefRoot Inst { get; } = new SchemaDefRoot();

		public override string this[SchemaRootKey key]
		{
			get
			{
				return FIELD_NAMES[(int) key];
			}
		}

		public override SchemaDictionaryBase<string> DefaultFields { get; } =
			new SchemaDictionaryRoot
			{
				{
					Inst[RK_NAME],
					new SchemaFieldDef(Inst[RK_NAME], "Name", ROOT_SCHEMA_NAME)
				},

				{
					Inst[RK_DESCRIPTION],
					new SchemaFieldDef(Inst[RK_DESCRIPTION], "Description", ROOT_SCHEMA_DESC)
				},

				{
					Inst[RK_VERSION],
					new SchemaFieldDef(Inst[RK_VERSION], "Cells Version", "1.0")
				},

				{
					Inst[RK_DEVELOPER],
					new SchemaFieldDef(Inst[RK_DEVELOPER], "Developer", ROOT_DEVELOPER_NAME)
				},

				{
					Inst[RK_APP_GUID],
					new SchemaFieldDef(Inst[RK_APP_GUID], "Unique App Guid String", 
						SchemaGuidManager.AppGuidUniqueString)
				},
			};
	}
}