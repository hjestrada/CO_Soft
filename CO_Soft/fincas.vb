
Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.IO

Imports System.Text.RegularExpressions






Public Class fincas


    'Necesarios para redondear formulario
    Public SD As Integer
    Public Declare Function GetClassLong Lib "user32" Alias "GetClassLongA" (Dt As IntPtr, UI As Integer) As Integer
    Public Declare Function GetDesktopWindow Lib "user32" () As Integer
    Public Declare Function SetClassLong Lib "user32" Alias "SetClassLongA" (Dt As IntPtr, IDF As Integer, IGT As Integer) As Integer
    Public Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (Wo As IntPtr, Ni As Integer, NK As Integer) As Integer


    Public Sub New()
        InitializeComponent()
        SuspendLayout()
        FormBorderStyle = FormBorderStyle.None
        Const CS_DROPSHADOW As Integer = 500000
        '----&H20000
        '----131072
        SD = SetWindowLong(Handle, -8, GetDesktopWindow())
        SetClassLong(Handle, -26, GetClassLong(Handle, -26) Or CS_DROPSHADOW)
        ResumeLayout(False)

    End Sub
    '----------------------------------------------------

    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Public Shared Function CreateRoundRectRgn(LR As Integer, TR As Integer, RR As Integer, BR As Integer, WE As Integer, HE As Integer) As IntPtr

    End Function

    Sub MAXID()

        SQLiteDA.SelectCommand = New SQLiteCommand
        SQLiteDA.SelectCommand.Connection = SQLiteCon
        SQLiteDA.SelectCommand.CommandText = "SELECT MAX(`id_finca`)  AS id FROM finca"
        SQLiteCon.Open()
        Dim valorDefecto As Integer = 1
        Dim ValorRetornado As Object = SQLiteDA.SelectCommand.ExecuteScalar()

        If ValorRetornado Is DBNull.Value Then
            Label6.Text = CStr(valorDefecto)
        Else
            Label6.Text = CStr(ValorRetornado) + 1
        End If
        SQLiteCon.Close()
    End Sub



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()

    End Sub


    Private Sub fincas_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Necesario para redondear formulario
        Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))
        MAXID()
        CargarcomboUsuario()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Panel2_Paint_1(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub


    Public Sub CargarcomboUsuario()
        Try
            SQLiteCon.Open()

            'Dim MySQLDA As New SQLiteDataAdapter("SELECT(`nombre_usu` || ' ' || `apellido_usu`) AS ALGO FROM usuario", SQLiteCon)

            Dim MySQLDA As New SQLiteDataAdapter("SELECT `id_usuario`,`nombre_usu` FROM usuario", SQLiteCon)

            Dim table As New DataTable
            MySQLDA.Fill(table)
            ComboBox1.DataSource = table
            ComboBox1.ValueMember = "id_usuario"
            ComboBox1.DisplayMember = "nombre_usu"

            ComboBox1.SelectedIndex = 0
            SQLiteCon.Close()

        Catch ex As Exception
            MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")

        Finally
            SQLiteCon.Close()
            SQLliteCMD = Nothing
        End Try
    End Sub
End Class