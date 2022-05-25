Imports System.Runtime.InteropServices
Imports System.IO.Ports






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
        iconoDesconectado()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Dim ask As MsgBoxResult = MsgBox("¿Esta seguro que desea cerrar esta ventana?, se perderá el proceso de fermentación que esta realizando, ¿Desea Continuar?", MsgBoxStyle.YesNo)
        If ask = MsgBoxResult.Yes Then

            Me.Close()
        End If

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label6.Text = Now
        ValidarpH()

        If Principal.SerialPort1.IsOpen Then
            iconoConectado()
        Else
            iconoDesconectado()

        End If

        TextBox2.Text = CO2
        TextBox1.Text = Temp
        TextBox3.Text = pH

        TextBox4.Text = comoviene

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

    Sub iconoConectado()
        PBConectado.Visible = True
        PBDesconectado.Visible = False
        Label11.Text = "Conectado"
        Label11.ForeColor = Color.Green
    End Sub
    Sub iconoDesconectado()
        PBConectado.Visible = False
        PBDesconectado.Visible = True
        Label11.Text = "Desconectado"
        Label11.ForeColor = Color.Red
    End Sub


End Class