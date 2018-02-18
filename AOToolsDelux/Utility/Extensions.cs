using Autodesk.Revit.DB;

namespace AOTools.Utility
{
	static class XYZExtensions
	{
		public static XYZ Multiply(this XYZ point, XYZ multiplier)
		{
			return new XYZ(point.X * multiplier.X, point.Y * multiplier.Y, point.Z * multiplier.Z);
		}
	}

}