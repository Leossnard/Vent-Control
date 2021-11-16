namespace Vent_Control
{
	partial class Form1
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.guardarcomoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verGraficosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.acercadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripNuevo = new System.Windows.Forms.ToolStripButton();
			this.toolStripAbrir = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripGuardar = new System.Windows.Forms.ToolStripButton();
			this.toolStripGuardarComo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripEnviarFondo = new System.Windows.Forms.ToolStripButton();
			this.toolStripLlevarFrente = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripBorrar = new System.Windows.Forms.ToolStripButton();
			this.toolStripDibujo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.labelMensaje = new System.Windows.Forms.Label();
			this.labelArchivo = new System.Windows.Forms.Label();
			this.labelModo = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonValidaciones = new System.Windows.Forms.Button();
			this.buttonModoManual = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonMonitoreo = new System.Windows.Forms.Button();
			this.buttonAgregarComp = new System.Windows.Forms.Button();
			this.buttonConfig = new System.Windows.Forms.Button();
			this.buttonComenzar = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.ayudaToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// archivoToolStripMenuItem
			// 
			this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.toolStripSeparator,
            this.guardarToolStripMenuItem,
            this.guardarcomoToolStripMenuItem,
            this.toolStripSeparator1,
            this.salirToolStripMenuItem});
			this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
			this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.archivoToolStripMenuItem.Text = "&Archivo";
			// 
			// nuevoToolStripMenuItem
			// 
			this.nuevoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nuevoToolStripMenuItem.Image")));
			this.nuevoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
			this.nuevoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.nuevoToolStripMenuItem.Text = "&Nuevo";
			this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(153, 6);
			// 
			// guardarToolStripMenuItem
			// 
			this.guardarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("guardarToolStripMenuItem.Image")));
			this.guardarToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
			this.guardarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.guardarToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.guardarToolStripMenuItem.Text = "&Guardar";
			this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
			// 
			// guardarcomoToolStripMenuItem
			// 
			this.guardarcomoToolStripMenuItem.Name = "guardarcomoToolStripMenuItem";
			this.guardarcomoToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.guardarcomoToolStripMenuItem.Text = "G&uardar como";
			this.guardarcomoToolStripMenuItem.Click += new System.EventHandler(this.guardarcomoToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
			// 
			// salirToolStripMenuItem
			// 
			this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
			this.salirToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.salirToolStripMenuItem.Text = "&Salir";
			this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
			// 
			// herramientasToolStripMenuItem
			// 
			this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opcionesToolStripMenuItem,
            this.verGraficosToolStripMenuItem});
			this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
			this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
			this.herramientasToolStripMenuItem.Text = "&Herramientas";
			// 
			// opcionesToolStripMenuItem
			// 
			this.opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
			this.opcionesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.opcionesToolStripMenuItem.Text = "&Opciones";
			// 
			// verGraficosToolStripMenuItem
			// 
			this.verGraficosToolStripMenuItem.Name = "verGraficosToolStripMenuItem";
			this.verGraficosToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.verGraficosToolStripMenuItem.Text = "Ver Graficos";
			this.verGraficosToolStripMenuItem.Click += new System.EventHandler(this.verGraficosToolStripMenuItem_Click);
			// 
			// ayudaToolStripMenuItem
			// 
			this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercadeToolStripMenuItem});
			this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
			this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.ayudaToolStripMenuItem.Text = "Ay&uda";
			// 
			// acercadeToolStripMenuItem
			// 
			this.acercadeToolStripMenuItem.Name = "acercadeToolStripMenuItem";
			this.acercadeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.acercadeToolStripMenuItem.Text = "&Acerca de...";
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNuevo,
            this.toolStripAbrir,
            this.toolStripSeparator6,
            this.toolStripGuardar,
            this.toolStripGuardarComo,
            this.toolStripSeparator5,
            this.toolStripEnviarFondo,
            this.toolStripLlevarFrente,
            this.toolStripSeparator7,
            this.toolStripBorrar,
            this.toolStripDibujo,
            this.toolStripSeparator3});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1350, 25);
			this.toolStrip1.TabIndex = 15;
			this.toolStrip1.Text = "Barra de Herramientas";
			// 
			// toolStripNuevo
			// 
			this.toolStripNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripNuevo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripNuevo.Image")));
			this.toolStripNuevo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripNuevo.Name = "toolStripNuevo";
			this.toolStripNuevo.Size = new System.Drawing.Size(23, 22);
			this.toolStripNuevo.Text = "Nuevo Archivo";
			this.toolStripNuevo.Click += new System.EventHandler(this.toolStripNuevo_Click);
			// 
			// toolStripAbrir
			// 
			this.toolStripAbrir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAbrir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAbrir.Image")));
			this.toolStripAbrir.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripAbrir.Name = "toolStripAbrir";
			this.toolStripAbrir.Size = new System.Drawing.Size(23, 22);
			this.toolStripAbrir.Text = "Abrir Archivo";
			this.toolStripAbrir.Click += new System.EventHandler(this.toolStripAbrir_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripGuardar
			// 
			this.toolStripGuardar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripGuardar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGuardar.Image")));
			this.toolStripGuardar.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripGuardar.Name = "toolStripGuardar";
			this.toolStripGuardar.Size = new System.Drawing.Size(23, 22);
			this.toolStripGuardar.Text = "Guardar";
			this.toolStripGuardar.Click += new System.EventHandler(this.toolStripGuardar_Click);
			// 
			// toolStripGuardarComo
			// 
			this.toolStripGuardarComo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripGuardarComo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGuardarComo.Image")));
			this.toolStripGuardarComo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripGuardarComo.Name = "toolStripGuardarComo";
			this.toolStripGuardarComo.Size = new System.Drawing.Size(23, 22);
			this.toolStripGuardarComo.Text = "Guardar Como";
			this.toolStripGuardarComo.Click += new System.EventHandler(this.toolStripGuardarComo_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripEnviarFondo
			// 
			this.toolStripEnviarFondo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripEnviarFondo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripEnviarFondo.Image")));
			this.toolStripEnviarFondo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripEnviarFondo.Name = "toolStripEnviarFondo";
			this.toolStripEnviarFondo.Size = new System.Drawing.Size(23, 22);
			this.toolStripEnviarFondo.Text = "Llevar al Fondo";
			this.toolStripEnviarFondo.Click += new System.EventHandler(this.toolStripEnviarFondo_Click);
			// 
			// toolStripLlevarFrente
			// 
			this.toolStripLlevarFrente.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLlevarFrente.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLlevarFrente.Image")));
			this.toolStripLlevarFrente.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripLlevarFrente.Name = "toolStripLlevarFrente";
			this.toolStripLlevarFrente.Size = new System.Drawing.Size(23, 22);
			this.toolStripLlevarFrente.Text = "Traer al Frente";
			this.toolStripLlevarFrente.Click += new System.EventHandler(this.toolStripLlevarFrente_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripBorrar
			// 
			this.toolStripBorrar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBorrar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBorrar.Image")));
			this.toolStripBorrar.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBorrar.Name = "toolStripBorrar";
			this.toolStripBorrar.Size = new System.Drawing.Size(23, 22);
			this.toolStripBorrar.Text = "Eliminar Componente";
			this.toolStripBorrar.Click += new System.EventHandler(this.toolStripBorrar_Click);
			// 
			// toolStripDibujo
			// 
			this.toolStripDibujo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDibujo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDibujo.Image")));
			this.toolStripDibujo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDibujo.Name = "toolStripDibujo";
			this.toolStripDibujo.Size = new System.Drawing.Size(23, 22);
			this.toolStripDibujo.Text = "Modo Dibujo";
			this.toolStripDibujo.Click += new System.EventHandler(this.toolStripDibujo_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// labelMensaje
			// 
			this.labelMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMensaje.AutoSize = true;
			this.labelMensaje.Location = new System.Drawing.Point(12, 11);
			this.labelMensaje.Name = "labelMensaje";
			this.labelMensaje.Size = new System.Drawing.Size(53, 13);
			this.labelMensaje.TabIndex = 0;
			this.labelMensaje.Text = "Mensaje: ";
			// 
			// labelArchivo
			// 
			this.labelArchivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelArchivo.AutoSize = true;
			this.labelArchivo.Location = new System.Drawing.Point(978, 11);
			this.labelArchivo.Name = "labelArchivo";
			this.labelArchivo.Size = new System.Drawing.Size(122, 13);
			this.labelArchivo.TabIndex = 1;
			this.labelArchivo.Text = "Archivo Actual: Ninguno";
			// 
			// labelModo
			// 
			this.labelModo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.labelModo.AutoSize = true;
			this.labelModo.Location = new System.Drawing.Point(522, 11);
			this.labelModo.Name = "labelModo";
			this.labelModo.Size = new System.Drawing.Size(110, 13);
			this.labelModo.TabIndex = 2;
			this.labelModo.Text = "Modo: Desconectado";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel2.Controls.Add(this.labelModo);
			this.panel2.Controls.Add(this.labelArchivo);
			this.panel2.Controls.Add(this.labelMensaje);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 696);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1350, 33);
			this.panel2.TabIndex = 17;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(259, 52);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1073, 638);
			this.panel1.TabIndex = 14;
			this.panel1.Click += new System.EventHandler(this.panel1_Click);
			// 
			// panel4
			// 
			this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel4.Location = new System.Drawing.Point(1338, 49);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(12, 647);
			this.panel4.TabIndex = 19;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.groupBox1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel3.Location = new System.Drawing.Point(0, 49);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(253, 647);
			this.panel3.TabIndex = 18;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonValidaciones);
			this.groupBox1.Controls.Add(this.buttonModoManual);
			this.groupBox1.Controls.Add(this.buttonStop);
			this.groupBox1.Controls.Add(this.buttonMonitoreo);
			this.groupBox1.Controls.Add(this.buttonAgregarComp);
			this.groupBox1.Controls.Add(this.buttonConfig);
			this.groupBox1.Controls.Add(this.buttonComenzar);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(253, 312);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Panel de Herramientas";
			// 
			// buttonValidaciones
			// 
			this.buttonValidaciones.Location = new System.Drawing.Point(53, 225);
			this.buttonValidaciones.Name = "buttonValidaciones";
			this.buttonValidaciones.Size = new System.Drawing.Size(147, 36);
			this.buttonValidaciones.TabIndex = 6;
			this.buttonValidaciones.Text = "Validaciones y Optimizar Salidas";
			this.buttonValidaciones.UseVisualStyleBackColor = true;
			this.buttonValidaciones.Click += new System.EventHandler(this.buttonValidaciones_Click);
			// 
			// buttonModoManual
			// 
			this.buttonModoManual.Location = new System.Drawing.Point(53, 196);
			this.buttonModoManual.Name = "buttonModoManual";
			this.buttonModoManual.Size = new System.Drawing.Size(147, 23);
			this.buttonModoManual.TabIndex = 5;
			this.buttonModoManual.Text = "Modo Manual";
			this.buttonModoManual.UseVisualStyleBackColor = true;
			this.buttonModoManual.Click += new System.EventHandler(this.buttonModoManual_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Location = new System.Drawing.Point(53, 138);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(147, 23);
			this.buttonStop.TabIndex = 4;
			this.buttonStop.Text = "Parar/Pausar";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonMonitoreo
			// 
			this.buttonMonitoreo.Location = new System.Drawing.Point(53, 167);
			this.buttonMonitoreo.Name = "buttonMonitoreo";
			this.buttonMonitoreo.Size = new System.Drawing.Size(147, 23);
			this.buttonMonitoreo.TabIndex = 3;
			this.buttonMonitoreo.Text = "Monitoreo";
			this.buttonMonitoreo.UseVisualStyleBackColor = true;
			this.buttonMonitoreo.Click += new System.EventHandler(this.buttonMonitoreo_Click);
			// 
			// buttonAgregarComp
			// 
			this.buttonAgregarComp.Location = new System.Drawing.Point(53, 51);
			this.buttonAgregarComp.Name = "buttonAgregarComp";
			this.buttonAgregarComp.Size = new System.Drawing.Size(147, 23);
			this.buttonAgregarComp.TabIndex = 2;
			this.buttonAgregarComp.Text = "Agregar Componentes";
			this.buttonAgregarComp.UseVisualStyleBackColor = true;
			this.buttonAgregarComp.Click += new System.EventHandler(this.buttonAgregarComp_Click);
			// 
			// buttonConfig
			// 
			this.buttonConfig.Location = new System.Drawing.Point(53, 80);
			this.buttonConfig.Name = "buttonConfig";
			this.buttonConfig.Size = new System.Drawing.Size(147, 23);
			this.buttonConfig.TabIndex = 1;
			this.buttonConfig.Text = "Configuraciones";
			this.buttonConfig.UseVisualStyleBackColor = true;
			this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
			// 
			// buttonComenzar
			// 
			this.buttonComenzar.Location = new System.Drawing.Point(53, 109);
			this.buttonComenzar.Name = "buttonComenzar";
			this.buttonComenzar.Size = new System.Drawing.Size(147, 23);
			this.buttonComenzar.TabIndex = 0;
			this.buttonComenzar.Text = "Comenzar Mediciones";
			this.buttonComenzar.UseVisualStyleBackColor = true;
			this.buttonComenzar.Click += new System.EventHandler(this.buttonComenzar_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1350, 729);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "Form1";
			this.Text = "Vent Control";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem guardarcomoToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem acercadeToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripLlevarFrente;
		private System.Windows.Forms.ToolStripButton toolStripGuardar;
		private System.Windows.Forms.ToolStripButton toolStripGuardarComo;
		private System.Windows.Forms.ToolStripButton toolStripAbrir;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton toolStripBorrar;
		private System.Windows.Forms.ToolStripButton toolStripNuevo;
		private System.Windows.Forms.ToolStripButton toolStripEnviarFondo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripDibujo;
		private System.Windows.Forms.ToolStripMenuItem verGraficosToolStripMenuItem;
		private System.Windows.Forms.Label labelMensaje;
		private System.Windows.Forms.Label labelArchivo;
		private System.Windows.Forms.Label labelModo;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonComenzar;
		private System.Windows.Forms.Button buttonConfig;
		private System.Windows.Forms.Button buttonAgregarComp;
		private System.Windows.Forms.Button buttonMonitoreo;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonModoManual;
		private System.Windows.Forms.Button buttonValidaciones;
	}
}

