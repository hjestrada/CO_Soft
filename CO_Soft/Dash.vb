Public Class Dash
    Private Sub RadialSlider1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BunifuDatepicker1_onValueChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub BunifuGauge1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub BunifuGauge2_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub BunifuTileButton1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BunifuCards6_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs)

        If PictureBox2.Visible = True Then
            PictureBox2.Visible = False
        Else
            PictureBox2.Visible = True

        End If
    End Sub

    Private Sub Dash_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList
        CargarcomboUsuario()

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
            '  MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")

        Finally
            SQLiteCon.Close()
            SQLliteCMD = Nothing
        End Try
    End Sub







    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        If PictureBox7.Visible = True Then
            PictureBox7.Visible = False

        Else
            PictureBox7.Visible = True

        End If
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
            ' MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


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
            '   MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


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
            ' MsgBox("Error" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")


        Finally
            SQLiteCon.Close()

        End Try

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Dim bm As Bitmap = PictureBox2.Image
            bm.RotateFlip(RotateFlipType.Rotate90FlipNone)
            Me.PictureBox2.Image = bm

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

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