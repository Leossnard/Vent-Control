namespace Vent_Control
{
	partial class FormMonitoreo
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
			this.buttonSubirBaseDatos = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelNumMediciones = new System.Windows.Forms.Label();
			this.checkBoxAutoScroll = new System.Windows.Forms.CheckBox();
			this.buttonAceptar = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dataGridViewMonitor = new System.Windows.Forms.DataGridView();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitor)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonSubirBaseDatos);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(800, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// buttonSubirBaseDatos
			// 
			this.buttonSubirBaseDatos.Location = new System.Drawing.Point(131, 12);
			this.buttonSubirBaseDatos.Name = "buttonSubirBaseDatos";
			this.buttonSubirBaseDatos.Size = new System.Drawing.Size(135, 23);
			this.buttonSubirBaseDatos.TabIndex = 1;
			this.buttonSubirBaseDatos.Text = "Subir a Base de Datos";
			this.buttonSubirBaseDatos.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(23, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(102, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Exportar a Excel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.labelNumMediciones);
			this.groupBox2.Controls.Add(this.checkBoxAutoScroll);
			this.groupBox2.Controls.Add(this.buttonAceptar);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(0, 383);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(800, 67);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			// 
			// labelNumMediciones
			// 
			this.labelNumMediciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelNumMediciones.AutoSize = true;
			this.labelNumMediciones.Location = new System.Drawing.Point(37, 29);
			this.labelNumMediciones.Name = "labelNumMediciones";
			this.labelNumMediciones.Size = new System.Drawing.Size(96, 13);
			this.labelNumMediciones.TabIndex = 2;
			this.labelNumMediciones.Text = "Medicion Numero: ";
			// 
			// checkBoxAutoScroll
			// 
			this.checkBoxAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxAutoScroll.AutoSize = true;
			this.checkBoxAutoScroll.Location = new System.Drawing.Point(570, 29);
			this.checkBoxAutoScroll.Name = "checkBoxAutoScroll";
			this.checkBoxAutoScroll.Size = new System.Drawing.Size(74, 17);
			this.checkBoxAutoScroll.TabIndex = 1;
			this.checkBoxAutoScroll.Text = "AutoScroll";
			this.checkBoxAutoScroll.UseVisualStyleBackColor = true;
			// 
			// buttonAceptar
			// 
			this.buttonAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAceptar.Location = new System.Drawing.Point(693, 25);
			this.buttonAceptar.Name = "buttonAceptar";
			this.buttonAceptar.Size = new System.Drawing.Size(75, 23);
			this.buttonAceptar.TabIndex = 0;
			this.buttonAceptar.Text = "Aceptar";
			this.buttonAceptar.UseVisualStyleBackColor = true;
			this.buttonAceptar.Click += new System.EventHandler(this.buttonAceptar_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.tabControl1);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(0, 40);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(800, 343);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(3, 16);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(794, 324);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dataGridViewMonitor);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(786, 298);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "COM";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// dataGridViewMonitor
			// 
			this.dataGridViewMonitor.AllowUserToAddRows = false;
			this.dataGridViewMonitor.AllowUserToDeleteRows = false;
			this.dataGridViewMonitor.AllowUserToResizeRows = false;
			this.dataGridViewMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewMonitor.Location = new System.Drawing.Point(3, 3);
			this.dataGridViewMonitor.Name = "dataGridViewMonitor";
			this.dataGridViewMonitor.ReadOnly = true;
			this.dataGridViewMonitor.RowHeadersVisible = false;
			this.dataGridViewMonitor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewMonitor.Size = new System.Drawing.Size(780, 292);
			this.dataGridViewMonitor.TabIndex = 0;
			// 
			// FormMonitoreo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "FormMonitoreo";
			this.Text = "FormMonitoreo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMonitoreo_FormClosing);
			this.Load += new System.EventHandler(this.FormMonitoreo_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonitor)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.DataGridView dataGridViewMonitor;
		private System.Windows.Forms.Button buttonSubirBaseDatos;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonAceptar;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.CheckBox checkBoxAutoScroll;
		private System.Windows.Forms.Label labelNumMediciones;
	}
}