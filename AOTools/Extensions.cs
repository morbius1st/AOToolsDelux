using System.Collections.Generic;
using AOTools.Settings;
using Autodesk.Revit.DB;

namespace AOTools
{
	static class XYZExtensions
	{
		public static XYZ Multiply(this XYZ point, XYZ multiplier)
		{
			return new XYZ(point.X * multiplier.X, point.Y * multiplier.Y, point.Z * multiplier.Z);
		}
	}

	public static class Extensions
	{
		public static FieldInfo Clone(this FieldInfo fi)
		{
			return new FieldInfo(fi);
		}

//		public static SchemaDictionary<T, FieldInfo> 
//			Clone<T>(this SchemaDictionary<T, FieldInfo> d) where T : SchemaKey
//
//		{
//			SchemaDictionary<T, FieldInfo> copy = 
//				new SchemaDictionary<T, FieldInfo>(d.Count);
//
//			foreach (KeyValuePair<T, FieldInfo> kvp in d)
//			{
//				copy.Add(kvp.Key, new FieldInfo(kvp.Value));
//			}
//
//			return copy;
//		}
	}
}