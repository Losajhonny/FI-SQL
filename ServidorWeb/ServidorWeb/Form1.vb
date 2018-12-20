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
                MessageBox.Show("Bienvenido " + txt_usuario.Text, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txt_usuario.Text = ""
                txt_password.Text = ""

                tab_inicial.TabPages.Remove(tab_log)
                tab_inicial.TabPages.Add(tab_ide)
                tab_inicial.SelectedIndex = 0
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

        agregarTab()
    End Sub

    Private Sub btn_agregar_Click(sender As Object, e As EventArgs) Handles btn_agregar.Click
        agregarTab()
    End Sub

    Public Sub agregarTab()
        Dim tp As TabPage = New TabPage("Script " + (tabide.TabPages.Count + 1).ToString())

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

    Private Sub btn_ejescript_Click(sender As Object, e As EventArgs) Handles btn_ejescript.Click
        ''Envia el texto de todo el richtextbox

        Dim texto As RichTextBox = tabide.SelectedTab.Controls(1)
        Dim cadena As String = texto.Text

        ''aqui realizar la peticion de instruccion
        Dim respuesta As Object = logica.paquete_Instruccion(cadena)

        Me.RichTextBox1.Text = respuesta
    End Sub

    Private Sub btn_ejecutar_Click(sender As Object, e As EventArgs) Handles btn_ejecutar.Click
        Dim texto As RichTextBox = tabide.SelectedTab.Controls(1)
        Dim cadena As String = texto.SelectedText

        ''aqui realizar la peticion de instruccion

    End Sub



    'If Me.RichTextBox1.Text <> "" Then
    '        MyParser.Setup()
    '        Me.RichTextBox2.Text = MyParser.Parse(New StringReader(Me.RichTextBox1.Text)).ToString()

    'Dim arbol As Ini = MyParser.Parser.CurrentReduction
    'Else
    '        MessageBox.Show("Ingrese una cadena valida", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If



End Class
