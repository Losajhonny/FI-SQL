Imports System.IO
Imports System.Net.Sockets

Public Class Logica

    Public Nothingg As String = "_nothing_"
    Public connect As Conexion = New Conexion()
    Public socket As Socket

    Public numAleatorio As New Random()

    Public Function paquete_Instruccion(ByVal instrucciones As String) As Object
        Dim cadena_envio As String = ""

        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''
        cadena_envio += "[ ""validar"": " + numAleatorio.Next.ToString() + "," +
            " ""paquete"" : ""usql"" , ""instruccion"" : ~" + instrucciones + "~ ]"

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio))

        ''obtengo el paquete de usql
        Dim paquete_inicio As Ini = MyParser.Parser.CurrentReduction

        ''debo abrir la conexion
        Dim socket As Socket = connect.Connect()

        ''envio conexion al paquete usql para la ejecucion 
        paquete_inicio.connect = connect
        paquete_inicio.socket = socket

        paquete_inicio.ejecutar()
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''
        ''envio el paquete de finalizar para proceso
        cadena_envio = "[ ""validar"": " + numAleatorio.Next.ToString() + "," +
            " ""paquete"" : ""fin"" ]"

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio)).ToString()
        ''info al servidor db que termine escuchar y comienze el proceso
        Dim paquete_fin As Ini = MyParser.Parser.CurrentReduction

        paquete_fin.connect = connect
        paquete_fin.socket = socket
        ''este paquete me retornara el resultao del servidor wev que seria una cadena en lenguaje plycs
        Dim resultado1 As String = paquete_fin.ejecutar()
        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''

        'resultado guarda solo una peticion de recibir pues en este caso que estoy en paquete instruccion
        'debo realizar otra peticion de recibir debido a que necesito lo siguiente

        '1. En caso de haber realizado un select necesito de los datos
        '2. Necesito los resultados de print de una base de datos
        ''son dos peticiones que hay que tomar en cuenta

        'como resultado ya trae la peticion de un select
        'entonces debo pedir el de print de la base de datos

        ''dejo que lo procese por intervalor pequeño de tiempo
        Threading.Thread.Sleep(100)
        ''solo que aqui debo retornar el valor devuelto por el sistema
        Dim resultado2 As String = connect.Recibir(socket)

        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''
        Dim respuesta As Object = Nothing

        If Not (resultado1 Is Nothing And resultado2 Is Nothing) Then
            'Puede que me de error por que no le envio nada de resepuesta entonces resultado va a ser ""
            ''por lo tannto me tiraria error por que analizao el paquete de respuesata

            ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''
            ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''
            ''''''''''''' ANALIZANDO LA RESPUESTA 1 '''''''''''''''''

            If Not resultado1.Equals(Nothingg) Then
                If Not resultado1.Equals("") Then
                    ''debo de utilizar el lenguaje para que me retorne el resultado
                    MyParser.Setup()
                    MyParser.Parse(New StringReader(resultado1))

                    ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
                    ''ahora debo de ejecutar

                    Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
                    respuesta = paquete_respuesta.responder()

                    ''verificar si viene de la misma validacion
                    If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
                        respuesta = Nothing
                    End If
                End If
            End If

            If Not (resultado2.Equals(Nothingg)) Then
                If Not resultado2.Equals("") Then
                    ''debo de utilizar el lenguaje para que me retorne el resultado
                    MyParser.Setup()
                    MyParser.Parse(New StringReader(resultado2))

                    ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
                    ''ahora debo de ejecutar

                    Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
                    respuesta = paquete_respuesta.responder()

                    ''verificar si viene de la misma validacion
                    If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
                        respuesta = Nothing
                    End If
                End If
            End If





            '''''''''''''''DEBO REALIZAR LOS METODOS DE RESPUESTA PARA RETORNA LOS VALORES DE
            '''''''''''''''RESULTADO 1 Y RESULTAD2
            ''''''''''''''RESULTADO1 = SELECT
            ''''''''''''''RESULTADO2 = IMPRIMIR DEL USQL






        End If
        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''

        ''debo cerrar la conexion
        connect.Disconect(socket)

        ''por el momento retorno resultado1 y resultado 2
        Return ("resultado1" + resultado1 + "\n" + "resultado2" + resultado2)
    End Function

    Public Function inicio_sesion(ByVal usuario As String, ByVal password As String) As Boolean
        Dim cadena_envio As String = ""
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''
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
        '''''''''''''''''''''''' ENVIANDO VALIDAR ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''
        ''envio el paquete de finalizar para proceso
        cadena_envio = "[ ""validar"": " + numAleatorio.Next.ToString() + "," +
            " ""paquete"" : ""fin"" ]"

        MyParser.Setup()
        MyParser.Parse(New StringReader(cadena_envio)).ToString()
        Dim paquete_fin As Ini = MyParser.Parser.CurrentReduction

        paquete_fin.connect = connect
        paquete_fin.socket = socket
        Dim resultado As String = paquete_fin.ejecutar()
        '''''''''''''''''''''''' ENVIANDO FIN ''''''''''''''''''''''''''''''''''''''


        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''
        ''fin siempre retornara una cadena vacia o pero siempre debo verifiacar que no sea nulo
        Dim aceptado As Boolean = False

        If Not resultado Is Nothing Then
            If Not (resultado.Equals(Nothingg)) Then
                If Not resultado.Equals("") Then
                    MyParser.Setup()
                    MyParser.Parse(New StringReader(resultado))

                    ''entonces debo mostrar al usuario pero debo de utilizar el lenguaje
                    ''ahora debo de ejecutar

                    Dim paquete_respuesta As Ini = MyParser.Parser.CurrentReduction
                    aceptado = paquete_respuesta.responder()

                    ''verificar si viene de la misma validacion
                    If Not paquete_inicio.Validar.Equals(paquete_respuesta.Validar) Then
                        aceptado = False
                    End If
                End If
            End If
        End If
        '''''''''''''''''''''''' RETORNANDO RESPUESTA ''''''''''''''''''''''''''''''''''''''

        ''debo cerrar la conexion
        connect.Disconect(socket)

        Return aceptado
    End Function
End Class
