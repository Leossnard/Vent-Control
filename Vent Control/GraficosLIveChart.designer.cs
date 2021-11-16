namespace Vent_Control
{
    partial class Graficos
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.dgDatosSectores = new System.Windows.Forms.DataGridView();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.btnIniciarCaptura = new System.Windows.Forms.Button();
            this.btnVaciarDataGrid = new System.Windows.Forms.Button();
            this.cbSectores = new System.Windows.Forms.ComboBox();
            this.tmAgregarDatos = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tstrpTiempoVisualizacion = new System.Windows.Forms.ToolStripButton();
            this.tsRefrescarGraficos = new System.Windows.Forms.ToolStripButton();
            this.tsVentsim = new System.Windows.Forms.ToolStripButton();
            this.tsExcel = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gpTablaDatos = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Real = new System.Windows.Forms.TabPage();
            this.Estático = new System.Windows.Forms.TabPage();
            this.cGlobal2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cGlobal3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cGlobal4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cGlobal1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tmrGlobales = new System.Windows.Forms.Timer(this.components);
            this.cartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            ((System.ComponentModel.ISupportInitialize)(this.dgDatosSectores)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.gpTablaDatos.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Real.SuspendLayout();
            this.Estático.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgDatosSectores
            // 
            this.dgDatosSectores.AllowUserToAddRows = false;
            this.dgDatosSectores.AllowUserToDeleteRows = false;
            this.dgDatosSectores.AllowUserToResizeColumns = false;
            this.dgDatosSectores.AllowUserToResizeRows = false;
            this.dgDatosSectores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDatosSectores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgDatosSectores.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgDatosSectores.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDatosSectores.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDatosSectores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDatosSectores.Location = new System.Drawing.Point(12, 30);
            this.dgDatosSectores.MinimumSize = new System.Drawing.Size(360, 501);
            this.dgDatosSectores.Name = "dgDatosSectores";
            this.dgDatosSectores.ReadOnly = true;
            this.dgDatosSectores.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgDatosSectores.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDatosSectores.Size = new System.Drawing.Size(417, 632);
            this.dgDatosSectores.TabIndex = 0;
            this.dgDatosSectores.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgDatosSectores_ColumnAdded);
            this.dgDatosSectores.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgDatosSectores_ColumnHeaderMouseDoubleClick);
            this.dgDatosSectores.MouseEnter += new System.EventHandler(this.dgDatosSectores_MouseEnter);
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cartesianChart1.Location = new System.Drawing.Point(6, 8);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1349, 705);
            this.cartesianChart1.TabIndex = 6;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSeleccionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionar.Location = new System.Drawing.Point(40, 706);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(129, 31);
            this.btnSeleccionar.TabIndex = 4;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // btnIniciarCaptura
            // 
            this.btnIniciarCaptura.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciarCaptura.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarCaptura.Location = new System.Drawing.Point(267, 668);
            this.btnIniciarCaptura.Name = "btnIniciarCaptura";
            this.btnIniciarCaptura.Size = new System.Drawing.Size(113, 31);
            this.btnIniciarCaptura.TabIndex = 0;
            this.btnIniciarCaptura.Text = "Iniciar";
            this.btnIniciarCaptura.UseVisualStyleBackColor = true;
            this.btnIniciarCaptura.Click += new System.EventHandler(this.btnIniciarCaptura_Click);
            // 
            // btnVaciarDataGrid
            // 
            this.btnVaciarDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVaciarDataGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVaciarDataGrid.Location = new System.Drawing.Point(267, 706);
            this.btnVaciarDataGrid.Name = "btnVaciarDataGrid";
            this.btnVaciarDataGrid.Size = new System.Drawing.Size(113, 31);
            this.btnVaciarDataGrid.TabIndex = 2;
            this.btnVaciarDataGrid.Text = "Liberar";
            this.btnVaciarDataGrid.UseVisualStyleBackColor = true;
            this.btnVaciarDataGrid.Click += new System.EventHandler(this.btnVaciarDataGrid_Click);
            // 
            // cbSectores
            // 
            this.cbSectores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSectores.BackColor = System.Drawing.Color.SeaShell;
            this.cbSectores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSectores.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSectores.FormattingEnabled = true;
            this.cbSectores.Location = new System.Drawing.Point(28, 668);
            this.cbSectores.Name = "cbSectores";
            this.cbSectores.Size = new System.Drawing.Size(141, 24);
            this.cbSectores.TabIndex = 3;
            this.cbSectores.SelectedIndexChanged += new System.EventHandler(this.cbSectores_SelectedIndexChanged);
            // 
            // tmAgregarDatos
            // 
            this.tmAgregarDatos.Tick += new System.EventHandler(this.tmAgregarDatos_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton1,
            this.tstrpTiempoVisualizacion,
            this.tsRefrescarGraficos,
            this.tsVentsim,
            this.tsExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1840, 27);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Vent_Control.Properties.Resources.grafcar_imagen;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton2.Tag = "";
            this.toolStripButton2.Text = "Presione para graficar la columna de interés";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Vent_Control.Properties.Resources.EliminarGrafico;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "Presione para eliminar el gráfico de la columna de interés";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tstrpTiempoVisualizacion
            // 
            this.tstrpTiempoVisualizacion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tstrpTiempoVisualizacion.Image = global::Vent_Control.Properties.Resources.timerVisualizacion2;
            this.tstrpTiempoVisualizacion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tstrpTiempoVisualizacion.Name = "tstrpTiempoVisualizacion";
            this.tstrpTiempoVisualizacion.Size = new System.Drawing.Size(29, 24);
            this.tstrpTiempoVisualizacion.Text = "Presione para definir el tiempo de visualización";
            this.tstrpTiempoVisualizacion.Click += new System.EventHandler(this.tstrpTiempoVisualizacion_Click);
            // 
            // tsRefrescarGraficos
            // 
            this.tsRefrescarGraficos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRefrescarGraficos.Image = global::Vent_Control.Properties.Resources.refrescar3;
            this.tsRefrescarGraficos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRefrescarGraficos.Name = "tsRefrescarGraficos";
            this.tsRefrescarGraficos.Size = new System.Drawing.Size(29, 24);
            this.tsRefrescarGraficos.Text = "Presione para actualizar gráficos globales";
            this.tsRefrescarGraficos.Click += new System.EventHandler(this.tsRefrescarGraficos_Click);
            // 
            // tsVentsim
            // 
            this.tsVentsim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsVentsim.Image = global::Vent_Control.Properties.Resources.icono_herramienta;
            this.tsVentsim.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsVentsim.Name = "tsVentsim";
            this.tsVentsim.Size = new System.Drawing.Size(29, 24);
            this.tsVentsim.Text = "Presione para abrir el modelo .vsm del circuito";
            this.tsVentsim.Click += new System.EventHandler(this.tsVentsim_Click);
            // 
            // tsExcel
            // 
            this.tsExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsExcel.Image = global::Vent_Control.Properties.Resources.excel2;
            this.tsExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsExcel.Name = "tsExcel";
            this.tsExcel.Size = new System.Drawing.Size(29, 24);
            this.tsExcel.Text = "Presione para exportar datos a Excel";
            this.tsExcel.Click += new System.EventHandler(this.tsExcel_Click);
            // 
            // gpTablaDatos
            // 
            this.gpTablaDatos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpTablaDatos.Controls.Add(this.dgDatosSectores);
            this.gpTablaDatos.Controls.Add(this.btnSeleccionar);
            this.gpTablaDatos.Controls.Add(this.cbSectores);
            this.gpTablaDatos.Controls.Add(this.btnVaciarDataGrid);
            this.gpTablaDatos.Controls.Add(this.btnIniciarCaptura);
            this.gpTablaDatos.Location = new System.Drawing.Point(1397, 68);
            this.gpTablaDatos.Name = "gpTablaDatos";
            this.gpTablaDatos.Size = new System.Drawing.Size(438, 743);
            this.gpTablaDatos.TabIndex = 10;
            this.gpTablaDatos.TabStop = false;
            this.gpTablaDatos.Text = "Tabla de Datos";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Real);
            this.tabControl1.Controls.Add(this.Estático);
            this.tabControl1.Location = new System.Drawing.Point(12, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1369, 758);
            this.tabControl1.TabIndex = 11;
            this.tabControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyDown);
            // 
            // Real
            // 
            this.Real.Controls.Add(this.cartesianChart1);
            this.Real.Location = new System.Drawing.Point(4, 25);
            this.Real.Name = "Real";
            this.Real.Padding = new System.Windows.Forms.Padding(3);
            this.Real.Size = new System.Drawing.Size(1361, 729);
            this.Real.TabIndex = 0;
            this.Real.Text = "Gráficos en Tiempo Real";
            this.Real.UseVisualStyleBackColor = true;
            // 
            // Estático
            // 
            this.Estático.Controls.Add(this.cGlobal2);
            this.Estático.Controls.Add(this.cGlobal3);
            this.Estático.Controls.Add(this.cGlobal4);
            this.Estático.Controls.Add(this.cGlobal1);
            this.Estático.Location = new System.Drawing.Point(4, 25);
            this.Estático.Name = "Estático";
            this.Estático.Padding = new System.Windows.Forms.Padding(3);
            this.Estático.Size = new System.Drawing.Size(1361, 729);
            this.Estático.TabIndex = 1;
            this.Estático.Text = "Gráficos Globales";
            this.Estático.UseVisualStyleBackColor = true;
            // 
            // cGlobal2
            // 
            chartArea1.Name = "ChartArea1";
            this.cGlobal2.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "LegendO2";
            this.cGlobal2.Legends.Add(legend1);
            this.cGlobal2.Location = new System.Drawing.Point(658, 8);
            this.cGlobal2.Name = "cGlobal2";
            this.cGlobal2.Size = new System.Drawing.Size(693, 349);
            this.cGlobal2.TabIndex = 15;
            this.cGlobal2.Text = "GraficoGlobal2";
            this.cGlobal2.DoubleClick += new System.EventHandler(this.cGlobal2_DoubleClick);
            // 
            // cGlobal3
            // 
            chartArea2.Name = "ChartArea1";
            this.cGlobal3.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "LegendCO2";
            this.cGlobal3.Legends.Add(legend2);
            this.cGlobal3.Location = new System.Drawing.Point(3, 363);
            this.cGlobal3.Name = "cGlobal3";
            this.cGlobal3.Size = new System.Drawing.Size(643, 360);
            this.cGlobal3.TabIndex = 14;
            this.cGlobal3.Text = "GraficoGlobal3";
            this.cGlobal3.DoubleClick += new System.EventHandler(this.cGlobal3_DoubleClick);
            // 
            // cGlobal4
            // 
            chartArea3.Name = "ChartArea1";
            this.cGlobal4.ChartAreas.Add(chartArea3);
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend3.Name = "Legend1";
            this.cGlobal4.Legends.Add(legend3);
            this.cGlobal4.Location = new System.Drawing.Point(658, 363);
            this.cGlobal4.Name = "cGlobal4";
            this.cGlobal4.Size = new System.Drawing.Size(703, 360);
            this.cGlobal4.TabIndex = 13;
            this.cGlobal4.Text = "GraficoGlobal4";
            this.cGlobal4.DoubleClick += new System.EventHandler(this.cGlobal4_DoubleClick);
            // 
            // cGlobal1
            // 
            chartArea4.Name = "ChartArea1";
            this.cGlobal1.ChartAreas.Add(chartArea4);
            legend4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend4.Name = "Legend1";
            this.cGlobal1.Legends.Add(legend4);
            this.cGlobal1.Location = new System.Drawing.Point(0, 6);
            this.cGlobal1.Name = "cGlobal1";
            this.cGlobal1.Size = new System.Drawing.Size(640, 351);
            this.cGlobal1.TabIndex = 10;
            this.cGlobal1.Text = "GraficoGlobal1";
            this.cGlobal1.DoubleClick += new System.EventHandler(this.cGlobal1_DoubleClick);
            // 
            // tmrGlobales
            // 
            this.tmrGlobales.Tick += new System.EventHandler(this.tmrGlobales_Tick);
            // 
            // cartesianChart2
            // 
            this.cartesianChart2.Location = new System.Drawing.Point(493, 0);
            this.cartesianChart2.Name = "cartesianChart2";
            this.cartesianChart2.Size = new System.Drawing.Size(8, 8);
            this.cartesianChart2.TabIndex = 12;
            this.cartesianChart2.Text = "cartesianChart2";
            // 
            // Graficos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1840, 822);
            this.Controls.Add(this.cartesianChart2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gpTablaDatos);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(1858, 869);
            this.Name = "Graficos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gráficos";
            this.SizeChanged += new System.EventHandler(this.Graficos_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgDatosSectores)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gpTablaDatos.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Real.ResumeLayout(false);
            this.Estático.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cGlobal1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgDatosSectores;
        private System.Windows.Forms.Button btnVaciarDataGrid;
        private System.Windows.Forms.Button btnIniciarCaptura;
        private System.Windows.Forms.ComboBox cbSectores;
        private System.Windows.Forms.Timer tmAgregarDatos;
        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.GroupBox gpTablaDatos;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Real;
        private System.Windows.Forms.TabPage Estático;
        private System.Windows.Forms.Timer tmrGlobales;
        private System.Windows.Forms.ToolStripButton tstrpTiempoVisualizacion;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart cGlobal1;
        private System.Windows.Forms.DataVisualization.Charting.Chart cGlobal4;
        private System.Windows.Forms.DataVisualization.Charting.Chart cGlobal2;
        private System.Windows.Forms.DataVisualization.Charting.Chart cGlobal3;
        private System.Windows.Forms.ToolStripButton tsRefrescarGraficos;
        private System.Windows.Forms.ToolStripButton tsVentsim;
        private LiveCharts.WinForms.CartesianChart cartesianChart2;
        public LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.ToolStripButton tsExcel;
    }
}