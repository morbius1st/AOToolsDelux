#region Using directives

#endregion

// itemname:	XMLClasses
// username:	jeffs
// created:		1/7/2018 3:47:41 PM


namespace CSToolsDelux.Utility
{
	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public partial class RevitAddIns
	{
		private RevitAddInsAddIn[] addInField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("AddIn")]
		public RevitAddInsAddIn[] AddIn
		{
			get { return this.addInField; }
			set { this.addInField = value; }
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class RevitAddInsAddIn
	{
		private string nameField;

		private string textField;

		private string descriptionField;

		private string assemblyField;

		private string fullClassNameField;

		private string clientIdField;

		private string vendorIdField;

		private string vendorDescriptionField;

		private string typeField;

		/// <remarks/>
		public string Name
		{
			get { return this.nameField; }
			set { this.nameField = value; }
		}

		/// <remarks/>
		public string Text
		{
			get { return this.textField; }
			set { this.textField = value; }
		}

		/// <remarks/>
		public string Description
		{
			get { return this.descriptionField; }
			set { this.descriptionField = value; }
		}

		/// <remarks/>
		public string Assembly
		{
			get { return this.assemblyField; }
			set { this.assemblyField = value; }
		}

		/// <remarks/>
		public string FullClassName
		{
			get { return this.fullClassNameField; }
			set { this.fullClassNameField = value; }
		}

		/// <remarks/>
		public string ClientId
		{
			get { return this.clientIdField; }
			set { this.clientIdField = value; }
		}

		/// <remarks/>
		public string VendorId
		{
			get { return this.vendorIdField; }
			set { this.vendorIdField = value; }
		}

		/// <remarks/>
		public string VendorDescription
		{
			get { return this.vendorDescriptionField; }
			set { this.vendorDescriptionField = value; }
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Type
		{
			get { return this.typeField; }
			set { this.typeField = value; }
		}
	}
}