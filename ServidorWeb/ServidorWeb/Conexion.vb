Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System
Imports System.Text

Public Class Conexion
    Public Const MAX_VALUE As Integer = 8192
    Public Const DELAY As Integer = 50

    ''' <summary>
    ''' Funcion que conecta con el servidor y le envia una cadena al servidor
    ''' </summary>
    ''' <param name="cadena"></param>
    ''' <returns></returns>
    Public Function Connect() As Socket
        Try
            'Creo la conexion con el socket
            Dim cliente As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            'Punto donde se comunicara con el servidor
            Dim localEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Parse("127.0.0.1"), 6400)
            'Socket tomara el punto de comunicacion
            cliente.Connect(localEndPoint)
            'cliente.Listen(5)
            'Bytes de informacion en bytes para enviar los datos
            'Dim info(MAX_VALUE) As Byte
            ''Conversion de la cadena a bytes
            'info = Encoding.Default.GetBytes(cadena)
            ''enviando informacion al servidor
            'cliente.Send(info)
            'cliente.Close()
            Return cliente
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Enviar(ByVal cadena As String, ByVal cliente As Socket) As Integer
        Try
            Dim info(MAX_VALUE) As Byte
            'Conversion de la cadena a bytes
            info = Encoding.ASCII.GetBytes(cadena)
            'enviando informacion al servidor
            cliente.Send(info)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Recibir(ByVal cliente As Socket) As String
        Dim respuesta As String = ""
        Try

            Dim info(MAX_VALUE) As Byte
            'Conversion de la cadena a bytes
            Dim byteRec As Integer = cliente.Receive(info)

            respuesta = Encoding.Default.GetString(info, 0, byteRec)

        Catch ex As Exception
        End Try
        Return respuesta
    End Function

    Public Function Disconect(ByVal cliente As Socket) As Integer
        Try
            cliente.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
