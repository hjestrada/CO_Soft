
Imports System.Runtime.InteropServices
Imports System.IO.Ports
Imports System.Data.SQLite

Public Class Reportes
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


    <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
    Public Shared Function CreateRoundRectRgn(LR As Integer, TR As Integer, RR As Integer, BR As Integer, WE As Integer, HE As Integer) As IntPtr

    End Function
    '----------------------------------------------------

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Reportes_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Necesario para redondear formulario
        Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))
        CargarcomboUsuario()

    End Sub



    Public Sub CargarcomboUsuario()
        Try
            SQLiteCon.Open()
            Dim MySQLDA As New SQLiteDataAdapter("SELECT `id_usuario`,`nombre_usu` FROM usuario", SQLiteCon)
            Dim table As New DataTable
            MySQLDA.Fill(table)
            ComboBox3.DataSource = table
            ComboBox3.ValueMember = "id_usuario"
            ComboBox3.DisplayMember = "nombre_usu"

            ComboBox1.SelectedIndex = 0
            SQLiteCon.Close()

        Catch ex As Exception


        Finally
            SQLiteCon.Close()
            SQLliteCMD = Nothing
        End Try
    End Sub

    Sub cargarfincas()
        Dim aux_usuario = ComboBox3.SelectedValue.ToString

        Try

            Dim MySQLDA As New SQLiteDataAdapter("SELECT nombre_fin,id_finca FROM finca INNER JOIN usuario WHERE usuario.ID_USUARIO=@Gg and finca.id_usuario=@Gg", SQLiteCon)
            MySQLDA.SelectCommand.Parameters.AddWithValue("@Gg", aux_usuario)
            Dim ds As New DataSet
            Dim table As New DataTable
            MySQLDA.Fill(ds)
            ComboBox1.DataSource = ds.Tables(0)
            ComboBox1.DisplayMember = ds.Tables(0).Columns(0).Caption.ToString
            ComboBox1.ValueMember = "id_finca"

        Catch ex As Exception

        Finally
            SQLiteCon.Close()

        End Try

    End Sub



    Sub cargarvariedad()
        Dim aux_usuario = ComboBox1.SelectedValue.ToString

        Try

            Dim MySQLDA As New SQLiteDataAdapter("SELECT nombre_var,id_variedad FROM variedad INNER JOIN finca WHERE variedad.id_finca=@Gg and finca.id_finca=@Gg", SQLiteCon)
            MySQLDA.SelectCommand.Parameters.AddWithValue("@Gg", aux_usuario)
            Dim ds As New DataSet
            Dim table As New DataTable

            MySQLDA.Fill(ds)
            ComboBox2.DataSource = ds.Tables(0)
            ComboBox2.DisplayMember = ds.Tables(0).Columns(0).Caption.ToString
            ComboBox2.ValueMember = "id_variedad"


        Catch ex As Exception
            '  MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


        Finally
            SQLiteCon.Close()

        End Try

    End Sub

    Sub cargarmuestras()
        Dim aux_variedad = ComboBox2.SelectedValue.ToString

        Try

            Dim MySQLDA As New SQLiteDataAdapter("SELECT fecha_recepcion,id_muestra FROM muestra INNER JOIN variedad WHERE variedad.id_variedad=@Gg and muestra.id_variedad=@Gg", SQLiteCon)
            MySQLDA.SelectCommand.Parameters.AddWithValue("@Gg", aux_variedad)
            Dim ds As New DataSet
            Dim table As New DataTable

            MySQLDA.Fill(ds)
            ComboBox4.DataSource = ds.Tables(0)
            ComboBox4.DisplayMember = ds.Tables(0).Columns(1).Caption.ToString
            ComboBox4.ValueMember = "id_variedad"

        Catch ex As Exception

        Finally
            SQLiteCon.Close()

        End Try



    End Sub



    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        cargarfincas()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        cargarvariedad()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        cargarmuestras()
    End Sub
End Class