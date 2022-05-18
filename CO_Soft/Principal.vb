Public Class Principal

    Public EstadoConx As String = "Desconectado"
    Public estado As Boolean = False

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'GetSerialPortNames()
        If SerialPort1.IsOpen Then
            EstadoConx = "Conectado"
        Else
            EstadoConx = "Desconectado"
        End If

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
                ComboBox1.Text = ComboBox1.Items(0)

            Else
                ComboBox1.Text = ""

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub



    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Dim ask As MsgBoxResult = MsgBox("¿Desea cerrar la conexión con el Dispositivo  " & SerialPort1.PortName & " y salir?", MsgBoxStyle.YesNo)
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        fincas.MdiParent = Me
        fincas.Show()
    End Sub

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetSerialPortNames()
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
        lecturas.MdiParent = Me
        lecturas.Show()
    End Sub
End Class
