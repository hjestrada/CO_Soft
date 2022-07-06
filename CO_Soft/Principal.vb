﻿Public Class Principal

    Public EstadoConx As String = "Desconectado"
    Public estado As Boolean = False


    Dim az As String     'utilizada para almacenar los datos que se reciben por el puerto
    Dim sib As Integer    ' sera utilizada como contador
    Dim msn(100000000) As String    'vector que servira de buffer para los datos que van llegando al puerto


    Private Sub PictureBox3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'GetSerialPortNames()
        If SerialPort1.IsOpen Then
            iconoConectado()

        Else
            iconoDesconectado()
            lecturas.Label11.Text = "Falla de Comunicación"
            lecturas.Label11.ForeColor = Color.Red
            lecturas.TimerGraficar.Stop()

        End If


    End Sub


    Sub iconoConectado()
        PBConectado.Visible = True
        PBDesconectado.Visible = False
        Label11.Text = "Tarjeta Conectada"
        Label11.ForeColor = Color.Green
    End Sub
    Sub iconoDesconectado()
        PBConectado.Visible = False
        PBDesconectado.Visible = True
        Label11.Text = "Tarjeta Desconectada"
        Label11.ForeColor = Color.Red
        Button1.Text = "Conectar"
        GetSerialPortNames()
    End Sub





    Sub Setup_Puerto_Serie()

        Try
            With SerialPort1
                If .IsOpen Then
                    .Close()

                End If

                .PortName = ComboBox1.Text
                .BaudRate = 9600 '// 19200 baud rate
                .DataBits = 8 '// 8 data bits
                .StopBits = IO.Ports.StopBits.One '// 1 Stop bit
                .Parity = IO.Ports.Parity.None '
                .DtrEnable = False
                .Handshake = IO.Ports.Handshake.None
                .ReadBufferSize = 2048
                .WriteBufferSize = 1024
                '.ReceivedBytesThreshold = 1
                .WriteTimeout = 500
                .Encoding = System.Text.Encoding.Default

                .Open() ' ABRE EL PUERTO SERIE

            End With

        Catch ex As Exception
            MsgBox("Error al abrir el puerto serial: " & ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Sub GetSerialPortNames()

        ' muestra COM ports disponibles.
        Dim l As Integer
        Dim ncom As String

        Try
            ComboBox1.Items.Clear()
            For Each sp As String In My.Computer.Ports.SerialPortNames
                l = sp.Length

                If ((sp(l - 1) >= "0") And (sp(l - 1) <= "9")) Then
                    ComboBox1.Items.Add(sp)
                Else
                    'hay una letra al final del COM
                    ncom = sp.Substring(0, l - 1)
                    ComboBox1.Items.Add(ncom)
                End If
            Next
            If ComboBox1.Items.Count >= 1 Then
                ComboBox1.Text = ComboBox1.Items(1)

            Else
                ComboBox1.Text = ""

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub



    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Dim ask As MsgBoxResult = MsgBox("¿Esta seguro que desea cerrar la aplicacion?", MsgBoxStyle.YesNo)
        If ask = MsgBoxResult.Yes Then
            SerialPort1.Close()
            Me.Close()
        End If
    End Sub



    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        usuarios.MdiParent = Me
        usuarios.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If Button1.Text = "Conectar" Then
                Button1.Text = "Desconectar"
                Setup_Puerto_Serie()
                estado = True
                EstadoConx = "Conectado"
                lecturas.Button1.Enabled = True
            Else

                Dim ask As MsgBoxResult = MsgBox("¿Desea cerrar la conexión con el Dispositivo  " & SerialPort1.PortName & " ?", MsgBoxStyle.YesNo)
                If ask = MsgBoxResult.Yes Then
                    SerialPort1.Close()
                    EstadoConx = "Desconectado"

                End If
                Button1.Text = "Conectar"
                GetSerialPortNames()

            End If

        Catch ex As Exception
        End Try
    End Sub


    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        lectura_Serial()
    End Sub

    Sub lectura_Serial()
        Dim auxdatos As String = ""

        Try
            az = SerialPort1.ReadLine.Trim
            msn(sib) = az
            auxdatos += msn(sib) + " "
            comoviene = auxdatos

            Dim Array_trama() As String = Split(auxdatos, "/")
            pH = Val(Array_trama(0))
            Temp = Val(Array_trama(1))
            CO2 = Val(Array_trama(2))
            sib += 1

        Catch ex As Exception
            CheckForIllegalCrossThreadCalls = False ' DESACTIVA ERROR POR SUBPROCESO
            ' MsgBox(ex.Message)
        End Try

    End Sub







    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        fincas.MdiParent = Me
        fincas.Show()
    End Sub

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetSerialPortNames()
        Dash.MdiParent = Me
        Dash.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        variedad.MdiParent = Me
        variedad.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        muestras.MdiParent = Me
        muestras.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dash.MdiParent = Me
        Dash.Show()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub PictureBox3_Click_1(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Acercade.MdiParent = Me

        Acercade.Show()

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Reportes.MdiParent = Me
        Reportes.Show()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
