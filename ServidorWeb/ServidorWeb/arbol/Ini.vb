Imports ServidorWeb

Public Class Ini
    Implements Instruccion

    Private _validar As String
    Private _package As Paquete

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
        Throw New NotImplementedException()
    End Function
End Class
