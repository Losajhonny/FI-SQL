Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class Logica

    Public Nothingg As String = "_nothing_"
    Public connect As Conexion = New Conexion()
    Public socket As Socket

    Public log As String = ""
    Public log2 As String = ""
    Public consola As String = ""
    Public tabla As DataTable
    Public errorr As DataTable

    Public numAleatorio As New Random()

    Public Function paquete_Instruccion(ByVal instrucciones As String) As Object
        Dim socket As Socket = connect.Connect()    ''creo una conexion

        log = ""
        consola = ""

        Dim logtemp As String = ""
        Dim cadena_envio As String = ""
        Dim respuesta_error As String = Nothingg
        Dim respuesta_select As String = Nothingg
        Dim respuesta_log As String = Nothingg
        Dim respuesta_consola As String = Nothingg
        Dim aleatorio As String = numAleatorio.Next.ToString()
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''
        logtemp += "Enviando Paquete:usql" + vbNewLine
        cadena_envio += "[ ""validar"": " + aleatorio + "," +
            " ""paquete"" : ""usql"" , ""instruccion"" : ~" + instrucciones + "~ ]"
        logtemp += cadena_envio + vbNewLine + vbNewLine

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio))

        ''ejecuto el paquete usql
        Dim paquete_inicio As Ini = MyParser.Parser.CurrentReduction

        Try
            ''envio conexion al paquete usql para la ejecucion 
            paquete_inicio.connect = connect
            paquete_inicio.socket = socket

            paquete_inicio.ejecutar()
        Catch ex As Exception
        End Try
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''
        ''envio el paquete de finalizar para proceso
        logtemp += "Enviando Paquete:fin" + vbNewLine
        cadena_envio = "[ ""validar"": " + aleatorio + "," + """paquete"" : ""fin"" ]"
        logtemp += cadena_envio + vbNewLine + vbNewLine
        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio)).ToString()
        Dim paquete_fin As Ini = MyParser.Parser.CurrentReduction

        Try
            paquete_fin.connect = connect
            paquete_fin.socket = socket
            paquete_fin.ejecutar()
        Catch ex As Exception
        End Try
        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''

        'Debo de recibir varios datos de informacion
        '     1. Lista de Errores
        '     2. Informacion ultimo select que se realizo
        '     3. Log que se realizo


        ''''''''''''''''''''''''' RECIBIENDO VALORES ''''''''''''''''''''''''''''''''
        respuesta_error = connect.Recibir(socket)
        Thread.Sleep(100)

        respuesta_select = connect.Recibir(socket)
        Thread.Sleep(100)

        respuesta_log = connect.Recibir(socket)
        Thread.Sleep(100)

        respuesta_consola = connect.Recibir(socket)
        Thread.Sleep(100)


        ' LAS RESPUESTAS ME VIENEN EN FORMATO PLYCS DEBO EJECUTARLO EN EL ANALIZADOR PARA OBTENER RESULTAOS

        '''''''''''''''''''''''' RESPUESTA LISTA ERRORES ''''''''''''''''''''''''''''
        If Not respuesta_error Is Nothing Then
            If Not respuesta_error.Equals(Nothingg) And Not respuesta_error.Equals("") Then
                MyParser.Setup()
                MyParser.Parse(New StringReader(respuesta_error)).ToString()
                Dim resErr As Ini = MyParser.Parser.CurrentReduction
                If Not resErr Is Nothing Then
                    errorr = resErr.responder()
                End If
            End If
        End If

        '''''''''''''''''''''''' RESPUESTA INFORMACION SELECT '''''''''''''''''''''''
        If Not respuesta_select Is Nothing Then
            If Not respuesta_select.Equals(Nothingg) And Not respuesta_select.Equals("") Then
                MyParser.Setup()
                MyParser.Parse(New StringReader(respuesta_select)).ToString()
                Dim resSel As Ini = MyParser.Parser.CurrentReduction

                If Not resSel Is Nothing Then
                    tabla = resSel.responder()
                End If
            End If
        End If

        '''''''''''''''''''''''' RESPUESTA LOG ''''''''''''''''''''''''''''''''''''''
        If Not respuesta_log Is Nothing Then
            If Not respuesta_log.Equals(Nothingg) And Not respuesta_log.Equals("") Then
                MyParser.Setup()
                MyParser.Parse(New StringReader(respuesta_log)).ToString()
                Dim resLog As Ini = MyParser.Parser.CurrentReduction

                If aleatorio.Equals(resLog.Validar) Then
                    Try
                        log = resLog.responder().ToString()
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If

        '''''''''''''''''''''''' RESPUESTA CONSOLA ''''''''''''''''''''''''''''''''''''''
        If Not respuesta_consola Is Nothing Then
            If Not respuesta_consola.Equals(Nothingg) And Not respuesta_consola.Equals("") Then
                MyParser.Setup()
                MyParser.Parse(New StringReader(respuesta_consola)).ToString()
                Dim resCon As Ini = MyParser.Parser.CurrentReduction

                If aleatorio.Equals(resCon.Validar) Then
                    Try
                        consola = resCon.responder().ToString()
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If
        'If Not (resultado1 Is Nothing And resultado2 Is Nothing) Then
        '    'Puede que me de error por que no le envio nada de resepuesta entonces resultado va a ser ""
        '    ''por lo tannto me tiraria error por que analizao el paquete de respuesata

        '    ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''
        '    ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''
        '    ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''

        '    If Not resultado1.Equals(Nothingg) Then
        '        If Not resultado1.Equals("") Then
        '            ''debo de utilizar el lenguaje para que me retorne el resultado
        '            MyParser.Setup()
        '            MyParser.Parse(New StringReader(resultado1))

        '            ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
        '            ''ahora debo de ejecutar

        '            Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
        '            respuesta = paquete_respuesta.responder()

        '            ''verificar si viene de la misma validacion
        '            If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
        '                respuesta = Nothing
        '            End If
        '        End If
        '    End If

        '    If Not (resultado2.Equals(Nothingg)) Then
        '        If Not resultado2.Equals("") Then
        '            ''debo de utilizar el lenguaje para que me retorne el resultado
        '            MyParser.Setup()
        '            MyParser.Parse(New StringReader(resultado2))

        '            ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
        '            ''ahora debo de ejecutar

        '            Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
        '            respuesta = paquete_respuesta.responder()

        '            ''verificar si viene de la misma validacion
        '            If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
        '                respuesta = Nothing
        '            End If
        '        End If
        '    End If





        '    '''''''''''''''DEBO REALIZAR LOS METODOS DE RESPUESTA PARA RETORNA LOS VALORES DE
        '    '''''''''''''''RESULTADO 1 Y RESULTAD2
        '    ''''''''''''''RESULTADO1 = SELECT
        '    ''''''''''''''RESULTADO2 = IMPRIMIR DEL USQL






        'End If
        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''

        ''debo cerrar la conexion
        connect.Disconect(socket)
        log2 += logtemp
        ''por el momento retorno resultado1 y resultado 2
        Return Nothing
    End Function

    Public Function inicio_sesion(ByVal usuario As String, ByVal password As String) As Boolean
        Dim socket As Socket = connect.Connect() 'Creando una conexion
        Dim cadena_envio As String = ""          'Cadena que se utiliza para enviar
        Dim aleatorio As String = numAleatorio.Next.ToString()
        Dim logtemp As String = ""
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''
        logtemp += "Enviando Paquete:login" + vbNewLine
        cadena_envio += "[ ""validar"": " + aleatorio + "," + """login"" : [ ""username"" : """ + usuario + """, " + """password"" : """ + password + """] ]"
        logtemp += cadena_envio + vbNewLine + vbNewLine

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio))

        ''ejecuto el paquete de login
        Dim paquete_inicio As Ini = MyParser.Parser.CurrentReduction
        ''le mando todos los valores del paquete
        Try
            paquete_inicio.connect = connect
            paquete_inicio.socket = socket
            paquete_inicio.ejecutar()
        Catch ex As Exception
        End Try
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''
        ''envio el paquete de finalizar para proceso
        logtemp += "Enviando Paquete:fin" + vbNewLine
        cadena_envio = "[ ""validar"": " + aleatorio + "," + """paquete"" : ""fin"" ]"
        logtemp += cadena_envio + vbNewLine + vbNewLine

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio)).ToString()
        Dim paquete_fin As Ini = MyParser.Parser.CurrentReduction

        Try
            paquete_fin.connect = connect
            paquete_fin.socket = socket
            paquete_fin.ejecutar()
        Catch ex As Exception
        End Try
        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''
        Dim resultado As String = Me.Nothingg
        resultado = connect.Recibir(socket)     ''obteniendo la respuesta del servidor db
        Thread.Sleep(100)

        Dim aceptado As Boolean = False

        If Not resultado Is Nothing Then
            If Not (resultado.Equals(Nothingg)) And Not (resultado.Equals("")) Then
                MyParser.Setup()
                MyParser.Parse(New StringReader(resultado))

                ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
                ''ahora debo de ejecutar

                Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
                Try
                    aceptado = paquete_respuesta.responder()
                Catch ex As Exception
                End Try

                ''verificar si viene de la misma validacion
                If Not aleatorio.Equals(paquete_respuesta.Validar) Then
                    aceptado = False
                End If
            End If
        End If
        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''

        ''debo cerrar la conexion
        connect.Disconect(socket)

        Return aceptado
    End Function
End Class
