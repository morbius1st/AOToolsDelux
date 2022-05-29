#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Windows.Media;
using EnvDTE;
using UtilityLibrary;


#endregion

// user name: jeffs
// created:   5/9/2020 10:23:38 PM


namespace CSToolsStudies.Windows.Support
{
	
	[MarkupExtensionReturnType(typeof(System.Windows.Media.Color))]
	public class CsColor : MarkupExtension, INotifyPropertyChanged
	{
		private System.Windows.Media.Color c;

		public CsColor() { }

		public System.Windows.Media.Color Color 
		{
			get => c;
			set
			{
				c = value;
				OnPropertyChanged();
			}
		}

		public System.Windows.Media.Color ColorX
		{
			get => c;
			set
			{
				c = value;
				OnPropertyChanged();
			}
		}


		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return (System.Windows.Media.Color) Color;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}


	[MarkupExtensionReturnType(typeof(System.Windows.Media.Color))]
	public class XmalColor : MarkupExtension
	{
		private System.Windows.Media.Color? c;
		private byte? a;
		private byte? r;
		private byte? g;
		private byte? b;

		public XmalColor() { }

		public System.Windows.Media.Color Color
		{
			get => c.Value;
			set
			{
				c = value;
			}
		}

		public byte R
		{
			get
			{
				return ((byte)(r.HasValue ? r.Value : c.HasValue ? c.Value.R : 255));
			}
			set
			{
				r = value;
			}
		}

		public byte G
		{
			get
			{
				return ((byte)(g.HasValue ? g.Value : c.HasValue ? c.Value.G : 255));
			}
			set
			{
				g = value;
			}
		}

		public byte B
		{
			get
			{
				return ((byte)(b.HasValue ? b.Value : c.HasValue ? c.Value.B : 255));
			}
			set
			{
				b = value;
			}
		}

		public byte A
		{
			get
			{
				return ((byte)(a.HasValue ? a.Value : c.HasValue ? c.Value.A : 255));
			}
			set
			{
				a = value;
			}
		}

		public System.Windows.Media.Color ToColor()
		{
			// return new SolidColorBrush(Color.FromArgb(
			// 	(byte) (A.HasValue ? A.Value : 255), R, G, B));

			return System.Windows.Media.Color.FromArgb(A, R, G, B);

		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return (System.Windows.Media.Color)ToColor();
			
		}
	}

	[MarkupExtensionReturnType(typeof(Brush))]
	public class ScBrush : MarkupExtension
	{
		private Color c;

		public ScBrush() { }

		public Color color
		{
			get => c;
			set
			{
				c = value;
				R = c.R;
				G = c.G;
				B = c.B;
			}
		}

		public byte R { get; set; }

		public byte G { get; set; }

		public byte B { get; set; }

		public byte? A { get; set; }

		public System.Windows.Media.Brush ToBrush()
		{
			return new SolidColorBrush(Color.FromArgb(
				(byte)(A.HasValue ? A.Value : 255), R, G, B));
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return ToBrush();
		}
	}

	[MarkupExtensionReturnType(typeof(Int32))]
	public class EnumToInt32 : MarkupExtension
	{
		public Enum e { get; set; }

		public EnumToInt32() { }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Convert.ToInt32(e);
		}
	}

	
	[MarkupExtensionReturnType(typeof(bool?))]
	public class NullableBool : MarkupExtension
	{
		private bool? bx;
		public bool? b
		{
			get => bx;
			set
			{
				bx = value;
			}
		}

		public NullableBool() { }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return bx;
		}
	}

}