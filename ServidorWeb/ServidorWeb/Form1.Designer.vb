<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tab_inicial = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.pictureline = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btn_iniciar = New System.Windows.Forms.Button()
        Me.txt_password = New System.Windows.Forms.TextBox()
        Me.txt_usuario = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FISQL = New System.Windows.Forms.TabPage()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btn_ejescript = New System.Windows.Forms.Button()
        Me.btn_ejecutar = New System.Windows.Forms.Button()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.gv_salida = New System.Windows.Forms.DataGridView()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.rtb_plan_ejecucion = New System.Windows.Forms.RichTextBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.rtb_mensaje = New System.Windows.Forms.RichTextBox()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.tabide = New System.Windows.Forms.TabControl()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.menuBar = New System.Windows.Forms.MenuStrip()
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btn_agregar = New System.Windows.Forms.ToolStripMenuItem()
        Me.btn_eliminar = New System.Windows.Forms.ToolStripMenuItem()
        Me.HerramientasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.tab_inicial.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.pictureline, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.FISQL.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.gv_salida, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.menuBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'tab_inicial
        '
        Me.tab_inicial.Controls.Add(Me.TabPage1)
        Me.tab_inicial.Controls.Add(Me.FISQL)
        Me.tab_inicial.Location = New System.Drawing.Point(2, 3)
        Me.tab_inicial.Name = "tab_inicial"
        Me.tab_inicial.SelectedIndex = 0
        Me.tab_inicial.Size = New System.Drawing.Size(1338, 699)
        Me.tab_inicial.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TabPage1.Controls.Add(Me.pictureline)
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1330, 670)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Login"
        '
        'pictureline
        '
        Me.pictureline.Location = New System.Drawing.Point(991, 71)
        Me.pictureline.Name = "pictureline"
        Me.pictureline.Size = New System.Drawing.Size(264, 311)
        Me.pictureline.TabIndex = 1
        Me.pictureline.TabStop = False
        Me.pictureline.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btn_iniciar)
        Me.Panel1.Controls.Add(Me.txt_password)
        Me.Panel1.Controls.Add(Me.txt_usuario)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(337, 135)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(604, 318)
        Me.Panel1.TabIndex = 0
        '
        'btn_iniciar
        '
        Me.btn_iniciar.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_iniciar.Location = New System.Drawing.Point(67, 222)
        Me.btn_iniciar.Name = "btn_iniciar"
        Me.btn_iniciar.Size = New System.Drawing.Size(465, 46)
        Me.btn_iniciar.TabIndex = 5
        Me.btn_iniciar.Text = "Iniciar"
        Me.btn_iniciar.UseVisualStyleBackColor = True
        '
        'txt_password
        '
        Me.txt_password.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_password.Location = New System.Drawing.Point(214, 163)
        Me.txt_password.Name = "txt_password"
        Me.txt_password.Size = New System.Drawing.Size(318, 38)
        Me.txt_password.TabIndex = 4
        '
        'txt_usuario
        '
        Me.txt_usuario.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_usuario.Location = New System.Drawing.Point(214, 99)
        Me.txt_usuario.Name = "txt_usuario"
        Me.txt_usuario.Size = New System.Drawing.Size(318, 38)
        Me.txt_usuario.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(61, 163)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(147, 32)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Password:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(61, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 32)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Usuario:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(217, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(159, 51)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "INICIO"
        '
        'FISQL
        '
        Me.FISQL.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.FISQL.Controls.Add(Me.FlowLayoutPanel1)
        Me.FISQL.Controls.Add(Me.TabControl2)
        Me.FISQL.Controls.Add(Me.tabide)
        Me.FISQL.Controls.Add(Me.TreeView1)
        Me.FISQL.Controls.Add(Me.menuBar)
        Me.FISQL.Location = New System.Drawing.Point(4, 25)
        Me.FISQL.Name = "FISQL"
        Me.FISQL.Padding = New System.Windows.Forms.Padding(3)
        Me.FISQL.Size = New System.Drawing.Size(1330, 670)
        Me.FISQL.TabIndex = 1
        Me.FISQL.Text = "FISQL - IDE"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btn_ejescript)
        Me.FlowLayoutPanel1.Controls.Add(Me.btn_ejecutar)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(233, 34)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(1087, 38)
        Me.FlowLayoutPanel1.TabIndex = 4
        '
        'btn_ejescript
        '
        Me.btn_ejescript.Location = New System.Drawing.Point(3, 3)
        Me.btn_ejescript.Name = "btn_ejescript"
        Me.btn_ejescript.Size = New System.Drawing.Size(111, 35)
        Me.btn_ejescript.TabIndex = 0
        Me.btn_ejescript.Text = "Ejecutar Script"
        Me.btn_ejescript.UseVisualStyleBackColor = True
        '
        'btn_ejecutar
        '
        Me.btn_ejecutar.Location = New System.Drawing.Point(120, 3)
        Me.btn_ejecutar.Name = "btn_ejecutar"
        Me.btn_ejecutar.Size = New System.Drawing.Size(82, 35)
        Me.btn_ejecutar.TabIndex = 1
        Me.btn_ejecutar.Text = "Ejecutar"
        Me.btn_ejecutar.UseVisualStyleBackColor = True
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage4)
        Me.TabControl2.Controls.Add(Me.TabPage5)
        Me.TabControl2.Controls.Add(Me.TabPage6)
        Me.TabControl2.Controls.Add(Me.TabPage7)
        Me.TabControl2.Location = New System.Drawing.Point(237, 382)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(1087, 281)
        Me.TabControl2.TabIndex = 3
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.gv_salida)
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(1079, 252)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "Salida Datos"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'gv_salida
        '
        Me.gv_salida.AllowUserToAddRows = False
        Me.gv_salida.AllowUserToDeleteRows = False
        Me.gv_salida.BackgroundColor = System.Drawing.SystemColors.ButtonFace
        Me.gv_salida.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gv_salida.Location = New System.Drawing.Point(6, 6)
        Me.gv_salida.Name = "gv_salida"
        Me.gv_salida.ReadOnly = True
        Me.gv_salida.RowTemplate.Height = 24
        Me.gv_salida.Size = New System.Drawing.Size(1067, 243)
        Me.gv_salida.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.rtb_plan_ejecucion)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(1079, 252)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Plan Ejecucion"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'rtb_plan_ejecucion
        '
        Me.rtb_plan_ejecucion.Location = New System.Drawing.Point(3, 3)
        Me.rtb_plan_ejecucion.Name = "rtb_plan_ejecucion"
        Me.rtb_plan_ejecucion.Size = New System.Drawing.Size(1070, 246)
        Me.rtb_plan_ejecucion.TabIndex = 0
        Me.rtb_plan_ejecucion.Text = ""
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.rtb_mensaje)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(1079, 252)
        Me.TabPage6.TabIndex = 2
        Me.TabPage6.Text = "Mensajes"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'rtb_mensaje
        '
        Me.rtb_mensaje.Location = New System.Drawing.Point(3, 3)
        Me.rtb_mensaje.Name = "rtb_mensaje"
        Me.rtb_mensaje.Size = New System.Drawing.Size(1073, 246)
        Me.rtb_mensaje.TabIndex = 0
        Me.rtb_mensaje.Text = ""
        '
        'TabPage7
        '
        Me.TabPage7.Location = New System.Drawing.Point(4, 25)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(1079, 252)
        Me.TabPage7.TabIndex = 3
        Me.TabPage7.Text = "Lista Errores"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'tabide
        '
        Me.tabide.Location = New System.Drawing.Point(233, 78)
        Me.tabide.Name = "tabide"
        Me.tabide.SelectedIndex = 0
        Me.tabide.Size = New System.Drawing.Size(1091, 298)
        Me.tabide.TabIndex = 2
        '
        'TreeView1
        '
        Me.TreeView1.Location = New System.Drawing.Point(6, 34)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(221, 629)
        Me.TreeView1.TabIndex = 0
        '
        'menuBar
        '
        Me.menuBar.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.menuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.HerramientasToolStripMenuItem, Me.LoginToolStripMenuItem})
        Me.menuBar.Location = New System.Drawing.Point(3, 3)
        Me.menuBar.Name = "menuBar"
        Me.menuBar.Size = New System.Drawing.Size(1324, 28)
        Me.menuBar.TabIndex = 1
        Me.menuBar.Text = "menuBar"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btn_agregar, Me.btn_eliminar})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(71, 24)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'btn_agregar
        '
        Me.btn_agregar.Name = "btn_agregar"
        Me.btn_agregar.Size = New System.Drawing.Size(165, 26)
        Me.btn_agregar.Text = "Agregar Tab"
        '
        'btn_eliminar
        '
        Me.btn_eliminar.Name = "btn_eliminar"
        Me.btn_eliminar.Size = New System.Drawing.Size(165, 26)
        Me.btn_eliminar.Text = "Eliminar Tab"
        '
        'HerramientasToolStripMenuItem
        '
        Me.HerramientasToolStripMenuItem.Name = "HerramientasToolStripMenuItem"
        Me.HerramientasToolStripMenuItem.Size = New System.Drawing.Size(110, 24)
        Me.HerramientasToolStripMenuItem.Text = "Herramientas"
        '
        'LoginToolStripMenuItem
        '
        Me.LoginToolStripMenuItem.Name = "LoginToolStripMenuItem"
        Me.LoginToolStripMenuItem.Size = New System.Drawing.Size(58, 24)
        Me.LoginToolStripMenuItem.Text = "Login"
        '
        'Timer1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1342, 703)
        Me.Controls.Add(Me.tab_inicial)
        Me.MainMenuStrip = Me.menuBar
        Me.Name = "Form1"
        Me.Text = "ServidorWeb"
        Me.tab_inicial.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.pictureline, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.FISQL.ResumeLayout(False)
        Me.FISQL.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        CType(Me.gv_salida, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.menuBar.ResumeLayout(False)
        Me.menuBar.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tab_inicial As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btn_iniciar As Button
    Friend WithEvents txt_password As TextBox
    Friend WithEvents txt_usuario As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents FISQL As TabPage
    Friend WithEvents TabControl2 As TabControl
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents tabide As TabControl
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents menuBar As MenuStrip
    Friend WithEvents ArchivoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HerramientasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoginToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents TabPage7 As TabPage
    Friend WithEvents btn_agregar As ToolStripMenuItem
    Friend WithEvents btn_eliminar As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents pictureline As PictureBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btn_ejescript As Button
    Friend WithEvents btn_ejecutar As Button
    Friend WithEvents rtb_plan_ejecucion As RichTextBox
    Friend WithEvents rtb_mensaje As RichTextBox
    Friend WithEvents gv_salida As DataGridView
End Class
