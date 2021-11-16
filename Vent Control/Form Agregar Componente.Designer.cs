namespace Vent_Control
{
	partial class Form_Agregar_Componente
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.pictureBoxVistaPrevia = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioButtonVert = new System.Windows.Forms.RadioButton();
			this.radioButtonHor = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.ButtonAgregarComp = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelNombreSensor = new System.Windows.Forms.Label();
			this.textBoxNombreSensor = new System.Windows.Forms.TextBox();
			this.checkBoxContenedor = new System.Windows.Forms.CheckBox();
			this.comboBoxComponentes = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxVistaPrevia)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox4);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(274, 551);
			this.panel1.TabIndex = 2;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.pictureBoxVistaPrevia);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox4.Location = new System.Drawing.Point(0, 202);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(274, 273);
			this.groupBox4.TabIndex = 4;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Vista Previa";
			// 
			// pictureBoxVistaPrevia
			// 
			this.pictureBoxVistaPrevia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxVistaPrevia.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxVistaPrevia.Location = new System.Drawing.Point(3, 16);
			this.pictureBoxVistaPrevia.Name = "pictureBoxVistaPrevia";
			this.pictureBoxVistaPrevia.Size = new System.Drawing.Size(268, 254);
			this.pictureBoxVistaPrevia.TabIndex = 0;
			this.pictureBoxVistaPrevia.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioButtonVert);
			this.groupBox2.Controls.Add(this.radioButtonHor);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(274, 58);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Rotar";
			// 
			// radioButtonVert
			// 
			this.radioButtonVert.AutoSize = true;
			this.radioButtonVert.Location = new System.Drawing.Point(165, 19);
			this.radioButtonVert.Name = "radioButtonVert";
			this.radioButtonVert.Size = new System.Drawing.Size(60, 17);
			this.radioButtonVert.TabIndex = 1;
			this.radioButtonVert.TabStop = true;
			this.radioButtonVert.Text = "Vertical";
			this.radioButtonVert.UseVisualStyleBackColor = true;
			this.radioButtonVert.CheckedChanged += new System.EventHandler(this.radioButtonVert_CheckedChanged);
			// 
			// radioButtonHor
			// 
			this.radioButtonHor.AutoSize = true;
			this.radioButtonHor.Location = new System.Drawing.Point(41, 19);
			this.radioButtonHor.Name = "radioButtonHor";
			this.radioButtonHor.Size = new System.Drawing.Size(72, 17);
			this.radioButtonHor.TabIndex = 0;
			this.radioButtonHor.TabStop = true;
			this.radioButtonHor.Text = "Horizontal";
			this.radioButtonHor.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.ButtonAgregarComp);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox3.Location = new System.Drawing.Point(0, 475);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(274, 76);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			// 
			// ButtonAgregarComp
			// 
			this.ButtonAgregarComp.Location = new System.Drawing.Point(69, 28);
			this.ButtonAgregarComp.Name = "ButtonAgregarComp";
			this.ButtonAgregarComp.Size = new System.Drawing.Size(136, 23);
			this.ButtonAgregarComp.TabIndex = 4;
			this.ButtonAgregarComp.Text = "Agregar Componente";
			this.ButtonAgregarComp.UseVisualStyleBackColor = true;
			this.ButtonAgregarComp.Click += new System.EventHandler(this.ButtonAgregarComp_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelNombreSensor);
			this.groupBox1.Controls.Add(this.textBoxNombreSensor);
			this.groupBox1.Controls.Add(this.checkBoxContenedor);
			this.groupBox1.Controls.Add(this.comboBoxComponentes);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(274, 144);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// labelNombreSensor
			// 
			this.labelNombreSensor.AutoSize = true;
			this.labelNombreSensor.Location = new System.Drawing.Point(47, 93);
			this.labelNombreSensor.Name = "labelNombreSensor";
			this.labelNombreSensor.Size = new System.Drawing.Size(83, 13);
			this.labelNombreSensor.TabIndex = 7;
			this.labelNombreSensor.Text = "Nombre Sensor:";
			// 
			// textBoxNombreSensor
			// 
			this.textBoxNombreSensor.Location = new System.Drawing.Point(50, 109);
			this.textBoxNombreSensor.Name = "textBoxNombreSensor";
			this.textBoxNombreSensor.Size = new System.Drawing.Size(175, 20);
			this.textBoxNombreSensor.TabIndex = 6;
			// 
			// checkBoxContenedor
			// 
			this.checkBoxContenedor.AutoSize = true;
			this.checkBoxContenedor.Location = new System.Drawing.Point(50, 59);
			this.checkBoxContenedor.Name = "checkBoxContenedor";
			this.checkBoxContenedor.Size = new System.Drawing.Size(172, 17);
			this.checkBoxContenedor.TabIndex = 5;
			this.checkBoxContenedor.Text = "Galeria Contiene Componentes";
			this.checkBoxContenedor.UseVisualStyleBackColor = true;
			// 
			// comboBoxComponentes
			// 
			this.comboBoxComponentes.FormattingEnabled = true;
			this.comboBoxComponentes.Location = new System.Drawing.Point(50, 32);
			this.comboBoxComponentes.Name = "comboBoxComponentes";
			this.comboBoxComponentes.Size = new System.Drawing.Size(175, 21);
			this.comboBoxComponentes.TabIndex = 1;
			this.comboBoxComponentes.SelectedValueChanged += new System.EventHandler(this.comboBoxComponentes_SelectedValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(47, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Seleccionar Componente a Agregar:";
			// 
			// Form_Agregar_Componente
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(299, 575);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.Name = "Form_Agregar_Componente";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form_Agregar_Componente";
			this.Load += new System.EventHandler(this.Form_Agregar_Componente_Load);
			this.panel1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxVistaPrevia)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboBoxComponentes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button ButtonAgregarComp;
		private System.Windows.Forms.PictureBox pictureBoxVistaPrevia;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radioButtonVert;
		private System.Windows.Forms.RadioButton radioButtonHor;
		private System.Windows.Forms.CheckBox checkBoxContenedor;
		private System.Windows.Forms.Label labelNombreSensor;
		private System.Windows.Forms.TextBox textBoxNombreSensor;
	}
}