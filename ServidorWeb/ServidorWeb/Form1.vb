Imports System.IO

Public Class Form1
    Private Sub btn_iniciar_Click(sender As Object, e As EventArgs) Handles btn_iniciar.Click
        If Me.RichTextBox1.Text <> "" Then
            MyParser.Setup()
            Me.RichTextBox2.Text = MyParser.Parse(New StringReader(Me.RichTextBox1.Text)).ToString()

            Dim arbol As Ini = MyParser.Parser.CurrentReduction
        Else
            MessageBox.Show("Ingrese una cadena valida", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class
