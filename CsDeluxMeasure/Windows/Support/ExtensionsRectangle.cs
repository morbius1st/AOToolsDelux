#region + Using Directives

using System.Drawing;
#endregion

// user name: jeffs
// created:   9/17/2023 4:31:46 PM

namespace CsDeluxMeasure.Windows.Support
{
	internal static class RectExtensions
	{

		public static Rectangle AsRectangle(this WindowApiUtilities.RECT r)
		{
			return new Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
		}

		public static Rectangle Scale(this Rectangle rc, double scaleFactor)
		{
			return new Rectangle(
				(int) (rc.Left   * scaleFactor),
				(int) (rc.Top    * scaleFactor),
				(int) (rc.Width  * scaleFactor),
				(int) (rc.Height * scaleFactor));
		}
	}
}
