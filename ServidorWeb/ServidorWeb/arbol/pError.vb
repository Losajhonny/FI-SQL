Public Class pError

    Dim _tipo As String
    Dim _descripcion As String
    Dim _lexema As String
    Dim _line As Integer
    Dim _colm As Integer

    Public Sub New(ByVal tipo As String, ByVal descripcion As String, ByVal lexema As String, ByVal line As Integer, ByVal colm As Integer)
        Me.Tipo = tipo
        Me.Descripcion = descripcion
        Me.Lexema = lexema
        Me.Line = line
        Me.Colm = colm
    End Sub

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(value As String)
            _tipo = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(value As String)
            _descripcion = value
        End Set
    End Property

    Public Property Lexema As String
        Get
            Return _lexema
        End Get
        Set(value As String)
            _lexema = value
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
