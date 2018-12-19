namespace ServidorDB
{
    partial class ServidorWeb
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
            this.tab_inicial = new System.Windows.Forms.TabControl();
            this.login = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_iniciar = new System.Windows.Forms.Button();
            this.txt_pass = new System.Windows.Forms.TextBox();
            this.txt_user = new System.Windows.Forms.TextBox();
            this.eti_pass = new System.Windows.Forms.Label();
            this.eti_user = new System.Windows.Forms.Label();
            this.eti_inicio = new System.Windows.Forms.Label();
            this.fisql = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tab_inicial.SuspendLayout();
            this.login.SuspendLayout();
            this.panel1.SuspendLayout();
            this.fisql.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_inicial
            // 
            this.tab_inicial.Controls.Add(this.login);
            this.tab_inicial.Controls.Add(this.fisql);
            this.tab_inicial.Location = new System.Drawing.Point(2, 3);
            this.tab_inicial.Name = "tab_inicial";
            this.tab_inicial.SelectedIndex = 0;
            this.tab_inicial.Size = new System.Drawing.Size(1338, 696);
            this.tab_inicial.TabIndex = 0;
            // 
            // login
            // 
            this.login.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.login.Controls.Add(this.panel1);
            this.login.Location = new System.Drawing.Point(4, 25);
            this.login.Name = "login";
            this.login.Padding = new System.Windows.Forms.Padding(3);
            this.login.Size = new System.Drawing.Size(1330, 667);
            this.login.TabIndex = 0;
            this.login.Text = "Login";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btn_iniciar);
            this.panel1.Controls.Add(this.txt_pass);
            this.panel1.Controls.Add(this.txt_user);
            this.panel1.Controls.Add(this.eti_pass);
            this.panel1.Controls.Add(this.eti_user);
            this.panel1.Controls.Add(this.eti_inicio);
            this.panel1.Location = new System.Drawing.Point(349, 148);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 335);
            this.panel1.TabIndex = 0;
            // 
            // btn_iniciar
            // 
            this.btn_iniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_iniciar.Location = new System.Drawing.Point(62, 236);
            this.btn_iniciar.Name = "btn_iniciar";
            this.btn_iniciar.Size = new System.Drawing.Size(475, 48);
            this.btn_iniciar.TabIndex = 5;
            this.btn_iniciar.Text = "Iniciar";
            this.btn_iniciar.UseVisualStyleBackColor = true;
            this.btn_iniciar.Click += new System.EventHandler(this.btn_iniciar_Click);
            // 
            // txt_pass
            // 
            this.txt_pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_pass.Location = new System.Drawing.Point(209, 173);
            this.txt_pass.Name = "txt_pass";
            this.txt_pass.Size = new System.Drawing.Size(328, 38);
            this.txt_pass.TabIndex = 4;
            // 
            // txt_user
            // 
            this.txt_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_user.Location = new System.Drawing.Point(209, 113);
            this.txt_user.Name = "txt_user";
            this.txt_user.Size = new System.Drawing.Size(328, 38);
            this.txt_user.TabIndex = 3;
            // 
            // eti_pass
            // 
            this.eti_pass.AutoSize = true;
            this.eti_pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eti_pass.Location = new System.Drawing.Point(56, 179);
            this.eti_pass.Name = "eti_pass";
            this.eti_pass.Size = new System.Drawing.Size(147, 32);
            this.eti_pass.TabIndex = 2;
            this.eti_pass.Text = "Password:";
            // 
            // eti_user
            // 
            this.eti_user.AutoSize = true;
            this.eti_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eti_user.Location = new System.Drawing.Point(56, 113);
            this.eti_user.Name = "eti_user";
            this.eti_user.Size = new System.Drawing.Size(121, 32);
            this.eti_user.TabIndex = 1;
            this.eti_user.Text = "Usuario:";
            // 
            // eti_inicio
            // 
            this.eti_inicio.AutoSize = true;
            this.eti_inicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eti_inicio.Location = new System.Drawing.Point(222, 23);
            this.eti_inicio.Name = "eti_inicio";
            this.eti_inicio.Size = new System.Drawing.Size(141, 55);
            this.eti_inicio.TabIndex = 0;
            this.eti_inicio.Text = "Inicio";
            // 
            // fisql
            // 
            this.fisql.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.fisql.Controls.Add(this.tabControl2);
            this.fisql.Controls.Add(this.tabControl1);
            this.fisql.Controls.Add(this.treeView1);
            this.fisql.Controls.Add(this.menuStrip1);
            this.fisql.Location = new System.Drawing.Point(4, 25);
            this.fisql.Name = "fisql";
            this.fisql.Padding = new System.Windows.Forms.Padding(3);
            this.fisql.Size = new System.Drawing.Size(1330, 667);
            this.fisql.TabIndex = 1;
            this.fisql.Text = "FISQL - IDE";
            this.fisql.Click += new System.EventHandler(this.fisql_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.loginToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1324, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.herramientasToolStripMenuItem.Text = "Herramientas";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.loginToolStripMenuItem.Text = "Login";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(266, 355);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1061, 308);
            this.tabControl2.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1053, 279);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1045, 266);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(262, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1065, 323);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1057, 294);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(855, 294);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.treeView1.Location = new System.Drawing.Point(6, 30);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(250, 631);
            this.treeView1.TabIndex = 3;
            // 
            // ServidorWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 703);
            this.Controls.Add(this.tab_inicial);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServidorWeb";
            this.Text = "ServidorWeb";
            this.tab_inicial.ResumeLayout(false);
            this.login.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.fisql.ResumeLayout(false);
            this.fisql.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_inicial;
        private System.Windows.Forms.TabPage login;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_iniciar;
        private System.Windows.Forms.TextBox txt_pass;
        private System.Windows.Forms.TextBox txt_user;
        private System.Windows.Forms.Label eti_pass;
        private System.Windows.Forms.Label eti_user;
        private System.Windows.Forms.Label eti_inicio;
        private System.Windows.Forms.TabPage fisql;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeView1;
    }
}