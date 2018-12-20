Imports System.Net.Sockets
Imports ServidorWeb

Public Class Ini
    Implements Instruccion

    Private _validar As String
    Private _package As Paquete
    Public connect As Conexion
    Public socket As Socket

    Public Sub New(ByVal validar As Integer, ByVal package As Paquete)
        Me.Validar = validar
        Me.Package = package
    End Sub

    Public Property Package As Paquete
        Get
            Return _package
        End Get
        Set(value As Paquete)
            _package = value
        End Set
    End Property

    Public Property Validar As String
        Get
            Return _validar
        End Get
        Set(value As String)
            _validar = value
        End Set
    End Property

    Public Function ejecutar() As Object Implements Instruccion.ejecutar
        ''aqui debo de enviar los datos por subpaquetes solamente
        ''debo enviar el validar despues miro que mas enviar
        Dim retorno As Object = Nothing

        If Not (socket Is Nothing) And Not (Package Is Nothing) And Not (Validar Is Nothing) Then
            If Package.Tipo_paquete = Paquete.FIN Then
                ''como me confundi el paquete fin no tiene que enviar validar para su proceso
                ''entonces solo debo ejecutar el paquete
                ''solo que este si retornara un valor
                Package.connect = connect
                Package.socket = socket
                retorno = Package.ejecutar()
            Else
                ''estoy enviando el validar
                connect.Enviar("validar~" + Validar, socket)
                ''dejo que lo procese por intervalor pequeño de tiempo
                Threading.Thread.Sleep(100)

                ''ahora debo enviar el paquete completo
                Package.connect = connect
                Package.socket = socket
                Package.ejecutar()
            End If

        End If
        Return retorno
    End Function

    Public Function responder() As Object Implements Instruccion.responder
        Return Package.responder()
    End Function
End Class
