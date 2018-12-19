﻿'Generated by the GOLD Parser Builder

Option Explicit On
Option Strict Off

Imports System.IO
Imports System.Windows.Forms


Module MyParser
    Public Parser As New GOLD.Parser

    Private Enum SymbolIndex
        [Eof] = 0                                 ' (EOF)
        [Error] = 1                               ' (Error)
        [Whitespace] = 2                          ' Whitespace
        [Quotedatosquote] = 3                     ' '"datos"'
        [Quoteerrorquote] = 4                     ' '"error"'
        [Quotefinquote] = 5                       ' '"fin"'
        [Quoteinstruccionquote] = 6               ' '"instruccion"'
        [Quoteloginquote] = 7                     ' '"login"'
        [Quotemsgquote] = 8                       ' '"msg"'
        [Quotepaquetequote] = 9                   ' '"paquete"'
        [Quotepasswordquote] = 10                 ' '"password"'
        [Quotereportequote] = 11                  ' '"reporte"'
        [Quotetipoquote] = 12                     ' '"tipo"'
        [Quoteusernamequote] = 13                 ' '"username"'
        [Quoteusqlquote] = 14                     ' '"usql"'
        [Quotevalidarquote] = 15                  ' '"validar"'
        [Comma] = 16                              ' ','
        [Colon] = 17                              ' ':'
        [Lbracket] = 18                           ' '['
        [Rbracket] = 19                           ' ']'
        [Double_] = 20                            ' 'double_'
        [False] = 21                              ' false
        [Inst] = 22                               ' inst
        [Integer] = 23                            ' integer
        [Text] = 24                               ' text
        [True] = 25                               ' true
        [Booleano] = 26                           ' <BOOLEANO>
        [Campo] = 27                              ' <CAMPO>
        [Campos] = 28                             ' <CAMPOS>
        [Datos] = 29                              ' <DATOS>
        [Fila] = 30                               ' <FILA>
        [Filas] = 31                              ' <FILAS>
        [Ini] = 32                                ' <INI>
        [Paquete] = 33                            ' <PAQUETE>
        [Paquete_error] = 34                      ' <PAQUETE_ERROR>
        [Paquete_fin] = 35                        ' <PAQUETE_FIN>
        [Paquete_inst] = 36                       ' <PAQUETE_INST>
        [Paquete_login] = 37                      ' <PAQUETE_LOGIN>
        [Paquete_reporte] = 38                    ' <PAQUETE_REPORTE>
    End Enum

    Private Enum ProductionIndex
        [Ini_Lbracket_Quotevalidarquote_Colon_Integer_Comma_Rbracket] = 0 ' <INI> ::= '[' '"validar"' ':' integer ',' <PAQUETE> ']'
        [Paquete] = 1                             ' <PAQUETE> ::= <PAQUETE_LOGIN>
        [Paquete2] = 2                            ' <PAQUETE> ::= <PAQUETE_FIN>
        [Paquete3] = 3                            ' <PAQUETE> ::= <PAQUETE_ERROR>
        [Paquete4] = 4                            ' <PAQUETE> ::= <PAQUETE_INST>
        [Paquete5] = 5                            ' <PAQUETE> ::= <PAQUETE_REPORTE>
        [Paquete_reporte_Quotepaquetequote_Colon_Quotereportequote_Comma_Quoteinstruccionquote_Colon_Inst] = 6 ' <PAQUETE_REPORTE> ::= '"paquete"' ':' '"reporte"' ',' '"instruccion"' ':' inst
        [Paquete_reporte_Quotepaquetequote_Colon_Quotereportequote_Comma_Quotedatosquote_Colon] = 7 ' <PAQUETE_REPORTE> ::= '"paquete"' ':' '"reporte"' ',' '"datos"' ':' <DATOS>
        [Paquete_error_Quotepaquetequote_Colon_Quoteerrorquote_Comma_Quotetipoquote_Colon_Text_Comma_Quotemsgquote_Colon_Text_Comma_Quotedatosquote_Colon] = 8 ' <PAQUETE_ERROR> ::= '"paquete"' ':' '"error"' ',' '"tipo"' ':' text ',' '"msg"' ':' text ',' '"datos"' ':' <FILAS>
        [Paquete_login_Quoteloginquote_Colon_Lbracket_Quoteusernamequote_Colon_Text_Comma_Quotepasswordquote_Colon_Text_Rbracket] = 9 ' <PAQUETE_LOGIN> ::= '"login"' ':' '[' '"username"' ':' text ',' '"password"' ':' text ']'
        [Paquete_login_Quoteloginquote_Colon_Lbracket_Quoteusernamequote_Colon_Text_Comma_Quoteloginquote_Colon_Rbracket] = 10 ' <PAQUETE_LOGIN> ::= '"login"' ':' '[' '"username"' ':' text ',' '"login"' ':' <BOOLEANO> ']'
        [Booleano_True] = 11                      ' <BOOLEANO> ::= true
        [Booleano_False] = 12                     ' <BOOLEANO> ::= false
        [Paquete_fin_Quotepaquetequote_Colon_Quotefinquote] = 13 ' <PAQUETE_FIN> ::= '"paquete"' ':' '"fin"'
        [Paquete_inst_Quotepaquetequote_Colon_Quoteusqlquote_Comma_Quoteinstruccionquote_Colon_Inst] = 14 ' <PAQUETE_INST> ::= '"paquete"' ':' '"usql"' ',' '"instruccion"' ':' inst
        [Paquete_inst_Quotepaquetequote_Colon_Quoteusqlquote_Comma_Quotedatosquote_Colon] = 15 ' <PAQUETE_INST> ::= '"paquete"' ':' '"usql"' ',' '"datos"' ':' <DATOS>
        [Datos_Lbracket_Rbracket] = 16            ' <DATOS> ::= '[' <FILAS> ']'
        [Datos_Lbracket_Text_Rbracket] = 17       ' <DATOS> ::= '[' text ']'
        [Datos_Lbracket_Inst_Rbracket] = 18       ' <DATOS> ::= '[' inst ']'
        [Filas_Comma] = 19                        ' <FILAS> ::= <FILAS> ',' <FILA>
        [Filas] = 20                              ' <FILAS> ::= <FILA>
        [Fila_Lbracket_Rbracket] = 21             ' <FILA> ::= '[' <CAMPOS> ']'
        [Campos_Comma] = 22                       ' <CAMPOS> ::= <CAMPOS> ',' <CAMPO>
        [Campos] = 23                             ' <CAMPOS> ::= <CAMPO>
        [Campo_Text_Colon_Text] = 24              ' <CAMPO> ::= text ':' text
        [Campo_Text_Colon_Integer] = 25           ' <CAMPO> ::= text ':' integer
        [Campo_Text_Colon_Double_] = 26           ' <CAMPO> ::= text ':' 'double_'
    End Enum

    Public Program As Object     'You might derive a specific object

    Public Sub Setup()
        'This procedure can be called to load the parse tables. The class can
        'read tables using a BinaryReader.

        Parser.LoadTables(Path.Combine(Application.StartupPath, "GramaticaVB.egt"))
    End Sub
    
    Public Function Parse(ByVal Reader As TextReader) As Boolean
        'This procedure starts the GOLD Parser Engine and handles each of the
        'messages it returns. Each time a reduction is made, you can create new
        'custom object and reassign the .CurrentReduction property. Otherwise, 
        'the system will use the Reduction object that was returned.
        '
        'The resulting tree will be a pure representation of the language 
        'and will be ready to implement.

        Dim Response As GOLD.ParseMessage
        Dim Done as Boolean                  'Controls when we leave the loop
        Dim Accepted As Boolean = False      'Was the parse successful?

        Accepted = False    'Unless the program is accepted by the parser

        Parser.Open(Reader)
        Parser.TrimReductions = False  'Please read about this feature before enabling  

        Done = False
        Do Until Done
            Response = Parser.Parse()

            Select Case Response              
                Case GOLD.ParseMessage.LexicalError
                    'Cannot recognize token
                    Done = True

                Case GOLD.ParseMessage.SyntaxError
                    'Expecting a different token
                    Done = True

                Case GOLD.ParseMessage.Reduction
                    'Create a customized object to store the reduction
                    Parser.CurrentReduction = CreateNewObject(Parser.CurrentReduction)

                Case GOLD.ParseMessage.Accept
                    'Accepted!
                    'Program = Parser.CurrentReduction  'The root node!                 
                    Done = True
                    Accepted = True

                Case GOLD.ParseMessage.TokenRead
                    'You don't have to do anything here.

                Case GOLD.ParseMessage.InternalError
                    'INTERNAL ERROR! Something is horribly wrong.
                    Done = True

                Case GOLD.ParseMessage.NotLoadedError
                    'This error occurs if the CGT was not loaded.                   
                    Done = True

                Case GOLD.ParseMessage.GroupError 
                    'COMMENT ERROR! Unexpected end of file
                    Done = True
            End Select
        Loop

        Return Accepted
    End Function

    Private Function CreateNewObject(Reduction as GOLD.Reduction) As Object
        Dim Result As Object = Nothing

        With Reduction
            Select Case .Parent.TableIndex                        
                Case ProductionIndex.Ini_Lbracket_Quotevalidarquote_Colon_Integer_Comma_Rbracket
                    ' <INI> ::= '[' '"validar"' ':' integer ',' <PAQUETE> ']' 

                    Dim init As Ini = New Ini(.Item(3).Data.ToString(), .Item(5).Data)
                    Result = init

                Case ProductionIndex.Paquete
                    ' <PAQUETE> ::= <PAQUETE_LOGIN> 

                    Result = .Item(0).Data

                Case ProductionIndex.Paquete2
                    ' <PAQUETE> ::= <PAQUETE_FIN> 

                    Result = .Item(0).Data

                Case ProductionIndex.Paquete3
                    ' <PAQUETE> ::= <PAQUETE_ERROR> 

                    Result = .Item(0).Data

                Case ProductionIndex.Paquete4
                    ' <PAQUETE> ::= <PAQUETE_INST> 

                    Result = .Item(0).Data

                Case ProductionIndex.Paquete5
                    ' <PAQUETE> ::= <PAQUETE_REPORTE> 

                    Result = .Item(0).Data

                Case ProductionIndex.Paquete_reporte_Quotepaquetequote_Colon_Quotereportequote_Comma_Quoteinstruccionquote_Colon_Inst
                    ' <PAQUETE_REPORTE> ::= '"paquete"' ':' '"reporte"' ',' '"instruccion"' ':' inst 

                    Dim instruccion As String = .Item(6).Data.ToString().Trim("~")
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.REPORTE)
                    npaquete.Instruccion = instruccion
                    npaquete.Line = line
                    npaquete.Colm = colm
                    Result = npaquete

                Case ProductionIndex.Paquete_reporte_Quotepaquetequote_Colon_Quotereportequote_Comma_Quotedatosquote_Colon
                    ' <PAQUETE_REPORTE> ::= '"paquete"' ':' '"reporte"' ',' '"datos"' ':' <DATOS> 

                    Dim reporte As String = Nothing
                    Dim obj As Object = .Item(6).Data

                    If TypeOf obj Is String Then
                        reporte = obj.ToString()
                    End If

                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.REPORTE)
                    npaquete.Report = reporte
                    npaquete.Line = line
                    npaquete.Colm = colm
                    Result = npaquete

                Case ProductionIndex.Paquete_error_Quotepaquetequote_Colon_Quoteerrorquote_Comma_Quotetipoquote_Colon_Text_Comma_Quotemsgquote_Colon_Text_Comma_Quotedatosquote_Colon
                    ' <PAQUETE_ERROR> ::= '"paquete"' ':' '"error"' ',' '"tipo"' ':' text ',' '"msg"' ':' text ',' '"datos"' ':' <FILAS> 

                    Dim tipo_error As String = .Item(6).Data.ToString().Trim("""")
                    Dim mensaje As String = .Item(10).Data.ToString().Trim("""")
                    Dim filas As List(Of List(Of Campo)) = Nothing

                    Dim obj As Object = .Item(14).Data

                    If TypeOf obj Is List(Of List(Of Campo)) Then
                        filas = obj
                    End If

                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.ERRORR)
                    npaquete.Tipo_error = tipo_error
                    npaquete.Msg = mensaje
                    npaquete.Filas = filas
                    npaquete.Line = line
                    npaquete.Colm = colm
                    Result = npaquete

                Case ProductionIndex.Paquete_login_Quoteloginquote_Colon_Lbracket_Quoteusernamequote_Colon_Text_Comma_Quotepasswordquote_Colon_Text_Rbracket
                    ' <PAQUETE_LOGIN> ::= '"login"' ':' '[' '"username"' ':' text ',' '"password"' ':' text ']' 

                    Dim username As String = .Item(5).Data.ToString().Trim("""")
                    Dim password As String = .Item(9).Data.ToString().Trim("""")
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.LOGIN)
                    npaquete.Username = username
                    npaquete.Password = password
                    npaquete.Line = line
                    npaquete.Colm = colm
                    Result = npaquete

                Case ProductionIndex.Paquete_login_Quoteloginquote_Colon_Lbracket_Quoteusernamequote_Colon_Text_Comma_Quoteloginquote_Colon_Rbracket
                    ' <PAQUETE_LOGIN> ::= '"login"' ':' '[' '"username"' ':' text ',' '"login"' ':' <BOOLEANO> ']' 

                    Dim username As String = .Item(5).Data.ToString().Trim("""")
                    Dim log As Boolean = .Item(9).Data
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.LOGIN)
                    npaquete.Username = username
                    npaquete.Log = log
                    npaquete.Line = line
                    npaquete.Colm = colm
                    Result = npaquete

                Case ProductionIndex.Booleano_True
                    ' <BOOLEANO> ::= true 

                    Result = True

                Case ProductionIndex.Booleano_False
                    ' <BOOLEANO> ::= false

                    Result = False

                Case ProductionIndex.Paquete_fin_Quotepaquetequote_Colon_Quotefinquote
                    ' <PAQUETE_FIN> ::= '"paquete"' ':' '"fin"' 

                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.FIN)
                    npaquete.Line = line
                    npaquete.Colm = colm

                    Result = npaquete

                Case ProductionIndex.Paquete_inst_Quotepaquetequote_Colon_Quoteusqlquote_Comma_Quoteinstruccionquote_Colon_Inst
                    ' <PAQUETE_INST> ::= '"paquete"' ':' '"usql"' ',' '"instruccion"' ':' inst 

                    Dim instruccion As String = .Item(6).Data.ToString().Trim("~")
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.INST)
                    npaquete.Instruccion = instruccion
                    npaquete.Line = line
                    npaquete.Colm = colm

                    Result = npaquete

                Case ProductionIndex.Paquete_inst_Quotepaquetequote_Colon_Quoteusqlquote_Comma_Quotedatosquote_Colon
                    ' <PAQUETE_INST> ::= '"paquete"' ':' '"usql"' ',' '"datos"' ':' <DATOS> 

                    Dim obj As Object = .Item(6).Data

                    Dim filas As List(Of List(Of Campo)) = Nothing
                    Dim respuesa As String = Nothing

                    If TypeOf obj Is List(Of List(Of Campo)) Then
                        filas = obj
                    Else
                        respuesa = obj.ToString().Trim("""").Trim("~")
                    End If

                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column

                    Dim npaquete As Paquete = New Paquete(Paquete.INST)
                    npaquete.Respuesta = respuesa
                    npaquete.Filas = filas
                    npaquete.Line = line
                    npaquete.Colm = colm

                    Result = npaquete

                Case ProductionIndex.Datos_Lbracket_Rbracket
                    ' <DATOS> ::= '[' <FILAS> ']' 

                    Result = .Item(1).Data

                Case ProductionIndex.Datos_Lbracket_Text_Rbracket
                    ' <DATOS> ::= '[' text ']' 

                    Result = .Item(1).Data.ToString().Trim("""")

                Case ProductionIndex.Datos_Lbracket_Inst_Rbracket
                    ' <DATOS> ::= '[' inst ']' 

                    Result = .Item(1).Data.ToString().Trim("~")

                Case ProductionIndex.Filas_Comma
                    ' <FILAS> ::= <FILAS> ',' <FILA> 

                    Dim lista As List(Of List(Of Campo)) = .Item(0).Data
                    Dim campo As List(Of Campo) = .Item(2).Data
                    lista.Add(campo)
                    Result = lista

                Case ProductionIndex.Filas
                    ' <FILAS> ::= <FILA> 

                    Dim lista As List(Of List(Of Campo)) = New List(Of List(Of Campo))
                    lista.Add(.Item(0).Data)
                    Result = lista

                Case ProductionIndex.Fila_Lbracket_Rbracket
                    ' <FILA> ::= '[' <CAMPOS> ']' 

                    Result = .Item(1).Data

                Case ProductionIndex.Campos_Comma
                    ' <CAMPOS> ::= <CAMPOS> ',' <CAMPO> 

                    Dim lista As List(Of Campo) = .Item(0).Data
                    Dim campo As Campo = .Item(2).Data
                    lista.Add(campo)
                    Result = lista

                Case ProductionIndex.Campos
                    ' <CAMPOS> ::= <CAMPO>

                    Dim lista As List(Of Campo) = New List(Of Campo)
                    lista.Add(.Item(0).Data)

                    Result = lista

                Case ProductionIndex.Campo_Text_Colon_Text
                    ' <CAMPO> ::= text ':' text 

                    Dim nombre As String = .Item(0).Data.ToString().Trim("""")
                    Dim valor As String = .Item(2).Data.ToString().Trim("""")
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column
                    Dim ncampo As Campo = New Campo(Campo.CADENA, nombre, valor, line, colm)

                    Result = ncampo

                Case ProductionIndex.Campo_Text_Colon_Integer
                    ' <CAMPO> ::= text ':' integer 
                    Dim nombre As String = .Item(0).Data.ToString().Trim("""")
                    Dim valor As String = .Item(2).Data.ToString()
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column
                    Dim ncampo As Campo = New Campo(Campo.NUMERO, nombre, valor, line, colm)

                    Result = ncampo
                Case ProductionIndex.Campo_Text_Colon_Double_
                    ' <CAMPO> ::= text ':' 'double_' 
                    Dim nombre As String = .Item(0).Data.ToString().Trim("""")
                    Dim valor As String = .Item(2).Data.ToString()
                    Dim line As Integer = .Item(0).Position().Line
                    Dim colm As Integer = .Item(0).Position().Column
                    Dim ncampo As Campo = New Campo(Campo.DOBLE, nombre, valor, line, colm)

                    Result = ncampo
            End Select
        End With     

        Return Result
    End Function
End Module