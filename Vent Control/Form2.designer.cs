namespace Vent_Control
{
    partial class FTiempoVisualizacion
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
            this.txtY = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.lblVisualizacion = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbUnidadTiempo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(100, 57);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(142, 22);
            this.txtY.TabIndex = 0;
            this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(266, 89);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // lblVisualizacion
            // 
            this.lblVisualizacion.AutoSize = true;
            this.lblVisualizacion.Location = new System.Drawing.Point(81, 25);
            this.lblVisualizacion.Name = "lblVisualizacion";
            this.lblVisualizacion.Size = new System.Drawing.Size(188, 17);
            this.lblVisualizacion.TabIndex = 2;
            this.lblVisualizacion.Text = "Ingrese Intervalo de Tiempo ";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cbUnidadTiempo
            // 
            this.cbUnidadTiempo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnidadTiempo.FormattingEnabled = true;
            this.cbUnidadTiempo.Location = new System.Drawing.Point(253, 57);
            this.cbUnidadTiempo.Name = "cbUnidadTiempo";
            this.cbUnidadTiempo.Size = new System.Drawing.Size(102, 24);
            this.cbUnidadTiempo.TabIndex = 3;
            // 
            // FTiempoVisualizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 124);
            this.Controls.Add(this.cbUnidadTiempo);
            this.Controls.Add(this.lblVisualizacion);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtY);
            this.Name = "FTiempoVisualizacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tiempo de Visualización";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label lblVisualizacion;
        public System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cbUnidadTiempo;
    }
}