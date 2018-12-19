Public Class Campo
    Public Const CADENA As Integer = 0
    Public Const DOBLE As Integer = 1
    Public Const NUMERO As Integer = 2

    Private _nombre As String
    Private _valor As String
    Private _line As Integer
    Private _colm As Integer
    Private _tipo As Integer

    Public Sub New(ByVal tipo As Integer, ByVal nombre_ As String,
                   ByVal valor_ As String, ByVal line_ As Integer,
                   ByVal colm_ As Integer
                    )
        Nombre = nombre_
        Valor = valor_
        Line = line_
        Colm = colm_
    End Sub

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Valor As String
        Get
            Return _valor
        End Get
        Set(value As String)
            _valor = value
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
End Class
