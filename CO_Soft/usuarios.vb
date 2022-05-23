
Option Strict Off
Option Explicit On
Imports System.Text.RegularExpressions

Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.IO
Imports System.Threading

Public Class usuarios



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

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub usuarios_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Necesario para redondear formulario
        Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))
        Button2.Enabled = False



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '
        If Trim(TextBox1.Text) = "" Or Trim(TextBox3.Text) = "" Or Trim(MaskedTextBox1.Text) = "" Then
            MsgBox("¡Error! no se permiten campos vacios")

        Else
            Dim sMail As String
            sMail = TextBox4.Text
            If Regex.IsMatch(sMail, "^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$") = False Then
                MsgBox("Error! Direccion de correo electronico invalido")
                TextBox4.Clear()
            Else

                SQLiteCon.Close()
                Try
                    SQLiteCon.Open()
                    SQLliteCMD = New SQLiteCommand

                    With SQLliteCMD
                        .CommandText = " INSERT INTO USUARIO (`id_usuario`, `nombre_usu`,`direccion`,`email`,`telefono`)
                                                           VALUES (@id_usuario, @nombre_usu,@direccion,@email,@telefono)"
                        .Connection = SQLiteCon
                        .Parameters.AddWithValue("@id_usuario", Me.TextBox5.Text)
                        .Parameters.AddWithValue("@nombre_usu", Me.TextBox1.Text)
                        .Parameters.AddWithValue("@direccion", Me.TextBox3.Text)
                        .Parameters.AddWithValue("@email", Me.TextBox4.Text)
                        .Parameters.AddWithValue("@telefono", Me.MaskedTextBox1.Text)
                        .ExecuteNonQuery()
                    End With

                    SQLiteCon.Close()
                    MsgBox("Datos Registrados Exitosamente")
                    limpiarcamposusuarios()

                Catch ex As Exception
                    SQLiteCon.Close()
                    MsgBox("Descripcion del error:" & ex.Message)
                    Return
                End Try
                SQLiteCon.Close()
            End If
        End If




    End Sub
    Sub limpiarcamposusuarios()
        TextBox5.Clear()
        TextBox1.Clear()

        TextBox3.Clear()
        TextBox4.Clear()
        MaskedTextBox1.Clear()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            SQLiteCon.Open()

            Button2.Enabled = False

            '------------------Bloque de actualizacion-------------------

            With SQLliteCMD
                .CommandText = "UPDATE `usuario`  SET  id_usuario=@id_usuario,nombre_usu=@nombre_usu,direccion=@direccion,email=@email,telefono=@telefono WHERE id_usuario=@id_usuario"
                .Connection = SQLiteCon
                .Parameters.AddWithValue("@id_usuario", Me.TextBox5.Text)
                .Parameters.AddWithValue("@nombre_usu", Me.TextBox1.Text)
                .Parameters.AddWithValue("@direccion", Me.TextBox3.Text)
                .Parameters.AddWithValue("@email", Me.TextBox4.Text)
                .Parameters.AddWithValue("@telefono", Me.MaskedTextBox1.Text)
                .ExecuteNonQuery()

            End With
            MsgBox("Datos Actualizados Exitosamente", MsgBoxStyle.Information, "Information")
            SQLiteCon.Close()
            limpiarcamposusuarios()
            Button1.Enabled = True
            '---------------------------
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            SQLiteCon.Close()
        End Try



    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If Not IsNumeric(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            '------bloque de busquedad------------------------------------------

            Dim Numero As String
            Numero = InputBox("Por favor digite la cédula del Usuario a Buscar", "Buscar")

            If String.IsNullOrEmpty(Numero) Then
                MessageBox.Show("Busqueda Cancelada")
                Return
            End If

            consulta = "SELECT * FROM `USUARIO` WHERE `id_usuario`=" & Numero & ""
            SQLiteDA = New SQLiteDataAdapter(consulta, SQLiteCon)
            dataSet = New DataSet
            SQLiteDA.Fill(dataSet, "usuario")
            lista = dataSet.Tables("usuario").Rows.Count

            If lista = 0 Then
                MsgBox("Registro no encontrado")
            End If
            TextBox5.Text = dataSet.Tables("usuario").Rows(0).Item("id_usuario")
            TextBox1.Text = dataSet.Tables("usuario").Rows(0).Item("nombre_usu")
            TextBox3.Text = dataSet.Tables("usuario").Rows(0).Item("direccion")
            TextBox4.Text = dataSet.Tables("usuario").Rows(0).Item("email")
            MaskedTextBox1.Text = dataSet.Tables("usuario").Rows(0).Item("telefono")
            Button2.Enabled = True
            SQLiteCon.Close()
            Button1.Enabled = False


        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            Button1.Enabled = True

            Button2.Enabled = False
            Dim Numero As String
            Numero = InputBox("Por favor digite el numero de Cedula del Usuario a eliminar")

            If String.IsNullOrEmpty(Numero) Then
                MessageBox.Show("Eliminación Cancelada")
                Return
            End If

            Dim result As DialogResult = MessageBox.Show("¿Seguro que desea eliminar este registro?, este proceso es irreversible y puede ocasionar perdidas de datos posteriores ", "Atención", MessageBoxButtons.YesNo)

            If (result = DialogResult.Yes) Then
                SQLiteCon.Open()
                SQLliteCMD = New SQLite.SQLiteCommand("delete from usuario where id_usuario='" & Numero & "'", SQLiteCon)
                SQLliteCMD.ExecuteNonQuery()
                MsgBox("Registro Eliminado")
                SQLiteCon.Close()
                limpiarcamposusuarios()
                Button1.Enabled = True
            Else
                MessageBox.Show("Eliminación Cancelada")

            End If


        Catch ex As Exception
            MsgBox("Error: " & ex.Message)

        End Try
    End Sub
End Class
