namespace Vent_Control
{
    partial class Estadisticas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Estadisticas));
            this.tbEstadisticas = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbEstadisticas
            // 
            this.tbEstadisticas.Location = new System.Drawing.Point(12, 12);
            this.tbEstadisticas.Multiline = true;
            this.tbEstadisticas.Name = "tbEstadisticas";
            this.tbEstadisticas.ReadOnly = true;
            this.tbEstadisticas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEstadisticas.Size = new System.Drawing.Size(439, 404);
            this.tbEstadisticas.TabIndex = 0;
            // 
            // Estadisticas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 427);
            this.Controls.Add(this.tbEstadisticas);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Estadisticas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estadísticas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbEstadisticas;
    }
}