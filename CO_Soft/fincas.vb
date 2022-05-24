
Option Strict Off
Option Explicit On

Imports System.Data.SQLite
Imports System.Runtime.InteropServices




Public Class fincas
    Public auxconsulta

    Dim Obj As New Clase1

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

    Sub CONSULTAFRECUENTE()
        Dim Sql As String = "Select * FROM finca order by id_usuario"
        Obj.consultaDGW(DataGridView1, Sql)
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()

    End Sub


    Private Sub fincas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try


            'Necesario para redondear formulario
            Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))
            MAXID()
            CargarcomboUsuario()

            ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
            Button2.Enabled = False

            CONSULTAFRECUENTE()
        Catch ex As Exception

        End Try



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
            'MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")

        Finally
            SQLiteCon.Close()
            SQLliteCMD = Nothing
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Trim(TextBox1.Text) = "" Or Trim(TextBox2.Text) = "" Then
            MsgBox("¡Error! no se permiten campos vacios")

        Else

            SQLiteCon.Close()
            Try
                SQLiteCon.Open()
                SQLliteCMD = New SQLiteCommand

                With SQLliteCMD
                    .CommandText = " INSERT INTO finca (`id_finca`, `nombre_fin`,`ubicacion_fin`,`id_usuario`)
                                                           VALUES (NULL,@nombre_fin,@ubicacion_fin,@id_usuario)"
                    .Connection = SQLiteCon
                    .Parameters.AddWithValue("@nombre_fin", Me.TextBox1.Text)
                    .Parameters.AddWithValue("@ubicacion_fin", Me.TextBox2.Text)
                    .Parameters.AddWithValue("@id_usuario", ComboBox1.SelectedValue.ToString)

                    .ExecuteNonQuery()


                End With

                SQLiteCon.Close()
                MsgBox("Datos Registrados Exitosamente")
                limpiarcamposfincas()
                CONSULTAFRECUENTE()
                MAXID()

            Catch ex As Exception
                SQLiteCon.Close()
                '  MsgBox("Descripcion del error:" & ex.Message)
                Return
            End Try
            SQLiteCon.Close()
        End If

    End Sub


    Sub limpiarcamposfincas()

        TextBox1.Clear()
        TextBox2.Clear()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        Try


            SQLiteCon.Open()
            Button2.Enabled = False
            Dim auxlabel As String

            SQLliteCMD = New SQLiteCommand




            '------------------Bloque de actualizacion-------------------
            With SQLliteCMD


                ' Update`finca` Set id_finca=5, nombre_fin='paraisos',ubicacion_fin='lejisimos',id_usuario=1117508100 WHERE id_finca=5
                ' "UPDATE `finca` SET id_finca=@id_finca, nombre_fin=@nombre_fin,ubicacion_fin=@ubicacion_fin,id_usuario=@id_usuario WHERE id_finca=@id_finca"

                auxlabel = Me.Label6.Text


                .CommandText = "PRAGMA foreign_keys = ON; UPDATE `finca` SET  nombre_fin=@nombre_fin,ubicacion_fin=@ubicacion_fin,id_usuario=@id_usuario WHERE id_finca=@id_finca"
                .Connection = SQLiteCon


                .Parameters.AddWithValue("@id_finca", auxlabel)
                .Parameters.AddWithValue("@nombre_fin", Me.TextBox1.Text)
                .Parameters.AddWithValue("@ubicacion_fin", Me.TextBox2.Text)
                .Parameters.AddWithValue("@id_usuario", auxconsulta)



                .ExecuteNonQuery()

            End With
            MsgBox("Datos Actualizados Exitosamente", MsgBoxStyle.Information, "Information")
            SQLiteCon.Close()
            limpiarcamposfincas()
            Button1.Enabled = True
            CONSULTAFRECUENTE()
            CargarcomboUsuario()
            MAXID()

            Me.Close()

        Catch ex As Exception
            '  MsgBox("Error: " & ex.Message)
            SQLiteCon.Close()
            Return
        End Try


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            '------bloque de busquedad------------------------------------------

            Dim Numero As String
            Numero = InputBox("Por favor digite el código de la finca a Buscar", "Buscar")

            If String.IsNullOrEmpty(Numero) Then
                MessageBox.Show("Busqueda Cancelada")
                Return
            End If

            consulta = "SELECT * FROM `finca` WHERE `id_finca`=" & Numero & ""
            SQLiteDA = New SQLiteDataAdapter(consulta, SQLiteCon)
            dataSet = New DataSet
            SQLiteDA.Fill(dataSet, "finca")
            lista = dataSet.Tables("finca").Rows.Count

            If lista = 0 Then
                MsgBox("Registro no encontrado")
            End If
            Label6.Text = dataSet.Tables("finca").Rows(0).Item("id_finca")
            TextBox1.Text = dataSet.Tables("finca").Rows(0).Item("nombre_fin")
            TextBox2.Text = dataSet.Tables("finca").Rows(0).Item("ubicacion_fin")
            auxconsulta = dataSet.Tables("finca").Rows(0).Item("id_usuario")


            Dim MySQLDA As New SQLiteDataAdapter("SELECT `nombre_usu` FROM usuario WHERE id_usuario=" & auxconsulta & "", SQLiteCon)

            Dim table As New DataTable
            MySQLDA.Fill(table)
            ComboBox1.DataSource = table
            ComboBox1.ValueMember = "id_usuario"
            ComboBox1.DisplayMember = "nombre_usu"


            Button2.Enabled = True
            SQLiteCon.Close()
            Button1.Enabled = False
            CONSULTAFRECUENTE()

            'CargarcomboUsuario()

        Catch ex As Exception
            ' MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Button1.Enabled = True

            Button2.Enabled = False
            Dim Numero As String
            Numero = InputBox("Por favor digite el consecutivo de la finca a eliminar")

            If String.IsNullOrEmpty(Numero) Then
                MessageBox.Show("Eliminación Cancelada")
                Return
            End If

            Dim result As DialogResult = MessageBox.Show("¿Seguro que desea eliminar este registro?, este proceso es irreversible y puede ocasionar perdidas de datos posteriores ", "Atención", MessageBoxButtons.YesNo)

            If (result = DialogResult.Yes) Then
                SQLiteCon.Open()
                SQLliteCMD = New SQLite.SQLiteCommand(" PRAGMA foreign_keys = ON;delete from finca  where id_finca='" & Numero & "'", SQLiteCon)


                SQLliteCMD.ExecuteNonQuery()
                MsgBox("Registro Eliminado")
                SQLiteCon.Close()
                limpiarcamposfincas()

                Button1.Enabled = True
                CONSULTAFRECUENTE()
                CargarcomboUsuario()
                MAXID()
            Else
                MessageBox.Show("Eliminación Cancelada")

            End If


        Catch ex As Exception
            ' MsgBox("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try

            Dim Sql As String = "Select * FROM finca where id_usuario=" & ComboBox1.SelectedValue.ToString & ""
            Obj.consultaDGW(DataGridView1, Sql)

        Catch ex As Exception

        End Try
    End Sub
End Class