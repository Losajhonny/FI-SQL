Imports System.IO

Public Class Form1
    Dim CARACTER As Integer 'NUMERO DE CARACTERES DEL RICHTEXTBOX

    Private tab_ide As TabPage
    Private tab_log As TabPage

    Public logica As Logica = New Logica()

    Private Sub btn_iniciar_Click(sender As Object, e As EventArgs) Handles btn_iniciar.Click
        If Not (txt_usuario.Text.Equals("") And txt_password.Text.Equals("")) Then
            ''realizar la peticion
            Dim resultado As Boolean = logica.inicio_sesion(txt_usuario.Text, txt_password.Text)
            ''ahora debo de analizar el resultado
            ''ya recibi la peticion


            If resultado = True Then
                eti_nombre_usuario.Text = txt_usuario.Text
                MessageBox.Show("Bienvenido " + txt_usuario.Text, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txt_usuario.Text = ""
                txt_password.Text = ""

                tab_inicial.TabPages.Remove(tab_log)
                tab_inicial.TabPages.Add(tab_ide)
                tab_inicial.SelectedIndex = 0

                rtb_log.Text = logica.log2
            End If


        Else
            MessageBox.Show("Ingrese el usuario y contraseña", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 10 'ACTUALIZACION  PICTUREBOX
        Timer1.Start()


        Dim val As String = Convert.ToString(DateTime.Today - DateTime.Now)

        tab_log = tab_inicial.TabPages.Item(0)
        tab_ide = tab_inicial.TabPages.Item(1)

        tab_inicial.TabPages.Remove(tab_ide)

        agregarTab("SQL")
    End Sub

    Private Sub btn_agregar_Click(sender As Object, e As EventArgs)
        agregarTab(Nothing)
    End Sub

    Public Sub agregarTab(ByVal nombre As String)

        Dim tp As TabPage

        If nombre Is Nothing Then
            tp = New TabPage("Script " + (tabide.TabPages.Count + 1).ToString())
        Else
            tp = New TabPage(nombre)
        End If

        Dim pb As PictureBox = New PictureBox()
        pb.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        pb.BackColor = System.Drawing.Color.LightGray
        pb.Location = New System.Drawing.Point(0, 0)
        pb.Size = New System.Drawing.Size(53, 587)
        pb.TabIndex = 2
        pb.TabStop = False



        'pb.Paint += New System.Windows.Forms.PaintEventHandler(Me.PictureBox1_Paint)

        Dim rt As RichTextBox = New RichTextBox()

        rt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        rt.BorderStyle = System.Windows.Forms.BorderStyle.None
        ''rt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        rt.Location = New System.Drawing.Point(65, 12)
        rt.Size = New System.Drawing.Size(557, 587)
        rt.TabIndex = 2
        rt.Text = ""

        rt.BorderStyle = BorderStyle.None
        rt.Cursor = System.Windows.Forms.Cursors.IBeam
        rt.ScrollBars = RichTextBoxScrollBars.Both
        rt.SetBounds(pb.Width, 0, 150, 100)
        rt.AcceptsTab = True
        '//rt.TextChanged += New System.EventHandler(rich_TextChanged);
        '//rt.MouseEnter += New System.EventHandler(rich_MouseEnter);

        tp.Controls.Add(pb) '' //pos 0 es ListBox
        tp.Controls.Add(rt) '' //pos 1 es el richtextbox

        tabide.TabPages.Add(tp)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim picture As PictureBox = tabide.SelectedTab.Controls(0)
            picture.Refresh() 'ACTUALIZACION  PICTUREBOX 
        Catch ex As Exception

        End Try
    End Sub

    Private Event MyEvente(e As System.Windows.Forms.PaintEventArgs)

    Private Sub PictureBox1_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles pictureline.Paint

        Try

            CARACTER = 0 'SE INICIALIZA A 0 EN CADA REPINTADO
            Dim picture As PictureBox = tabide.SelectedTab.Controls(0)
            Dim rich As RichTextBox = tabide.SelectedTab.Controls(1)
            Dim ALTURA As Integer = rich.GetPositionFromCharIndex(0).Y 'COORDENADA Y DEL PRIMER CARACTER DEL TEXTO

            If rich.Lines.Length > 0 Then 'SI HAY ALGUNA LINEA LAS RECORRERA TODAS Y ESCRIBIRA SU NUMERO AL LADO DEL PRIMER CARACTER DE LA LINEA
                For I = 0 To rich.Lines.Length - 1
                    e.Graphics.DrawString(I + 1, rich.Font, Brushes.Blue, picture.Width - (e.Graphics.MeasureString(I + 1, rich.Font).Width + 10), ALTURA)
                    CARACTER += rich.Lines(I).Length + 1 'INDICE DEL PRIMER CARACTER DE LA LINEA SIGUIENTE
                    ALTURA = rich.GetPositionFromCharIndex(CARACTER).Y 'POSICION EN Y DEL PRIMER CARACTER DE LA LINEA SIGUIENTE
                Next
            Else 'PARA QUE SE INICIE CON UN 1 EN EL PICTUREBOX
                e.Graphics.DrawString(1, rich.Font, Brushes.Blue, picture.Width - (e.Graphics.MeasureString(1, rich.Font).Width + 10), ALTURA)
            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub btn_ejescript_Click(sender As Object, e As EventArgs)
        ejecutarScript()

    End Sub

    Private Sub btn_ejecutar_Click(sender As Object, e As EventArgs)
        ejecutarSelect()
    End Sub

    Private Sub ejecutarScript()
        ''Envia el texto de todo el richtextbox

        Dim texto As RichTextBox = tabide.SelectedTab.Controls(1)
        Dim cadena As String = texto.Text

        If Not cadena.Equals("") Then
            ejecutarCodigo(cadena)
        End If
    End Sub

    Private Sub ejecutarSelect()
        Dim texto As RichTextBox = tabide.SelectedTab.Controls(1)
        Dim cadena As String = texto.SelectedText

        If Not cadena.Equals("") Then
            ejecutarCodigo(cadena)
        End If
    End Sub

    Private Sub ejecutarCodigo(ByVal cadena As String)
        ''aqui realizar la peticion de instruccion
        logica.paquete_Instruccion(cadena)

        rtb_log.Text = logica.log2
        rtb_plan_ejecucion.Text = logica.log
        rtb_mensaje.Text = logica.consola

        If Not logica.tabla Is Nothing Then
            gv_salida.DataSource = logica.tabla
        End If

        If Not logica.errorr Is Nothing Then
            gv_errores.DataSource = logica.errorr

            For i = 0 To logica.errorr.Columns.Count - 1
                If gv_errores.Columns(i).Name.Equals("fila") Or gv_errores.Columns(i).Name.Equals("col") Or gv_errores.Columns(i).Name.Equals("lexema") Then
                    gv_errores.Columns(i).Width = 60
                ElseIf gv_errores.Columns(i).Name.Equals("Descripcion") Then
                    gv_errores.Columns(i).Width = 400
                End If
            Next
        End If
    End Sub

    Private Sub btn_eliminar_Click(sender As Object, e As EventArgs)
        If Not tabide.SelectedIndex = 0 Then
            tabide.TabPages.Remove(tabide.SelectedTab)
        End If
    End Sub

    Private Sub EjecutarScriptToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ejecutarScript()
    End Sub

    Private Sub EjecutarToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ejecutarSelect()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        ejecutarScript()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        ejecutarSelect()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        ''guardar y cerrar
        If Not tabide.SelectedIndex = 0 Then

            tabide.TabPages.Remove(tabide.SelectedTab)
        End If
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        ''guardar todo el script

    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        ''eliinar texto seleccionado
        Dim rtb_tmp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtb_tmp.Text = ""
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        agregarTab(Nothing)
    End Sub

    Private Sub AgregarTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgregarTabToolStripMenuItem.Click
        agregarTab(Nothing)
    End Sub

    Private Sub EliminarTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EliminarTabToolStripMenuItem.Click
        If Not tabide.SelectedIndex = 0 Then

            tabide.TabPages.Remove(tabide.SelectedTab)
        End If
    End Sub

    Private Sub GuardarScriptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GuardarScriptToolStripMenuItem.Click
        ''guaradar 
    End Sub

    Private Sub EjecutarScriptToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles EjecutarScriptToolStripMenuItem.Click
        ejecutarScript()
    End Sub

    Private Sub EjecutarToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles EjecutarToolStripMenuItem.Click
        ejecutarSelect()
    End Sub

    Private Sub LimpiarTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LimpiarTabToolStripMenuItem.Click
        ''eliinar texto seleccionado
        Dim rtb_tmp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtb_tmp.Text = ""
    End Sub

    Private Sub BaseDeDatosToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BaseDeDatosToolStripMenuItem1.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "crear base_datos <<nombre_db>>;" + vbNewLine

    End Sub

    Private Sub UsuarioToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles UsuarioToolStripMenuItem2.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "crear usuario <<nombre_usuario>> colocar password = <<password_usuario>>;" + vbNewLine
    End Sub

    Private Sub TablaToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles TablaToolStripMenuItem3.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "#El compelemento puede ser nulo, no nulo, autoincrementable, unico, llave_primaria, llave_foranea tabla atributo" + vbNewLine
        rtbTemp.Text += vbNewLine + "crear tabla <<nombre_tabla>> (" + vbNewLine + "<<Tipo_dato>> <<nombre_campo>> [ <<complemento>> ] ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_campo>> [ <<complemento>> ] , " + vbNewLine + "<<NAtributos>>" + vbNewLine + ");" + vbNewLine
    End Sub

    Private Sub ObjetoToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ObjetoToolStripMenuItem3.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "crear objeto <<nombre_objeto>> (" + vbNewLine + "<<Tipo_dato>> <<nombre_campo>> ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_campo>> , " + vbNewLine + "<<NAtributos>>" + vbNewLine + ");" + vbNewLine
    End Sub

    Private Sub ProcedimientoToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ProcedimientoToolStripMenuItem1.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "crear procedimiento <<nombre_procedimiento>> (" + vbNewLine + "<<Tipo_dato>> <<nombre_parametro>> ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_parametro>> , " + vbNewLine + "<<NParametros>>" + vbNewLine + ") {" + vbNewLine
        rtbTemp.Text += "<<sentencias>>" + vbNewLine + "}" + vbNewLine
    End Sub

    Private Sub FuncionToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FuncionToolStripMenuItem1.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "crear Funcion <<nombre_funcion>> (" + vbNewLine + "<<Tipo_dato>> <<nombre_parametro>> ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_parametro>> , " + vbNewLine + "<<NParametros>>" + vbNewLine + ") <<Tipo_Retorno>> {" + vbNewLine
        rtbTemp.Text += "<<sentencias>>" + vbNewLine + "}" + vbNewLine
    End Sub

    Private Sub USARToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles USARToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "usar <<nombre_db>>;" + vbNewLine
    End Sub

    Private Sub AgregarTablaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgregarTablaToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "alterar tabla <<nombre_tabla>> agregar (" + vbNewLine + "<<Tipo_dato>> <<nombre_campo>> [ <<complemento>> ] ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_campo>> [ <<complemento>> ] , " + vbNewLine + "<<NAtributos>>" + vbNewLine + ");" + vbNewLine
    End Sub

    Private Sub QuitarTablaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitarTablaToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "alterar tabla <<nombre_tabla>> quitar <<Campo1 , Campo2, CampoN>>" + vbNewLine
    End Sub

    Private Sub AgregarObjetoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgregarObjetoToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "alterar objeto <<nombre_objeto>> agregar (" + vbNewLine + "<<Tipo_dato>> <<nombre_campo>> ,"
        rtbTemp.Text += vbNewLine + "<<Tipo_dato>> <<nombre_campo>> , " + vbNewLine + "<<NAtributos>>" + vbNewLine + ");" + vbNewLine
    End Sub

    Private Sub QuitarObjetoToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles QuitarObjetoToolStripMenuItem1.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "alterar objeto <<nombre_objeto>> quitar <<Campo1 , Campo2, CampoN>> ;" + vbNewLine
    End Sub

    Private Sub CambiarPasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CambiarPasswordToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "alterar usuario <<nombre_tabla>> cambiar password = <<nuevo_password>> ;" + vbNewLine
    End Sub

    Private Sub TablaToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles TablaToolStripMenuItem4.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "eliminar tabla <<nombre_tabla>> ;" + vbNewLine
    End Sub

    Private Sub ObjetoToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ObjetoToolStripMenuItem4.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "eliminar objeto <<nombre_objeto>> ;" + vbNewLine
    End Sub

    Private Sub UsuarioToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles UsuarioToolStripMenuItem3.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "eliminar usuario <<nombre_usuario>> ;" + vbNewLine
    End Sub

    Private Sub InsertarNormalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertarNormalToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "insertar en tabla <<nombre_tabla>> ( <<valor1, valor2, valorn>> );" + vbNewLine
    End Sub

    Private Sub InsertarEspecialToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertarEspecialToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "insertar en tabla <<nombre_tabla>> ( <<campo1, campo2, campon>> ) valores ( <<valor1, valor2, valorn>> );" + vbNewLine
    End Sub

    Private Sub ActualizarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActualizarToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "actualizar tabla <<nombre_tabla>> ( <<campo1, campo2, campon>> ) valores ( <<valor1, valor2, valorn>> ) donde <<EXPRESION>>;" + vbNewLine
    End Sub

    Private Sub BorrarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BorrarToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "borrar en tabla <<nombre_tabla>> donde <<EXPRESION>>;" + vbNewLine
    End Sub

    Private Sub OtorgarPermisoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OtorgarPermisoToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "otorgar permisos <<nombre_usuario>>, <<nombre_db>>.<<nombre_objeto>> ;" + vbNewLine
    End Sub

    Private Sub DenegarPermisoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DenegarPermisoToolStripMenuItem.Click
        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "denegar permisos <<nombre_usuario>>, <<nombre_db>>.<<nombre_objeto>> ;" + vbNewLine
    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        tab_inicial.TabPages.Remove(tab_ide)
        tab_inicial.TabPages.Add(tab_log)
        tab_inicial.SelectedIndex = 0

        '''''debo reiniciar todo
    End Sub

    Private Sub SeleccionarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeleccionarToolStripMenuItem.Click

        tabide.SelectTab(0)

        Dim rtbTemp As RichTextBox = tabide.SelectedTab.Controls(1)
        rtbTemp.Text += vbNewLine + "seleccionar ( <<atr1, atr2, atrn>> | * ) de <<tabla1, tabla2, tablan>>" + vbNewLine
        rtbTemp.Text += vbNewLine + "donde <<CONDICION>>;" + vbNewLine
    End Sub
End Class
