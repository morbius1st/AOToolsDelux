#region Using directives
using System.Runtime.Serialization;
using DluxMeasure.UnitStyles;
using Point = System.Drawing.Point;

using UtilityLibrary;

#endregion

// itemname:	SettingsUsr
// username:	jeffs
// created:		1/3/2018 8:04:40 PM

namespace DluxMeasure.AppSettings.ConfigSettings
{
	[DataContract(Namespace = Header.NSpace)]
	public static class SettingsUsr
	{
		public static SettingsMgr<SettingsUsrBase> SmUsr { get; private set; }

		public static SettingsUsrBase SmUsrSetg { get; private set; }

		public static void SmUsrInit()
		{
			SmUsr = new SettingsMgr<SettingsUsrBase>(Reset);
			SmUsrSetg = SmUsr.Settings;
		}
		public static bool IsValid()
		{
			return SmUsr != null;
		}

		public static void Reset()
		{
			SmUsr = new SettingsMgr<SettingsUsrBase>(Reset);
			SmUsrSetg = SmUsr.Settings;
		}
	}

	[DataContract(Namespace = Header.NSpace)]
	public class SettingsUsrBase : SettingsPathFileUserBase
	{
		public sealed override string FileVersion { get; set; } = "1.0";

		[DataMember]
		public Point FormMeasurePointsLocation = new Point(0, 0);
		[DataMember]
		public bool MeasurePointsShowWorkplane = false;

		[DataMember]
		public UnitStyleType DxMeasureUnitStyle { get; set; } = UnitStyleType.PROJECT;


		[DataMember]
		public UnitStyleType DxMeasureUnitStyleAlt { get; set; } = UnitStyleType.PROJECT;

	}

}
