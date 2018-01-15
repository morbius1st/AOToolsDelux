using System.Collections.Generic;
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
			return new FieldInfo(fi.Key, fi.Name,fi.Desc, fi.Value, 
				fi.UnitType, fi.Guid);
		}

		public static Dictionary<T, FieldInfo> 
			Clone<T>(this Dictionary<T, FieldInfo> d) where T : SchemaKey

		{
			Dictionary<T, FieldInfo> copy = 
				new Dictionary<T, FieldInfo>(d.Count);

			foreach (KeyValuePair<T, FieldInfo> kvp in d)
			{
				copy.Add(kvp.Key, new FieldInfo(kvp.Value));
			}

			return copy;
		}
	}
}