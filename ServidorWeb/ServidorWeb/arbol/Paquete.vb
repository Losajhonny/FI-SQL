Imports ServidorWeb

Public Class Paquete
    Implements Instruccion

    ''constantes de la clase paquete
    Public Const LOGIN As Integer = 0
    Public Const FIN As Integer = 1
    Public Const ERRORR As Integer = 2
    Public Const INST As Integer = 3
    Public Const REPORTE As Integer = 4

    ''atributos de un paquete reporte
    Private _reporte As String


    ''atributos de un paquete de error
    Private _tipo_error As String
    Private _msg As String
    '->ademas que utiliza el atributo de filas del paquete de instrucciones



    ''atributos de un paquete de login
    Private _username As String
    Private _password As String
    Private _log As Boolean


    ''atributos de un paquete de instruccion
    Private _tipo_paquete As Integer
    Private _instruccion As String
    Private _filas As List(Of List(Of Campo))
    Private _respuesta As String


    ''informacion del parser
    Private _line As Integer
    Private _colm As Integer

    Public Sub New(ByVal tipo_paquete As Integer)
        Me.Tipo_paquete = tipo_paquete
    End Sub

    Public Property Instruccion As String
        Get
            Return _instruccion
        End Get
        Set(value As String)
            _instruccion = value
        End Set
    End Property

    Public Property Tipo_paquete As Integer
        Get
            Return _tipo_paquete
        End Get
        Set(value As Integer)
            _tipo_paquete = value
        End Set
    End Property

    Public Property Line As Integer
        Get
            Return _line
        End Get
        Set(value As Integer)
            _line = value
        End Set
    End Property

    Public Property Colm As Integer
        Get
            Return _colm
        End Get
        Set(value As Integer)
            _colm = value
        End Set
    End Property

    Public Property Respuesta As String
        Get
            Return _respuesta
        End Get
        Set(value As String)
            _respuesta = value
        End Set
    End Property

    Public Property Filas As List(Of List(Of Campo))
        Get
            Return _filas
        End Get
        Set(value As List(Of List(Of Campo)))
            _filas = value
        End Set
    End Property

    Public Property Username As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Public Property Log As Boolean
        Get
            Return _log
        End Get
        Set(value As Boolean)
            _log = value
        End Set
    End Property

    Public Property Tipo_error As String
        Get
            Return _tipo_error
        End Get
        Set(value As String)
            _tipo_error = value
        End Set
    End Property

    Public Property Msg As String
        Get
            Return _msg
        End Get
        Set(value As String)
            _msg = value
        End Set
    End Property

    Public Property Report As String
        Get
            Return _reporte
        End Get
        Set(value As String)
            _reporte = value
        End Set
    End Property

    Public Function ejecutar() As Object Implements Instruccion.ejecutar
        Throw New NotImplementedException()
    End Function
End Class
