#region using

using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using CsDeluxMeasure.UnitsUtil;
using UtilityLibrary;
using System.Security.Cryptography;
using CsDeluxMeasure.Windows.Support;

#endregion

// username: jeffs
// created:  9/14/2022 7:44:28 PM

namespace CsDeluxMeasure.RevitSupport
{
	public class RevitInterOp
	{
	#region private fields

		private SplitButton sb;

		private List<PushButton> pbList = new List<PushButton>();

	#endregion

	#region ctor

		public RevitInterOp() { }

	#endregion

	#region public properties

		public SplitButton SB {
			get => sb;
			set => sb = value;
		}

		// public List<PushButton> PbList => pbList;

	#endregion

	#region private properties

	#endregion


	#region public methods

		
		public void AddPbToList(PushButton pb)
		{
			pbList.Add(pb);
		}


		public void UpdateRibbonButton(ListCollectionView c)
		{
			if (c.Count == 0) return;

			int i;
			for (i = 0; i < c.Count; i++)
			{
				updatePushButton(i, c.GetItemAt(i) as UnitsDataR);

				if (i == UnitStyleCmd.MAX_STYLE_CMDS) break;
			}

			for (int j = i; j < UnitStyleCmd.MAX_STYLE_CMDS; j++)
			{
				hidePushButton(i);
			}

			sb.CurrentButton=pbList[0];

		}

		public string MakePbTitle(string name)
		{
			if (name.Length < AppRibbon.MAX_RIBBON_NAME_WIDTH) return name;

			List<string> lines = CsStringUtil.StringDivide(name, new [] { ' ' }, AppRibbon.MAX_RIBBON_NAME_WIDTH, 0);

			return CsStringUtil.MakeMultiLineString(lines, AppRibbon.MAX_RIBBON_NAME_WIDTH);
		}

		public string MakePbToolTip(string desc)
		{
			string result = $"Set Project Units to {desc}";

			if (result.Length < AppRibbon.MAX_RIBBON_TOOLTIP_WIDTH) return result;

			List<string> lines = CsStringUtil.StringDivide(result, new [] { ' ' }, AppRibbon.MAX_RIBBON_TOOLTIP_WIDTH, 0);

			return CsStringUtil.MakeMultiLineString(lines, AppRibbon.MAX_RIBBON_TOOLTIP_WIDTH);
		}

		public string MakePbName(int idx, string name)
		{
			return $"{name}{idx:D2}";
		}

		public string MakePbHiddenTitle(int idx, string name)
		{
			return $"Name| {MakePbName(idx, name)}";
		}
		
		public string MakePbHiddenDesc(int idx, string desc)
		{
			return $"Desc| {MakePbName(idx, desc)}";
		}

		public void ListPbs()
		{
			int i = 0;
			foreach (PushButton pb in pbList)
			{
				Debug.WriteLine($"push button| {i++}| title| {pb.Name}| xxx| {pb.ItemText}");
			}
		}

	#endregion

	#region private methods

		private void updatePushButton(int idx, UnitsDataR udr)
		{
			string title = MakePbTitle(udr.Name);
			string toolTip = MakePbToolTip(udr.Description);

			// BitmapImage iconSm =  CsUtilitiesMedia.GetBitmapImage(udr.Ustyle.IconId, AppRibbon.NAMESPACE_PREFIX_RESOURCES);
			BitmapImage iconSm =  CsUtilitiesMedia.GetBitmapImageResource($"{AppRibbon.ICON_FOLDER}/{udr.Ustyle.IconId}");


			// BitmapImage iconLg =  CsUtilitiesMedia.GetBitmapImage(udr.Ustyle.IconId, AppRibbon.NAMESPACE_PREFIX_RESOURCES);
			BitmapImage iconLg =  CsUtilitiesMedia.GetBitmapImageResource($"{AppRibbon.ICON_FOLDER}/{udr.Ustyle.IconId}");

			pbList[idx].ItemText = title;
			pbList[idx].ToolTip = toolTip;
			pbList[idx].Image = iconSm;
			pbList[idx].LargeImage = iconLg;
			pbList[idx].Visible = true;
		}

		private void hidePushButton(int idx)
		{
			string title = MakePbHiddenTitle(idx, pbList[idx].Name);
			string toolTip = MakePbToolTip(pbList[idx].Name);

			BitmapImage iconSm = null;
			BitmapImage iconLg = null;

			pbList[idx].ItemText = title;
			pbList[idx].ToolTip = toolTip;
			pbList[idx].Image = iconSm;
			pbList[idx].LargeImage = iconLg;
			pbList[idx].Visible = false;
		}


	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitInterOp";
		}

	#endregion
	}
}