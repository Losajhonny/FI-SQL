namespace ServidorDB
{
    partial class ServidorDb
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
            this.components = new System.ComponentModel.Container();
            this.consola = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // consola
            // 
            this.consola.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.consola.Location = new System.Drawing.Point(12, 12);
            this.consola.Name = "consola";
            this.consola.Size = new System.Drawing.Size(677, 519);
            this.consola.TabIndex = 0;
            this.consola.Text = "";
            // 
            // ServidorDb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 543);
            this.Controls.Add(this.consola);
            this.Name = "ServidorDb";
            this.Text = "Consola del Servidor DB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServidorDb_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServidorDb_FormClosed);
            this.Load += new System.EventHandler(this.ServidorDb_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox consola;
        private System.Windows.Forms.Timer timer1;
        //private System.Windows.Forms.RichTextBox rtb_consola;
    }
}

