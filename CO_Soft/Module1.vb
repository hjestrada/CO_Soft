


Option Strict Off
Option Explicit On
Imports System.Text.RegularExpressions

Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.IO
Imports System.Data.OleDb

Module Module1
    'Public DB_Path As String = "Data Source=" & Application.StartupPath & "\fermentador_memo.s3db;"
    Public DB_Path As String = "Data Source=fermentador_memo.s3db;"
    Public SQLiteCon As New SQLiteConnection(DB_Path)
    Public SQLliteCMD As New SQLiteCommand
    Public SQLiteDA As New SQLiteDataAdapter
    Public consulta As String
    Public dataSet As DataSet
    Public lista As Byte

    Public pH As String
    Public Temp As String
    Public CO2 As String


    Public estado As Boolean = False
    Public comoviene As String




End Module
