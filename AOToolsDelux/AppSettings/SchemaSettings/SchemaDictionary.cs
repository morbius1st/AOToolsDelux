using System.Runtime.Serialization;

namespace AOToolsDelux.AppSettings.SchemaSettings
{
//	[CollectionDataContract(Name = "SchemaFields", KeyName = "OrderKey", 
//		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]

[CollectionDataContract(Name = "AppData", KeyName = "OrderKey",
		ValueName = "SchemaField", ItemName = "SchemaFieldItem", 
		Namespace = "")]
	public class SchemaDictionaryApp : SchemaDictionaryBase<SchemaAppKey>
	{
		public SchemaDictionaryApp() { }
		public SchemaDictionaryApp(int capacity) :base(capacity) { }
		public SchemaDictionaryApp Clone()
		{
			return Clone(this);
		}
	}

	[CollectionDataContract(Name = "UnitStyle", KeyName = "OrderKey",
	ValueName = "SchemaField", ItemName = "SchemaFieldItem",
	Namespace = "")]
	public class SchemaDictionaryUsr : SchemaDictionaryBase<SchemaUsrKey>
	{
		public SchemaDictionaryUsr() { }
		public SchemaDictionaryUsr(int capacity) : base(capacity) { }
		public SchemaDictionaryUsr Clone()
		{
			return Clone(this);
		}
	}
}