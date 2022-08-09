#region + Using Directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using CsDeluxMeasure.UnitsUtil;

#endregion

// user name: jeffs
// created:   7/10/2022 8:16:33 PM

namespace CsDeluxMeasure.Windows.Support
{
	public class NameValidation : ValidationRule
	{
		public UnitsManager  uMgr { get; set; }

		// validation requirements
		// min 4 characters
		// must start with alpha (uc or lc)
		// middle is alphanumeric, space, or dash
		// must end with alphanumeric (no dash, no space)
		// name must be unique

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string text = (string) value;

			if (text == null || text.Length < 4) return new ValidationResult(false, "Name must be a minimum of 4 characters");

			Regex r = new Regex("^[a-zA-Z][a-zA-Z0-9 -]{2,}[a-zA-Z0-9]{1}$");

			if (!r.IsMatch(text)) return new ValidationResult(false, "Name does not meet requirements");

			if (uMgr.HasNameUserList(text)) return new ValidationResult(false, "Name is already in use");

			return ValidationResult.ValidResult;
		}
	}
}
