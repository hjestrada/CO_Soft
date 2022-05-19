


Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.IO


Public Class usuarios


    Dim DB_Path As String = "Data Source=" & Application.StartupPath & "\fermentador_memo.s3db;"
    Dim SQLiteCon As New SqliteConnection(DB_Path)
    Dim SQLliteCMD As New SqliteCommand
    Dim SQLiteDA As New SQLiteDataAdapter



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
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '
        If Trim(TextBox1.Text) = "" Or Trim(TextBox2.Text) = "" Or Trim(TextBox3.Text) = "" Or Trim(MaskedTextBox1.Text) = "" Then
            MsgBox("¡Error! no se permiten campos vacios")

        Else
            SQLiteCon.Close()
            Try


                SQLiteCon.Open()
                SQLliteCMD = New SQLiteCommand

                With SQLliteCMD
                    .CommandText = " INSERT INTO USUARIO (`id_usuario`, `nombre_usu`,`apellido_usu`,`direccion`,`email`,`telefono`)
                                                           VALUES (@id_usuario, @nombre_usu,@apellido_usu,@direccion,@email,@telefono)"
                    .Connection = SQLiteCon
                    .Parameters.AddWithValue("@id_usuario", Me.TextBox5.Text)
                    .Parameters.AddWithValue("@nombre_usu", Me.TextBox1.Text)
                    .Parameters.AddWithValue("@apellido_usu", Me.TextBox2.Text)
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





    End Sub
    Sub limpiarcamposusuarios()
        TextBox5.Clear()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        MaskedTextBox1.Clear()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub
End Class