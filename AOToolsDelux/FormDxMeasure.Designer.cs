using AOTools.UnitStyles;

namespace AOTools
{
	partial class FormDxMeasure
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.Label lblBar1;
			System.Windows.Forms.Label lblBar2;
			System.Windows.Forms.Label lblBar3;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDxMeasure));
			this.lblNote = new System.Windows.Forms.Label();
			this.cbxWpOnOff = new System.Windows.Forms.CheckBox();
			this.lblDistX = new System.Windows.Forms.TextBox();
			this.lblDistXY = new System.Windows.Forms.TextBox();
			this.lblDistYZ = new System.Windows.Forms.TextBox();
			this.lblDistXYZ = new System.Windows.Forms.TextBox();
			this.lblDistXZ = new System.Windows.Forms.TextBox();
			this.lblDistZ = new System.Windows.Forms.TextBox();
			this.lblDistY = new System.Windows.Forms.TextBox();
			this.lblAlongXaxis = new System.Windows.Forms.Label();
			this.lblDistances = new System.Windows.Forms.Label();
			this.lblP2Z = new System.Windows.Forms.Label();
			this.lblP2Y = new System.Windows.Forms.Label();
			this.lblP2X = new System.Windows.Forms.Label();
			this.lblP1Z = new System.Windows.Forms.Label();
			this.lblP1Y = new System.Windows.Forms.Label();
			this.lblP1X = new System.Windows.Forms.Label();
			this.lblPoint2 = new System.Windows.Forms.Label();
			this.lblZ1 = new System.Windows.Forms.Label();
			this.lblY1 = new System.Windows.Forms.Label();
			this.lblPoint1 = new System.Windows.Forms.Label();
			this.lblCoordinates = new System.Windows.Forms.Label();
			this.lblX1 = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnDone = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblAxis = new System.Windows.Forms.Label();
			this.lblYZ1 = new System.Windows.Forms.Label();
			this.lblXZ1 = new System.Windows.Forms.Label();
			this.lblXY1 = new System.Windows.Forms.Label();
			this.lblXYZ = new System.Windows.Forms.Label();
			this.rbUnitFtFracIn = new System.Windows.Forms.RadioButton();
			this.rbUnitFracIn = new System.Windows.Forms.RadioButton();
			this.rbUnitDecFt = new System.Windows.Forms.RadioButton();
			this.rbUnitDecIn = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.rbUnitsProj = new System.Windows.Forms.RadioButton();
			this.tbxMessage = new System.Windows.Forms.TextBox();
			this.rbUnitFtDecIn = new System.Windows.Forms.RadioButton();
			this.rbUnitFractFt = new System.Windows.Forms.RadioButton();
			lblBar1 = new System.Windows.Forms.Label();
			lblBar2 = new System.Windows.Forms.Label();
			lblBar3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblBar1
			// 
			lblBar1.BackColor = System.Drawing.Color.Silver;
			lblBar1.Enabled = false;
			lblBar1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblBar1.Location = new System.Drawing.Point(110, 44);
			lblBar1.MaximumSize = new System.Drawing.Size(0, 4);
			lblBar1.MinimumSize = new System.Drawing.Size(460, 0);
			lblBar1.Name = "lblBar1";
			lblBar1.Size = new System.Drawing.Size(468, 4);
			lblBar1.TabIndex = 114;
			// 
			// lblBar2
			// 
			lblBar2.BackColor = System.Drawing.Color.Silver;
			lblBar2.Enabled = false;
			lblBar2.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblBar2.Location = new System.Drawing.Point(118, 147);
			lblBar2.MaximumSize = new System.Drawing.Size(0, 4);
			lblBar2.MinimumSize = new System.Drawing.Size(460, 0);
			lblBar2.Name = "lblBar2";
			lblBar2.Size = new System.Drawing.Size(460, 4);
			lblBar2.TabIndex = 115;
			// 
			// lblBar3
			// 
			lblBar3.BackColor = System.Drawing.Color.Silver;
			lblBar3.Enabled = false;
			lblBar3.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblBar3.Location = new System.Drawing.Point(118, 221);
			lblBar3.MaximumSize = new System.Drawing.Size(0, 4);
			lblBar3.MinimumSize = new System.Drawing.Size(460, 0);
			lblBar3.Name = "lblBar3";
			lblBar3.Size = new System.Drawing.Size(460, 4);
			lblBar3.TabIndex = 117;
			// 
			// lblNote
			// 
			this.lblNote.AutoSize = true;
			this.lblNote.Location = new System.Drawing.Point(13, 13);
			this.lblNote.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(399, 18);
			this.lblNote.TabIndex = 42;
			this.lblNote.Text = "Note: point selection is projected into the current work plane";
			// 
			// cbxWpOnOff
			// 
			this.cbxWpOnOff.AutoSize = true;
			this.cbxWpOnOff.Location = new System.Drawing.Point(424, 11);
			this.cbxWpOnOff.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.cbxWpOnOff.Name = "cbxWpOnOff";
			this.cbxWpOnOff.Size = new System.Drawing.Size(153, 22);
			this.cbxWpOnOff.TabIndex = 95;
			this.cbxWpOnOff.Text = "Display Work Plane";
			this.cbxWpOnOff.UseVisualStyleBackColor = true;
			this.cbxWpOnOff.CheckedChanged += new System.EventHandler(this.cbxWpOnOff_CheckedChanged);
			// 
			// lblDistX
			// 
			this.lblDistX.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistX.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistX.Location = new System.Drawing.Point(110, 58);
			this.lblDistX.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistX.Name = "lblDistX";
			this.lblDistX.ReadOnly = true;
			this.lblDistX.Size = new System.Drawing.Size(124, 18);
			this.lblDistX.TabIndex = 69;
			this.lblDistX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistXY
			// 
			this.lblDistXY.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistXY.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistXY.Location = new System.Drawing.Point(110, 86);
			this.lblDistXY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistXY.Name = "lblDistXY";
			this.lblDistXY.ReadOnly = true;
			this.lblDistXY.Size = new System.Drawing.Size(124, 18);
			this.lblDistXY.TabIndex = 81;
			this.lblDistXY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistYZ
			// 
			this.lblDistYZ.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistYZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistYZ.Location = new System.Drawing.Point(450, 86);
			this.lblDistYZ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistYZ.Name = "lblDistYZ";
			this.lblDistYZ.ReadOnly = true;
			this.lblDistYZ.Size = new System.Drawing.Size(124, 18);
			this.lblDistYZ.TabIndex = 79;
			this.lblDistYZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistXYZ
			// 
			this.lblDistXYZ.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistXYZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistXYZ.Location = new System.Drawing.Point(110, 110);
			this.lblDistXYZ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistXYZ.Name = "lblDistXYZ";
			this.lblDistXYZ.ReadOnly = true;
			this.lblDistXYZ.Size = new System.Drawing.Size(124, 18);
			this.lblDistXYZ.TabIndex = 77;
			this.lblDistXYZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistXZ
			// 
			this.lblDistXZ.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistXZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistXZ.Location = new System.Drawing.Point(280, 86);
			this.lblDistXZ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistXZ.Name = "lblDistXZ";
			this.lblDistXZ.ReadOnly = true;
			this.lblDistXZ.Size = new System.Drawing.Size(124, 18);
			this.lblDistXZ.TabIndex = 75;
			this.lblDistXZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistZ
			// 
			this.lblDistZ.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistZ.Location = new System.Drawing.Point(450, 58);
			this.lblDistZ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistZ.Name = "lblDistZ";
			this.lblDistZ.ReadOnly = true;
			this.lblDistZ.Size = new System.Drawing.Size(124, 18);
			this.lblDistZ.TabIndex = 73;
			this.lblDistZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblDistY
			// 
			this.lblDistY.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblDistY.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblDistY.Location = new System.Drawing.Point(280, 58);
			this.lblDistY.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistY.Name = "lblDistY";
			this.lblDistY.ReadOnly = true;
			this.lblDistY.Size = new System.Drawing.Size(124, 18);
			this.lblDistY.TabIndex = 71;
			this.lblDistY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblAlongXaxis
			// 
			this.lblAlongXaxis.AutoSize = true;
			this.lblAlongXaxis.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAlongXaxis.Location = new System.Drawing.Point(11, 211);
			this.lblAlongXaxis.Margin = new System.Windows.Forms.Padding(5, 8, 5, 4);
			this.lblAlongXaxis.Name = "lblAlongXaxis";
			this.lblAlongXaxis.Size = new System.Drawing.Size(99, 18);
			this.lblAlongXaxis.TabIndex = 68;
			this.lblAlongXaxis.Text = "Unit Display";
			// 
			// lblDistances
			// 
			this.lblDistances.AutoSize = true;
			this.lblDistances.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDistances.Location = new System.Drawing.Point(11, 35);
			this.lblDistances.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblDistances.Name = "lblDistances";
			this.lblDistances.Size = new System.Drawing.Size(81, 18);
			this.lblDistances.TabIndex = 67;
			this.lblDistances.Text = "Distances";
			// 
			// lblP2Z
			// 
			this.lblP2Z.BackColor = System.Drawing.SystemColors.Control;
			this.lblP2Z.Location = new System.Drawing.Point(450, 184);
			this.lblP2Z.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP2Z.Name = "lblP2Z";
			this.lblP2Z.Size = new System.Drawing.Size(124, 18);
			this.lblP2Z.TabIndex = 66;
			this.lblP2Z.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblP2Y
			// 
			this.lblP2Y.BackColor = System.Drawing.SystemColors.Control;
			this.lblP2Y.Location = new System.Drawing.Point(280, 184);
			this.lblP2Y.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP2Y.Name = "lblP2Y";
			this.lblP2Y.Size = new System.Drawing.Size(124, 18);
			this.lblP2Y.TabIndex = 65;
			this.lblP2Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblP2X
			// 
			this.lblP2X.BackColor = System.Drawing.SystemColors.Control;
			this.lblP2X.Location = new System.Drawing.Point(110, 184);
			this.lblP2X.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP2X.Name = "lblP2X";
			this.lblP2X.Size = new System.Drawing.Size(124, 18);
			this.lblP2X.TabIndex = 64;
			this.lblP2X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblP1Z
			// 
			this.lblP1Z.BackColor = System.Drawing.SystemColors.Control;
			this.lblP1Z.Location = new System.Drawing.Point(450, 160);
			this.lblP1Z.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP1Z.Name = "lblP1Z";
			this.lblP1Z.Size = new System.Drawing.Size(124, 18);
			this.lblP1Z.TabIndex = 63;
			this.lblP1Z.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblP1Y
			// 
			this.lblP1Y.BackColor = System.Drawing.SystemColors.Control;
			this.lblP1Y.Location = new System.Drawing.Point(280, 160);
			this.lblP1Y.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP1Y.Name = "lblP1Y";
			this.lblP1Y.Size = new System.Drawing.Size(124, 18);
			this.lblP1Y.TabIndex = 62;
			this.lblP1Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblP1X
			// 
			this.lblP1X.BackColor = System.Drawing.SystemColors.Control;
			this.lblP1X.Location = new System.Drawing.Point(110, 160);
			this.lblP1X.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblP1X.Name = "lblP1X";
			this.lblP1X.Size = new System.Drawing.Size(124, 18);
			this.lblP1X.TabIndex = 61;
			this.lblP1X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPoint2
			// 
			this.lblPoint2.AutoSize = true;
			this.lblPoint2.Location = new System.Drawing.Point(21, 184);
			this.lblPoint2.Margin = new System.Windows.Forms.Padding(5, 8, 5, 4);
			this.lblPoint2.Name = "lblPoint2";
			this.lblPoint2.Size = new System.Drawing.Size(52, 18);
			this.lblPoint2.TabIndex = 60;
			this.lblPoint2.Text = "Point 2";
			// 
			// lblZ1
			// 
			this.lblZ1.AutoSize = true;
			this.lblZ1.Location = new System.Drawing.Point(424, 57);
			this.lblZ1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblZ1.Name = "lblZ1";
			this.lblZ1.Size = new System.Drawing.Size(16, 18);
			this.lblZ1.TabIndex = 59;
			this.lblZ1.Text = "Z";
			// 
			// lblY1
			// 
			this.lblY1.AutoSize = true;
			this.lblY1.Location = new System.Drawing.Point(255, 58);
			this.lblY1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblY1.Name = "lblY1";
			this.lblY1.Size = new System.Drawing.Size(18, 18);
			this.lblY1.TabIndex = 58;
			this.lblY1.Text = "Y";
			// 
			// lblPoint1
			// 
			this.lblPoint1.AutoSize = true;
			this.lblPoint1.Location = new System.Drawing.Point(21, 160);
			this.lblPoint1.Margin = new System.Windows.Forms.Padding(5, 8, 5, 4);
			this.lblPoint1.Name = "lblPoint1";
			this.lblPoint1.Size = new System.Drawing.Size(52, 18);
			this.lblPoint1.TabIndex = 57;
			this.lblPoint1.Text = "Point 1";
			// 
			// lblCoordinates
			// 
			this.lblCoordinates.AutoSize = true;
			this.lblCoordinates.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCoordinates.Location = new System.Drawing.Point(11, 137);
			this.lblCoordinates.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblCoordinates.Name = "lblCoordinates";
			this.lblCoordinates.Size = new System.Drawing.Size(98, 18);
			this.lblCoordinates.TabIndex = 56;
			this.lblCoordinates.Text = "Coordinates";
			// 
			// lblX1
			// 
			this.lblX1.AutoSize = true;
			this.lblX1.Location = new System.Drawing.Point(85, 58);
			this.lblX1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblX1.Name = "lblX1";
			this.lblX1.Size = new System.Drawing.Size(17, 18);
			this.lblX1.TabIndex = 55;
			this.lblX1.Text = "X";
			// 
			// btnSelect
			// 
			this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSelect.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSelect.Location = new System.Drawing.Point(292, 319);
			this.btnSelect.Margin = new System.Windows.Forms.Padding(11, 4, 11, 4);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(131, 47);
			this.btnSelect.TabIndex = 54;
			this.btnSelect.Text = "Select Points";
			this.btnSelect.UseVisualStyleBackColor = true;
			// 
			// btnDone
			// 
			this.btnDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnDone.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDone.Location = new System.Drawing.Point(439, 319);
			this.btnDone.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(131, 47);
			this.btnDone.TabIndex = 53;
			this.btnDone.Text = "Done";
			this.btnDone.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(424, 160);
			this.label1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 18);
			this.label1.TabIndex = 98;
			this.label1.Text = "Z";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(255, 160);
			this.label2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 18);
			this.label2.TabIndex = 97;
			this.label2.Text = "Y";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(85, 160);
			this.label3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(17, 18);
			this.label3.TabIndex = 96;
			this.label3.Text = "X";
			// 
			// lblAxis
			// 
			this.lblAxis.AutoSize = true;
			this.lblAxis.Location = new System.Drawing.Point(21, 58);
			this.lblAxis.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblAxis.Name = "lblAxis";
			this.lblAxis.Size = new System.Drawing.Size(34, 18);
			this.lblAxis.TabIndex = 99;
			this.lblAxis.Text = "Axis";
			// 
			// lblYZ1
			// 
			this.lblYZ1.AutoSize = true;
			this.lblYZ1.Location = new System.Drawing.Point(416, 86);
			this.lblYZ1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblYZ1.Name = "lblYZ1";
			this.lblYZ1.Size = new System.Drawing.Size(26, 18);
			this.lblYZ1.TabIndex = 102;
			this.lblYZ1.Text = "YZ";
			// 
			// lblXZ1
			// 
			this.lblXZ1.AutoSize = true;
			this.lblXZ1.Location = new System.Drawing.Point(244, 86);
			this.lblXZ1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblXZ1.Name = "lblXZ1";
			this.lblXZ1.Size = new System.Drawing.Size(25, 18);
			this.lblXZ1.TabIndex = 101;
			this.lblXZ1.Text = "XZ";
			// 
			// lblXY1
			// 
			this.lblXY1.AutoSize = true;
			this.lblXY1.Location = new System.Drawing.Point(75, 86);
			this.lblXY1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblXY1.Name = "lblXY1";
			this.lblXY1.Size = new System.Drawing.Size(27, 18);
			this.lblXY1.TabIndex = 100;
			this.lblXY1.Text = "XY";
			// 
			// lblXYZ
			// 
			this.lblXYZ.AutoSize = true;
			this.lblXYZ.Location = new System.Drawing.Point(67, 110);
			this.lblXYZ.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.lblXYZ.Name = "lblXYZ";
			this.lblXYZ.Size = new System.Drawing.Size(35, 18);
			this.lblXYZ.TabIndex = 104;
			this.lblXYZ.Text = "XYZ";
			// 
			// rbUnitFtFracIn
			// 
			this.rbUnitFtFracIn.AutoSize = true;
			this.rbUnitFtFracIn.Location = new System.Drawing.Point(23, 262);
			this.rbUnitFtFracIn.Name = "rbUnitFtFracIn";
			this.rbUnitFtFracIn.Size = new System.Drawing.Size(202, 22);
			this.rbUnitFtFracIn.TabIndex = 106;
			this.rbUnitFtFracIn.Text = "Feet and Fractional Inches";
			this.rbUnitFtFracIn.UseVisualStyleBackColor = true;
			this.rbUnitFtFracIn.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// rbUnitFracIn
			// 
			this.rbUnitFracIn.AutoSize = true;
			this.rbUnitFracIn.Location = new System.Drawing.Point(424, 234);
			this.rbUnitFracIn.Name = "rbUnitFracIn";
			this.rbUnitFracIn.Size = new System.Drawing.Size(139, 22);
			this.rbUnitFracIn.TabIndex = 107;
			this.rbUnitFracIn.Text = "Fractional Inches";
			this.rbUnitFracIn.UseVisualStyleBackColor = true;
			this.rbUnitFracIn.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// rbUnitDecFt
			// 
			this.rbUnitDecFt.AutoSize = true;
			this.rbUnitDecFt.Location = new System.Drawing.Point(255, 262);
			this.rbUnitDecFt.Name = "rbUnitDecFt";
			this.rbUnitDecFt.Size = new System.Drawing.Size(113, 22);
			this.rbUnitDecFt.TabIndex = 108;
			this.rbUnitDecFt.Text = "Decimal Feet";
			this.rbUnitDecFt.UseVisualStyleBackColor = true;
			this.rbUnitDecFt.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// rbUnitDecIn
			// 
			this.rbUnitDecIn.AutoSize = true;
			this.rbUnitDecIn.Location = new System.Drawing.Point(424, 262);
			this.rbUnitDecIn.Name = "rbUnitDecIn";
			this.rbUnitDecIn.Size = new System.Drawing.Size(128, 22);
			this.rbUnitDecIn.TabIndex = 109;
			this.rbUnitDecIn.Text = "Decimal Inches";
			this.rbUnitDecIn.UseVisualStyleBackColor = true;
			this.rbUnitDecIn.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(424, 184);
			this.label5.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 18);
			this.label5.TabIndex = 112;
			this.label5.Text = "Z";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(255, 184);
			this.label9.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(18, 18);
			this.label9.TabIndex = 111;
			this.label9.Text = "Y";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(85, 184);
			this.label11.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 18);
			this.label11.TabIndex = 110;
			this.label11.Text = "X";
			// 
			// rbUnitsProj
			// 
			this.rbUnitsProj.AutoSize = true;
			this.rbUnitsProj.Checked = true;
			this.rbUnitsProj.Location = new System.Drawing.Point(23, 234);
			this.rbUnitsProj.Name = "rbUnitsProj";
			this.rbUnitsProj.Size = new System.Drawing.Size(156, 22);
			this.rbUnitsProj.TabIndex = 113;
			this.rbUnitsProj.TabStop = true;
			this.rbUnitsProj.Text = "Per Project Settings";
			this.rbUnitsProj.UseVisualStyleBackColor = true;
			this.rbUnitsProj.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// tbxMessage
			// 
			this.tbxMessage.BackColor = System.Drawing.SystemColors.Control;
			this.tbxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbxMessage.Location = new System.Drawing.Point(11, 319);
			this.tbxMessage.Margin = new System.Windows.Forms.Padding(0);
			this.tbxMessage.Multiline = true;
			this.tbxMessage.Name = "tbxMessage";
			this.tbxMessage.Size = new System.Drawing.Size(270, 51);
			this.tbxMessage.TabIndex = 116;
			this.tbxMessage.Text = "x\r\nx\r\nx";
			// 
			// rbUnitFtDecIn
			// 
			this.rbUnitFtDecIn.AutoSize = true;
			this.rbUnitFtDecIn.Location = new System.Drawing.Point(23, 290);
			this.rbUnitFtDecIn.Name = "rbUnitFtDecIn";
			this.rbUnitFtDecIn.Size = new System.Drawing.Size(191, 22);
			this.rbUnitFtDecIn.TabIndex = 118;
			this.rbUnitFtDecIn.Text = "Feet and Decimal Inches";
			this.rbUnitFtDecIn.UseVisualStyleBackColor = true;
			this.rbUnitFtDecIn.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// rbUnitFractFt
			// 
			this.rbUnitFractFt.AutoSize = true;
			this.rbUnitFractFt.Location = new System.Drawing.Point(255, 234);
			this.rbUnitFractFt.Name = "rbUnitFractFt";
			this.rbUnitFractFt.Size = new System.Drawing.Size(124, 22);
			this.rbUnitFractFt.TabIndex = 119;
			this.rbUnitFractFt.Text = "Fractional Feet";
			this.rbUnitFractFt.UseVisualStyleBackColor = true;
			this.rbUnitFractFt.Click += new System.EventHandler(this.rbUnit_Click);
			// 
			// FormDxMeasure
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 379);
			this.Controls.Add(this.rbUnitFractFt);
			this.Controls.Add(this.rbUnitFtDecIn);
			this.Controls.Add(lblBar3);
			this.Controls.Add(this.tbxMessage);
			this.Controls.Add(lblBar2);
			this.Controls.Add(lblBar1);
			this.Controls.Add(this.rbUnitsProj);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.rbUnitDecIn);
			this.Controls.Add(this.rbUnitDecFt);
			this.Controls.Add(this.rbUnitFracIn);
			this.Controls.Add(this.rbUnitFtFracIn);
			this.Controls.Add(this.lblXYZ);
			this.Controls.Add(this.lblYZ1);
			this.Controls.Add(this.lblXZ1);
			this.Controls.Add(this.lblXY1);
			this.Controls.Add(this.lblAxis);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbxWpOnOff);
			this.Controls.Add(this.lblDistXY);
			this.Controls.Add(this.lblDistYZ);
			this.Controls.Add(this.lblDistXYZ);
			this.Controls.Add(this.lblDistXZ);
			this.Controls.Add(this.lblDistZ);
			this.Controls.Add(this.lblDistY);
			this.Controls.Add(this.lblDistX);
			this.Controls.Add(this.lblAlongXaxis);
			this.Controls.Add(this.lblDistances);
			this.Controls.Add(this.lblP2Z);
			this.Controls.Add(this.lblP2Y);
			this.Controls.Add(this.lblP2X);
			this.Controls.Add(this.lblP1Z);
			this.Controls.Add(this.lblP1Y);
			this.Controls.Add(this.lblP1X);
			this.Controls.Add(this.lblPoint2);
			this.Controls.Add(this.lblZ1);
			this.Controls.Add(this.lblY1);
			this.Controls.Add(this.lblPoint1);
			this.Controls.Add(this.lblCoordinates);
			this.Controls.Add(this.lblX1);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.btnDone);
			this.Controls.Add(this.lblNote);
			this.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormDxMeasure";
			this.Text = "Delux Measure";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDxMeasure_FormClosing);
			this.Load += new System.EventHandler(this.FormDxMeasure_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox lblDistX;
		internal System.Windows.Forms.TextBox lblDistY;
		internal System.Windows.Forms.TextBox lblDistZ;
		internal System.Windows.Forms.TextBox lblDistXY;
		internal System.Windows.Forms.TextBox lblDistYZ;
		internal System.Windows.Forms.TextBox lblDistXYZ;
		internal System.Windows.Forms.TextBox lblDistXZ;
		
		
		private System.Windows.Forms.Label lblNote;
		private System.Windows.Forms.CheckBox cbxWpOnOff;
		private System.Windows.Forms.Label lblAlongXaxis;
		private System.Windows.Forms.Label lblDistances;
		internal System.Windows.Forms.Label lblP2Z;
		internal System.Windows.Forms.Label lblP2Y;
		internal System.Windows.Forms.Label lblP2X;
		internal System.Windows.Forms.Label lblP1Z;
		internal System.Windows.Forms.Label lblP1Y;
		internal System.Windows.Forms.Label lblP1X;
		private System.Windows.Forms.Label lblPoint2;
		private System.Windows.Forms.Label lblZ1;
		private System.Windows.Forms.Label lblY1;
		private System.Windows.Forms.Label lblPoint1;
		private System.Windows.Forms.Label lblCoordinates;
		private System.Windows.Forms.Label lblX1;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblAxis;
		private System.Windows.Forms.Label lblYZ1;
		private System.Windows.Forms.Label lblXZ1;
		private System.Windows.Forms.Label lblXY1;
		private System.Windows.Forms.Label lblXYZ;
		private System.Windows.Forms.RadioButton rbUnitFtFracIn;
		private System.Windows.Forms.RadioButton rbUnitFracIn;
		private System.Windows.Forms.RadioButton rbUnitDecFt;
		private System.Windows.Forms.RadioButton rbUnitDecIn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.RadioButton rbUnitsProj;
		public System.Windows.Forms.TextBox tbxMessage;
		private System.Windows.Forms.RadioButton rbUnitFtDecIn;
		private System.Windows.Forms.RadioButton rbUnitFractFt;
		
	}
}