Imports System.IO

Public Class Form1

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

        Dim val As String = Convert.ToString(DateTime.Today - DateTime.Now)

        tab_log = tab_inicial.TabPages.Item(0)
        tab_ide = tab_inicial.TabPages.Item(1)

        tab_inicial.TabPages.Remove(tab_ide)
    End Sub




    'If Me.RichTextBox1.Text <> "" Then
    '        MyParser.Setup()
    '        Me.RichTextBox2.Text = MyParser.Parse(New StringReader(Me.RichTextBox1.Text)).ToString()

    'Dim arbol As Ini = MyParser.Parser.CurrentReduction
    'Else
    '        MessageBox.Show("Ingrese una cadena valida", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If



End Class
