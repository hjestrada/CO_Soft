
Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.IO





Public Class variedad

    Dim obj2 As New Clase1


    Public auxconsulta, auxconsulta2


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



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()

    End Sub

    Private Sub variedad_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Necesario para redondear formulario
        Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))
        CargarcomboUsuario()
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        MAXID()
        consultaDGW()



    End Sub


    Sub consultaDGW()

        Dim Sql As String = "Select id_variedad, nombre_var FROM variedad where id_finca=" & ComboBox2.SelectedValue.ToString & ""
        obj2.consultaDGW(DataGridView1, Sql)


    End Sub
    Sub MAXID()

        SQLiteDA.SelectCommand = New SQLiteCommand
        SQLiteDA.SelectCommand.Connection = SQLiteCon
        SQLiteDA.SelectCommand.CommandText = "SELECT MAX(`id_variedad`)  AS id FROM variedad"
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





    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

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
            consultaDGW()
        Catch ex As Exception
            MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")

        Finally
            SQLiteCon.Close()
            SQLliteCMD = Nothing
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        cargarfincas()

    End Sub

    Sub cargarfincas()
        Dim aux_usuario = ComboBox1.SelectedValue.ToString

        Try

            Dim MySQLDA As New SQLiteDataAdapter("SELECT nombre_fin,id_finca FROM finca INNER JOIN usuario WHERE usuario.ID_USUARIO=@Gg and finca.id_usuario=@Gg", SQLiteCon)

            MySQLDA.SelectCommand.Parameters.AddWithValue("@Gg", aux_usuario)
            Dim ds As New DataSet
            Dim table As New DataTable

            MySQLDA.Fill(ds)

            ComboBox2.DataSource = ds.Tables(0)
            ComboBox2.DisplayMember = ds.Tables(0).Columns(0).Caption.ToString
            ComboBox2.ValueMember = "id_finca"


        Catch ex As Exception
            '  MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


        Finally
            SQLiteCon.Close()

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Trim(TextBox2.Text) = "" Then
            MsgBox("¡Error! no se permiten campos vacios")

        Else

            SQLiteCon.Close()
            Try
                SQLiteCon.Open()
                SQLliteCMD = New SQLiteCommand

                With SQLliteCMD
                    .CommandText = " INSERT INTO variedad (`id_variedad`, `nombre_var`,`id_finca`)
                                                           VALUES (NULL,@nombre_var,@id_finca)"
                    .Connection = SQLiteCon
                    .Parameters.AddWithValue("@nombre_var", Me.TextBox2.Text)
                    .Parameters.AddWithValue("@id_finca", ComboBox2.SelectedValue.ToString)
                    .Parameters.AddWithValue("@id_usuario", ComboBox1.SelectedValue.ToString)
                    .ExecuteNonQuery()
                    consultaDGW()

                End With

                SQLiteCon.Close()
                MsgBox("Datos Registrados Exitosamente")
                TextBox2.Clear()
                MAXID()

            Catch ex As Exception
                SQLiteCon.Close()
                '  MsgBox("Descripcion del error:" & ex.Message)
                Return
            End Try
            SQLiteCon.Close()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try

            Dim Numero As String
            Numero = InputBox("Por favor digite el consecutivo de la Variedad a eliminar")

            If String.IsNullOrEmpty(Numero) Then
                MessageBox.Show("Eliminación Cancelada")
                Return
            End If

            Dim result As DialogResult = MessageBox.Show("¿Seguro que desea eliminar este registro?, este proceso es irreversible y puede ocasionar perdidas de datos posteriores ", "Atención", MessageBoxButtons.YesNo)

            If (result = DialogResult.Yes) Then
                SQLiteCon.Open()
                SQLliteCMD = New SQLite.SQLiteCommand(" PRAGMA foreign_keys = ON;delete from variedad  where id_variedad='" & Numero & "'", SQLiteCon)


                SQLliteCMD.ExecuteNonQuery()
                MsgBox("Registro Eliminado")
                SQLiteCon.Close()



                CargarcomboUsuario()
                MAXID()
            Else
                MessageBox.Show("Eliminación Cancelada")

            End If


        Catch ex As Exception
            ' MsgBox("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        consultaDGW()
    End Sub

End Class