namespace Vent_Control
{
    partial class Clase_Ventsim
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clase_Ventsim));
            this.btnExaminar = new System.Windows.Forms.Button();
            this.btnAbrirModelo = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExaminar
            // 
            this.btnExaminar.Location = new System.Drawing.Point(54, 40);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(135, 32);
            this.btnExaminar.TabIndex = 0;
            this.btnExaminar.Text = "Examinar Modelo";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // btnAbrirModelo
            // 
            this.btnAbrirModelo.Location = new System.Drawing.Point(54, 101);
            this.btnAbrirModelo.Name = "btnAbrirModelo";
            this.btnAbrirModelo.Size = new System.Drawing.Size(135, 32);
            this.btnAbrirModelo.TabIndex = 1;
            this.btnAbrirModelo.Text = "Abrir Modelo";
            this.btnAbrirModelo.UseVisualStyleBackColor = true;
            this.btnAbrirModelo.Click += new System.EventHandler(this.btnAbrirModelo_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(258, 40);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(115, 32);
            this.btnSalir.TabIndex = 2;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // Clase_Ventsim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 166);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnAbrirModelo);
            this.Controls.Add(this.btnExaminar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(508, 213);
            this.MinimumSize = new System.Drawing.Size(508, 213);
            this.Name = "Clase_Ventsim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modelo .vsm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Button btnAbrirModelo;
        private System.Windows.Forms.Button btnSalir;
    }
}