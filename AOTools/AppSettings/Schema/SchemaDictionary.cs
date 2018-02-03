using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AOTools.AppSettings
{
	[CollectionDataContract(Name = "SchemaFields", KeyName = "OrderKey", 
		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
	public class SchemaDictionaryBase<T> : Dictionary<T, SchemaFieldUnit> 
	{
		public SchemaDictionaryBase() { }

		public SchemaDictionaryBase(int capacity) : base(capacity) { }

		protected TU Clone<TU>(TU original) where TU : SchemaDictionaryBase<T>, new()
		{
			TU copy = new TU();

			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in original)
			{
				copy.Add(kvp.Key, new SchemaFieldUnit(kvp.Value));
			}
			return copy;
		}
	}

	[CollectionDataContract(Name = "SchemaFields", KeyName = "OrderKey",
		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
	public class SchemaDictionaryApp : SchemaDictionaryBase<SchemaAppKey>
	{
		public SchemaDictionaryApp() { }
		public SchemaDictionaryApp(int capacity) :base(capacity) { }
		public SchemaDictionaryApp Clone()
		{
			return Clone(this);
		}
	}

	[CollectionDataContract(Name = "SchemaFields", KeyName = "OrderKey",
		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
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