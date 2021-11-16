
namespace Vent_Control
{
	partial class ModBusConfig
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
			this.buttonAceptarMOD = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxEsclavo = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBoxPuerto = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonEscribir = new System.Windows.Forms.Button();
			this.textBoxDireccion = new System.Windows.Forms.TextBox();
			this.textBoxValor = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dataGridResumenMOD = new System.Windows.Forms.DataGridView();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxDirre2 = new System.Windows.Forms.TextBox();
			this.textBoxValor2 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.Actuador = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Puerto_COM = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ID_Esclavo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Direccion1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Valor_Actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Direccion2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Valor_estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridResumenMOD)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonAceptarMOD
			// 
			this.buttonAceptarMOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAceptarMOD.Location = new System.Drawing.Point(893, 19);
			this.buttonAceptarMOD.Name = "buttonAceptarMOD";
			this.buttonAceptarMOD.Size = new System.Drawing.Size(96, 32);
			this.buttonAceptarMOD.TabIndex = 0;
			this.buttonAceptarMOD.Text = "Aceptar";
			this.buttonAceptarMOD.UseVisualStyleBackColor = true;
			this.buttonAceptarMOD.Click += new System.EventHandler(this.buttonAceptarMOD_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.buttonAceptarMOD);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox3.Location = new System.Drawing.Point(0, 378);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(1001, 68);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Puerto COM";
			// 
			// textBoxEsclavo
			// 
			this.textBoxEsclavo.Location = new System.Drawing.Point(150, 46);
			this.textBoxEsclavo.Name = "textBoxEsclavo";
			this.textBoxEsclavo.Size = new System.Drawing.Size(121, 20);
			this.textBoxEsclavo.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(147, 29);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "ID Esclavo";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textBoxEsclavo);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.comboBoxPuerto);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(3, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(279, 91);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ModBus RTU";
			// 
			// comboBoxPuerto
			// 
			this.comboBoxPuerto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPuerto.FormattingEnabled = true;
			this.comboBoxPuerto.Location = new System.Drawing.Point(13, 45);
			this.comboBoxPuerto.Name = "comboBoxPuerto";
			this.comboBoxPuerto.Size = new System.Drawing.Size(121, 21);
			this.comboBoxPuerto.TabIndex = 0;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.groupBox4);
			this.groupBox5.Controls.Add(this.groupBox1);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox5.Location = new System.Drawing.Point(0, 0);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(285, 378);
			this.groupBox5.TabIndex = 12;
			this.groupBox5.TabStop = false;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.textBoxDirre2);
			this.groupBox4.Controls.Add(this.textBoxValor2);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.buttonEscribir);
			this.groupBox4.Controls.Add(this.textBoxDireccion);
			this.groupBox4.Controls.Add(this.textBoxValor);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox4.Location = new System.Drawing.Point(3, 107);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(279, 268);
			this.groupBox4.TabIndex = 10;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Write Single Register";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Direccion Encendido";
			// 
			// buttonEscribir
			// 
			this.buttonEscribir.Location = new System.Drawing.Point(98, 147);
			this.buttonEscribir.Name = "buttonEscribir";
			this.buttonEscribir.Size = new System.Drawing.Size(108, 70);
			this.buttonEscribir.TabIndex = 4;
			this.buttonEscribir.Text = "Actualizar";
			this.buttonEscribir.UseVisualStyleBackColor = true;
			this.buttonEscribir.Click += new System.EventHandler(this.buttonEscribir_Click);
			// 
			// textBoxDireccion
			// 
			this.textBoxDireccion.Location = new System.Drawing.Point(10, 46);
			this.textBoxDireccion.Name = "textBoxDireccion";
			this.textBoxDireccion.Size = new System.Drawing.Size(121, 20);
			this.textBoxDireccion.TabIndex = 6;
			// 
			// textBoxValor
			// 
			this.textBoxValor.Location = new System.Drawing.Point(10, 96);
			this.textBoxValor.Name = "textBoxValor";
			this.textBoxValor.Size = new System.Drawing.Size(121, 20);
			this.textBoxValor.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Valor";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dataGridResumenMOD);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(285, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(716, 378);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Resumen Puertos Modbus";
			// 
			// dataGridResumenMOD
			// 
			this.dataGridResumenMOD.AllowUserToAddRows = false;
			this.dataGridResumenMOD.AllowUserToDeleteRows = false;
			this.dataGridResumenMOD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridResumenMOD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Actuador,
            this.Puerto_COM,
            this.ID_Esclavo,
            this.Direccion1,
            this.Valor_Actual,
            this.Direccion2,
            this.Valor_estado});
			this.dataGridResumenMOD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridResumenMOD.Location = new System.Drawing.Point(3, 16);
			this.dataGridResumenMOD.MultiSelect = false;
			this.dataGridResumenMOD.Name = "dataGridResumenMOD";
			this.dataGridResumenMOD.ReadOnly = true;
			this.dataGridResumenMOD.RowHeadersVisible = false;
			this.dataGridResumenMOD.Size = new System.Drawing.Size(710, 359);
			this.dataGridResumenMOD.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(147, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Direccion Estado";
			// 
			// textBoxDirre2
			// 
			this.textBoxDirre2.Location = new System.Drawing.Point(147, 46);
			this.textBoxDirre2.Name = "textBoxDirre2";
			this.textBoxDirre2.Size = new System.Drawing.Size(121, 20);
			this.textBoxDirre2.TabIndex = 10;
			// 
			// textBoxValor2
			// 
			this.textBoxValor2.Location = new System.Drawing.Point(147, 96);
			this.textBoxValor2.Name = "textBoxValor2";
			this.textBoxValor2.Size = new System.Drawing.Size(121, 20);
			this.textBoxValor2.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(147, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(31, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "Valor";
			// 
			// Actuador
			// 
			this.Actuador.HeaderText = "Actuador";
			this.Actuador.Name = "Actuador";
			this.Actuador.ReadOnly = true;
			// 
			// Puerto_COM
			// 
			this.Puerto_COM.HeaderText = "Puerto COM";
			this.Puerto_COM.Name = "Puerto_COM";
			this.Puerto_COM.ReadOnly = true;
			// 
			// ID_Esclavo
			// 
			this.ID_Esclavo.HeaderText = "ID_Esclavo";
			this.ID_Esclavo.Name = "ID_Esclavo";
			this.ID_Esclavo.ReadOnly = true;
			// 
			// Direccion1
			// 
			this.Direccion1.HeaderText = "Direccion Power";
			this.Direccion1.Name = "Direccion1";
			this.Direccion1.ReadOnly = true;
			// 
			// Valor_Actual
			// 
			this.Valor_Actual.HeaderText = "Valor Power";
			this.Valor_Actual.Name = "Valor_Actual";
			this.Valor_Actual.ReadOnly = true;
			// 
			// Direccion2
			// 
			this.Direccion2.HeaderText = "Direccion Estado";
			this.Direccion2.Name = "Direccion2";
			this.Direccion2.ReadOnly = true;
			// 
			// Valor_estado
			// 
			this.Valor_estado.HeaderText = "Valor Estado";
			this.Valor_estado.Name = "Valor_estado";
			this.Valor_estado.ReadOnly = true;
			// 
			// ModBusConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1001, 446);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox3);
			this.Name = "ModBusConfig";
			this.Text = "ModBusConfig";
			this.Load += new System.EventHandler(this.ModBusConfig_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridResumenMOD)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonAceptarMOD;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxEsclavo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonEscribir;
		private System.Windows.Forms.TextBox textBoxDireccion;
		private System.Windows.Forms.TextBox textBoxValor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGridView dataGridResumenMOD;
		private System.Windows.Forms.ComboBox comboBoxPuerto;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxDirre2;
		private System.Windows.Forms.TextBox textBoxValor2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DataGridViewTextBoxColumn Actuador;
		private System.Windows.Forms.DataGridViewTextBoxColumn Puerto_COM;
		private System.Windows.Forms.DataGridViewTextBoxColumn ID_Esclavo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Direccion1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor_Actual;
		private System.Windows.Forms.DataGridViewTextBoxColumn Direccion2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor_estado;
	}
}