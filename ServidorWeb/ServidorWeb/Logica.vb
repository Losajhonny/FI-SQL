Imports System.IO
Imports System.Net.Sockets

Public Class Logica
    Public connect As Conexion = New Conexion()
    Public socket As Socket

    Public numAleatorio As New Random()

    Public Function inicio_sesion(ByVal usuario As String, ByVal password As String) As Boolean
        Dim cadena_envio As String = ""

        cadena_envio += "[ ""validar"": " + numAleatorio.Next.ToString() + "," +
            " ""login"" : [ ""username"" : """ + usuario + """, " + """password"" : """ + password + """] ]"

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio))

        ''debo abrir la conexion
        Dim socket As Socket = connect.Connect()

        ''ejecuto el paquete de login
        Dim paquete_inicio As Ini = MyParser.Parser.CurrentReduction
        ''le mando todos los valores del paquete
        paquete_inicio.connect = connect
        paquete_inicio.socket = socket

        paquete_inicio.ejecutar()

        ''result no tiene nada porque no me retorna nada el ast
        cadena_envio = "[ ""validar"": " + numAleatorio.Next.ToString() + "," +
            " ""paquete"" : ""fin"" ]"

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio)).ToString()
        Dim paquete_fin As Ini = MyParser.Parser.CurrentReduction

        paquete_fin.connect = connect
        paquete_fin.socket = socket
        Dim resultado As String = paquete_fin.ejecutar()


        ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
        MyParser.Setup()
        MyParser.Parse(New StringReader(resultado))

        Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
        Dim aceptado As Boolean = paquete_respuesta.responder()
        ''ahora debo de ejecutar


        ''verificar si viene de la misma validacion
        If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
            aceptado = False
        End If
        ''debo cerrar la conexion
        connect.Disconect(socket)

        Return aceptado
    End Function
End Class
