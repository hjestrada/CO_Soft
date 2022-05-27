Imports System.Runtime.InteropServices
Imports System.IO.Ports
Imports System.Data.SQLite

Public Class lecturas


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

    Private Sub lecturas_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Necesario para redondear formulario
        Me.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height - 2, 20, 20))

        Label6.Text = Now
        TimerGraficar.Stop()

        CargarcomboUsuario()


        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList
        ColorPh()

    End Sub


    Public Sub CargarcomboUsuario()
        Try
            SQLiteCon.Open()

            'Dim MySQLDA As New SQLiteDataAdapter("SELECT(`nombre_usu` || ' ' || `apellido_usu`) AS ALGO FROM usuario", SQLiteCon)

            Dim MySQLDA As New SQLiteDataAdapter("SELECT `id_usuario`,`nombre_usu` FROM usuario", SQLiteCon)

            Dim table As New DataTable
            MySQLDA.Fill(table)
            ComboBox3.DataSource = table
            ComboBox3.ValueMember = "id_usuario"
            ComboBox3.DisplayMember = "nombre_usu"

            ComboBox1.SelectedIndex = 0
            SQLiteCon.Close()

        Catch ex As Exception
            'MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")

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
            '  MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


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
            '  MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


        Finally
            SQLiteCon.Close()

        End Try

    End Sub













    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Dim ask As MsgBoxResult = MsgBox("¿Esta seguro que desea cerrar esta ventana?, se perderá el proceso de fermentación que esta realizando, ¿Desea Continuar?", MsgBoxStyle.YesNo)
        If ask = MsgBoxResult.Yes Then

            Me.Close()
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles TimerGraficar.Tick

        Try
            graficar()
        Catch ex As Exception

        End Try

    End Sub

    Sub graficar()
        Chart1.Series("Temperatura").Points.AddXY(DateTime.Now.ToString("hh:mm:ss"), Temp)
        Chart2.Series("Dioxido de Carbono (ppm)").Points.AddXY(DateTime.Now.ToString("hh:mm:ss"), CO2)
        Chart3.Series("pH").Points.AddXY(DateTime.Now.ToString("hh:mm:ss"), pH)
        insertarLecturas()

    End Sub

    Sub ValidarpH()

        Dim pHvalidar = Val(TextBox3.Text)


        If pHvalidar <= 6.99 Then
            Label12.Text = "Ácido"

        Else
            If pHvalidar = 7 Then
                Label12.Text = "Neutro"

            Else
                Label12.Text = "Alcalino"
            End If

        End If


    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button1.Enabled = True

        TimerGraficar.Stop()
        Label11.Text = "Detenido"
        Label11.ForeColor = Color.Red
        Chart1.Series(0).Points.Clear()
        Chart2.Series(0).Points.Clear()
        Chart3.Series(0).Points.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (ComboBox4.Items.Count <= 0) Then
            MsgBox("Error no se puede iniciar un proceso de fermentacion si no existe una muestra asociada")
        Else
            graficar()
            Button1.Enabled = False

            TimerLectura.Start()
            TimerGraficar.Start()


            Label11.Text = "Iniciado"
            Label11.ForeColor = Color.Green
            Dim aux

            aux = NumericUpDown1.Value * 60000
            TimerGraficar.Interval = aux

        End If




    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button1.Enabled = True

        TimerGraficar.Stop()
        Label11.Text = "Pausa"
        Label11.ForeColor = Color.DarkOrange

    End Sub

    Private Sub TimerLectura_Tick(sender As Object, e As EventArgs) Handles TimerLectura.Tick
        ColorPh()
        Label6.Text = Now
        ValidarpH()
        TextBox2.Text = CO2
        TextBox1.Text = Temp
        TextBox3.Text = pH
    End Sub





    Sub insertarLecturas()


        SQLiteCon.Close()
        Try
            SQLiteCon.Open()
            SQLliteCMD = New SQLiteCommand

            With SQLliteCMD

                .CommandText = " INSERT INTO lectura (`id_lectura`, `ph`,`temperatura`,`dioxido`,'fecha_lectura','id_muestra')
                                            VALUES (NULL,@ph,@temperatura,@dioxido,@fecha_lectura,@id_muestra)"
                .Connection = SQLiteCon
                .Parameters.AddWithValue("@ph", pH)
                .Parameters.AddWithValue("@temperatura", Temp)
                .Parameters.AddWithValue("@dioxido", CO2)
                .Parameters.AddWithValue("@fecha_lectura", Now)
                .Parameters.AddWithValue("@id_muestra", ComboBox4.Text)
                .ExecuteNonQuery()

            End With
            SQLiteCon.Close()
        Catch ex As Exception
            SQLiteCon.Close()
            '  MsgBox("Descripcion del error:" & ex.Message)
            Return
        End Try
        SQLiteCon.Close()

    End Sub

    Sub ColorPh()

        Dim auxcolor As Decimal = Convert.ToDecimal(pH)

        If (auxcolor >= 0 And auxcolor <= 0.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(239, 28, 35)
        End If


        If (auxcolor >= 1 And auxcolor <= 1.99) Then

            Me.PanelColor.BackColor = Color.FromArgb(255, 122, 27)
        End If

        If (auxcolor >= 2 And auxcolor <= 2.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(245, 198, 20)

        End If


        If (auxcolor >= 3 And auxcolor <= 3.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(246, 229, 1)
        End If


        If (auxcolor >= 4 And auxcolor <= 4.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(183, 211, 51)
        End If


        If (auxcolor >= 5 And auxcolor <= 5.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(134, 193, 67)
        End If


        If (auxcolor >= 6 And auxcolor <= 6.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(80, 184, 73)
        End If


        If (auxcolor >= 7 And auxcolor <= 7.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(53, 169, 68)
        End If


        If (auxcolor >= 8 And auxcolor <= 8.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(36, 180, 111)
        End If


        If (auxcolor >= 9 And auxcolor <= 9.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(10, 186, 183)
        End If


        If (auxcolor >= 10 And auxcolor <= 10.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(70, 144, 205)
        End If


        If (auxcolor >= 11 And auxcolor <= 11.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(58, 84, 161)
        End If


        If (auxcolor >= 12 And auxcolor <= 12.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(89, 81, 158)
        End If


        If (auxcolor >= 13 And auxcolor <= 13.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(99, 69, 157)
        End If


        If (auxcolor >= 14 And auxcolor <= 14.99) Then
            Me.PanelColor.BackColor = Color.FromArgb(66, 46, 131)

        End If


    End Sub


    Private Sub Chart1_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Chart1.Series(0).ToolTip = "#VAL"
    End Sub

    Private Sub Chart2_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Chart2.Series(0).ToolTip = "#VAL"
    End Sub

    Private Sub Chart3_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Chart3.Series(0).ToolTip = "#VAL"
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

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Me.PanelColor.BackColor = Color.FromArgb(239, 28, 35)
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint

    End Sub
End Class