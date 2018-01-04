using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace AOTools
{
		class PickedBox2
	{
		public XYZ Min;
		public XYZ Max;

		public PickedBox2(PickedBox picked, bool fixRange)
		{
			double minX = picked.Min.X;
			double minY = picked.Min.Y;
			double minZ = picked.Min.Z;

			double maxX = picked.Max.X;
			double maxY = picked.Max.Y;
			double maxZ = picked.Max.Z;

			if (picked.Min.X > picked.Max.X)
			{
				maxX = picked.Min.X;
				minX = picked.Max.X;
			}
			
			if (picked.Min.Y > picked.Max.Y)
			{
				maxY = picked.Min.Y;
				minY = picked.Max.Y;
			}

			if (fixRange)
			{
				minZ = Double.MinValue;
				maxZ = Double.MaxValue;
			}

			Min = new XYZ(minX, minY, minZ);
			Max = new XYZ(maxX, maxY, maxZ);
		}

		public PickedBox2(PickedBox picked) : this(picked, false) { }
	}
}
