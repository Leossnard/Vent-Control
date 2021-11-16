namespace Vent_Control
{
	partial class FormValidaciones
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonAceptarVal = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.dataGridViewOptimizacion = new System.Windows.Forms.DataGridView();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dataGridViewMonitoreo = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Minimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Max = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AumentarMensaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DisminuirMensaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.buttonAplicar = new System.Windows.Forms.Button();
			this.NombreSalida = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewOptimizacion)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitoreo)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonAplicar);
			this.groupBox1.Controls.Add(this.buttonAceptarVal);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Location = new System.Drawing.Point(0, 379);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(963, 71);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// buttonAceptarVal
			// 
			this.buttonAceptarVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAceptarVal.Location = new System.Drawing.Point(859, 36);
			this.buttonAceptarVal.Name = "buttonAceptarVal";
			this.buttonAceptarVal.Size = new System.Drawing.Size(75, 23);
			this.buttonAceptarVal.TabIndex = 0;
			this.buttonAceptarVal.Text = "Aceptar";
			this.buttonAceptarVal.UseVisualStyleBackColor = true;
			this.buttonAceptarVal.Click += new System.EventHandler(this.buttonAceptarVal_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.dataGridViewOptimizacion);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(352, 379);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Optimizar Salidas";
			// 
			// dataGridViewOptimizacion
			// 
			this.dataGridViewOptimizacion.AllowUserToAddRows = false;
			this.dataGridViewOptimizacion.AllowUserToDeleteRows = false;
			this.dataGridViewOptimizacion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dataGridViewOptimizacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewOptimizacion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NombreSalida});
			this.dataGridViewOptimizacion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewOptimizacion.Location = new System.Drawing.Point(3, 16);
			this.dataGridViewOptimizacion.MultiSelect = false;
			this.dataGridViewOptimizacion.Name = "dataGridViewOptimizacion";
			this.dataGridViewOptimizacion.RowHeadersVisible = false;
			this.dataGridViewOptimizacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridViewOptimizacion.Size = new System.Drawing.Size(346, 360);
			this.dataGridViewOptimizacion.TabIndex = 4;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dataGridViewMonitoreo);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(352, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(611, 379);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Validaciones";
			// 
			// dataGridViewMonitoreo
			// 
			this.dataGridViewMonitoreo.AllowUserToAddRows = false;
			this.dataGridViewMonitoreo.AllowUserToDeleteRows = false;
			this.dataGridViewMonitoreo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dataGridViewMonitoreo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewMonitoreo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Minimo,
            this.Max,
            this.Actual,
            this.AumentarMensaje,
            this.DisminuirMensaje});
			this.dataGridViewMonitoreo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewMonitoreo.Location = new System.Drawing.Point(3, 16);
			this.dataGridViewMonitoreo.MultiSelect = false;
			this.dataGridViewMonitoreo.Name = "dataGridViewMonitoreo";
			this.dataGridViewMonitoreo.ReadOnly = true;
			this.dataGridViewMonitoreo.RowHeadersVisible = false;
			this.dataGridViewMonitoreo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewMonitoreo.Size = new System.Drawing.Size(605, 360);
			this.dataGridViewMonitoreo.TabIndex = 3;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Nombre Medicion";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 105;
			// 
			// Minimo
			// 
			this.Minimo.HeaderText = "Min";
			this.Minimo.Name = "Minimo";
			this.Minimo.ReadOnly = true;
			this.Minimo.Width = 49;
			// 
			// Max
			// 
			this.Max.HeaderText = "Max";
			this.Max.Name = "Max";
			this.Max.ReadOnly = true;
			this.Max.Width = 52;
			// 
			// Actual
			// 
			this.Actual.HeaderText = "Actual";
			this.Actual.Name = "Actual";
			this.Actual.ReadOnly = true;
			this.Actual.Width = 62;
			// 
			// AumentarMensaje
			// 
			this.AumentarMensaje.HeaderText = "Aumentar Mensaje";
			this.AumentarMensaje.Name = "AumentarMensaje";
			this.AumentarMensaje.ReadOnly = true;
			this.AumentarMensaje.Width = 110;
			// 
			// DisminuirMensaje
			// 
			this.DisminuirMensaje.HeaderText = "Disminuir Mensaje";
			this.DisminuirMensaje.Name = "DisminuirMensaje";
			this.DisminuirMensaje.ReadOnly = true;
			this.DisminuirMensaje.Width = 107;
			// 
			// buttonAplicar
			// 
			this.buttonAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAplicar.Location = new System.Drawing.Point(778, 36);
			this.buttonAplicar.Name = "buttonAplicar";
			this.buttonAplicar.Size = new System.Drawing.Size(75, 23);
			this.buttonAplicar.TabIndex = 1;
			this.buttonAplicar.Text = "Aplicar";
			this.buttonAplicar.UseVisualStyleBackColor = true;
			this.buttonAplicar.Click += new System.EventHandler(this.buttonAplicar_Click);
			// 
			// NombreSalida
			// 
			this.NombreSalida.HeaderText = "Nombre Salida";
			this.NombreSalida.Name = "NombreSalida";
			this.NombreSalida.Width = 101;
			// 
			// FormValidaciones
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(963, 450);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Name = "FormValidaciones";
			this.Text = "FormValidaciones";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormValidaciones_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewOptimizacion)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitoreo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonAceptarVal;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.DataGridView dataGridViewOptimizacion;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGridView dataGridViewMonitoreo;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Minimo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Max;
		private System.Windows.Forms.DataGridViewTextBoxColumn Actual;
		private System.Windows.Forms.DataGridViewTextBoxColumn AumentarMensaje;
		private System.Windows.Forms.DataGridViewTextBoxColumn DisminuirMensaje;
		private System.Windows.Forms.Button buttonAplicar;
		private System.Windows.Forms.DataGridViewTextBoxColumn NombreSalida;
	}
}