﻿Imports System.Net.Sockets
Imports ServidorWeb

Public Class Paquete
    Implements Instruccion
    Public connect As Conexion
    Public socket As Socket

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

        If Tipo_paquete = LOGIN Then
            ''proceso el envio del login
            connect.Enviar("paquete~login", socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
            ''envio el usario
            connect.Enviar("usuario~" + Username, socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
            ''envio password
            connect.Enviar("password~" + Username, socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
            ''por lo tanto se termino el envio
        ElseIf Tipo_paquete = INST Then
            ''proceso el envio del usql
            connect.Enviar("paquete~usql", socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
            ''proceso el envio de l instruccion
            connect.Enviar("instruccion~" + Instruccion, socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
        ElseIf Tipo_paquete = FIN Then
            ''proceso el envio fin
            connect.Enviar("paquete~fin", socket)
            ''dejo que lo procese por intervalor pequeño de tiempo
            Threading.Thread.Sleep(Conexion.DELAY)
            ''solo que aqui debo retornar el valor devuelto por el sistema
        End If

        Return Nothing
    End Function

    Public Function responder() As Object Implements Instruccion.responder
        If Tipo_paquete = LOGIN Then
            Return Log
        ElseIf Tipo_paquete = INST Then
            If Filas Is Nothing Then
                Return Respuesta
            Else
                'padar los datos de la lista a un datatable
                Return ObtenerInfo_Select()
            End If
        ElseIf Tipo_paquete = ERRORR Then
            ''aqui siempre debo de devolver un datatable
            Return ObtenerInfo_errores()
        End If

        ''Aqui voy porfavor este siempre me esta devolviendo un false por que no he puesto una respuesta
        '' ojo  devo realizar todas las respuestas de parte del servidor db

        Return False
    End Function

    Public Function ObtenerInfo_errores() As Object
        Dim dt As DataTable = New DataTable("Error")

        ''en este caso debo de buscar las columnas si existe
        For i = 0 To Filas.Count - 1
            For j = 0 To Filas(i).Count - 1
                ''obteniedo y creando las columnas
                Dim dc As DataColumn = New DataColumn(Filas(i)(j).Nombre)
                dt.Columns.Add(dc)
            Next
            Exit For
        Next

        ''ahora debo ingresar todas las filas
        For i = 0 To Filas.Count - 1
            Dim dr As DataRow = dt.NewRow()

            For j = 0 To Filas(i).Count - 1
                ''ingresando valores
                dr(Filas(i)(j).Nombre) = Filas(i)(j).Valor
            Next

            dt.Rows.Add(dr)
        Next
        Return dt
    End Function


    Public Function ObtenerInfo_Select() As Object
        Dim dt As DataTable = New DataTable("Tabla")

        'debo buscar en la primera fila de la lista
        'las columnas a agregar en el datatable
        'despues ya realizar el recorrido normal

        ''en esta ocasion las columnas siempre viene por lo menos 1

        For i = 0 To Filas.Count - 1
            For j = 0 To Filas(i).Count - 1
                ''obteniedo y creando las columnas
                Dim dc As DataColumn = New DataColumn(Filas(i)(j).Nombre)
                dt.Columns.Add(dc)
            Next
            Exit For
        Next

        ''ahora debo ingresar todas las filas
        For i = 0 To Filas.Count - 1
            Dim dr As DataRow = dt.NewRow()

            For j = 0 To Filas(i).Count - 1
                ''ingresando valores
                dr(Filas(i)(j).Nombre) = Filas(i)(j).Valor
            Next

            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
End Class
