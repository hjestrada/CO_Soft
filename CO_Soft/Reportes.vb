Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Data.SQLite
Imports System.IO
Imports Document = iTextSharp.text.Document
Imports iTextSharp.text.pdf
Imports iTextSharp.text

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
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList

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

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        If (ComboBox4.Items.Count <= 0) Then
            MsgBox("Error no se puede iniciar un proceso de Reporte si no existe una muestra asociada")
        Else
            Dim auxMuestra = ComboBox4.Text


            Dim Sql As String = "select * from lectura where id_muestra=" & auxMuestra
            Filtro_SQLite(Sql, DataGridView2)

        End If


    End Sub



    Sub Filtro_SQLite(ByVal Sql As String, ByVal Tabla As DataGridView)

        ':::La conexion y la consulta SQL
        Dim Da As New SQLiteDataAdapter(Sql, SQLiteCon)
        ':::Creamos nuestro DataTable para almacenar el resultado
        Dim Dt As New DataTable
        ':::Llenamos nuestro DataTable con el resultado de la consulta
        Da.Fill(Dt)
        ':::Asignamos a nuestro DataGridView el DataTable para mostrar los registros
        Tabla.DataSource = Dt
        ':::Damos color a las filas y formato a la columna hora
        ' Tabla.RowsDefaultCellStyle.BackColor = Color.LightBlue
        'Tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.WHITE
        'Tabla.Columns("Hora").DefaultCellStyle.Format = "hh:mm:ss tt"
    End Sub



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        If (ComboBox4.Items.Count <= 0) Then
            MsgBox("Error no se puede iniciar un proceso de Exportacion a PDF si no existe una muestra asociada")
        Else


            Label1.Text = "Inciando Proceso de Exportación a PDF"
        'Label1.ForeColor = Color.YELLOW


        Try

            ' Intentar generar el documento.
            Dim doc As New Document(PageSize.LETTER.Rotate(), 10, 10, 10, 10)
            ' Path que guarda el reporte en el escritorio de windows (Desktop).
            Dim filename As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\Reporte.pdf"
            Dim file As New FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
            PdfWriter.GetInstance(doc, file)
            doc.Open()
            ExportarDatosPDF(doc)
            doc.Close()
            Process.Start(filename)
        Catch ex As Exception
            'Si el intento es fallido, mostrar MsgBox.
            MessageBox.Show("No se puede generar el documento PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

            Label1.Text = "Datos exportados a PDF exitosamente"

        End If


    End Sub



    Public Sub ExportarDatosPDF(ByVal document As Document)
        'Se crea un objeto PDFTable con el numero de columnas del DataGridView. 
        Dim datatable As New PdfPTable(DataGridView2.ColumnCount)
        'Se asignan algunas propiedades para el diseño del PDF.
        datatable.DefaultCell.Padding = 3
        Dim headerwidths As Single() = GetColumnasSize()
        datatable.SetWidths(headerwidths)
        datatable.WidthPercentage = 50
        datatable.DefaultCell.BorderWidth = 2
        datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER
        'Se crea el encabezado en el PDF. 
        Dim encabezado As New Paragraph("Reporte Aplicativo de Fermentación Memory´s Café", New Font(Font.Name = "Tahoma", 20, Font.Bold))
        'Se crea el texto abajo del encabezado.
        Dim texto As New Phrase("Reporte de Datos: " + Now, New Font(Font.Name = "Tahoma", 14, Font.Bold))
        Dim texto2 As New Paragraph("Usuario: " + ComboBox3.Text, New Font(Font.Name = "Tahoma", 14, Font.Bold))
        Dim texto3 As New Paragraph("Finca: " + ComboBox1.Text, New Font(Font.Name = "Tahoma", 14, Font.Bold))
        Dim texto4 As New Paragraph("Variedad: " + ComboBox2.Text, New Font(Font.Name = "Tahoma", 14, Font.Bold))
        Dim texto5 As New Paragraph("Muestra: " + ComboBox4.Text, New Font(Font.Name = "Tahoma", 14, Font.Bold))


        'Se capturan los nombres de las columnas del DataGridView.
        For i As Integer = 0 To DataGridView2.ColumnCount - 1
            datatable.AddCell(DataGridView2.Columns(i).HeaderText)
        Next
        datatable.HeaderRows = 1
        datatable.DefaultCell.BorderWidth = 1
        For i As Integer = 0 To DataGridView2.Rows.Count - 2
            For j As Integer = 0 To DataGridView2.Columns.Count - 1
                datatable.AddCell((DataGridView2(j, i).Value).ToString)
            Next
            datatable.CompleteRow()
        Next
        'da 2 tab entre columnas
        datatable.AddCell("")
        datatable.AddCell("")
        'imprime resultados
        datatable.AddCell(DataGridView2(2, 6).Value)
        datatable.AddCell(DataGridView2(3, 6).Value)
        datatable.CompleteRow()
        'Se agrega etiquetas
        document.Add(encabezado)
        document.Add(texto)
        document.Add(texto2)
        document.Add(texto3)
        document.Add(texto4)
        document.Add(texto5)


        document.Add(datatable)
    End Sub




    Public Function GetColumnasSize() As Single()
        Dim values As Single() = New Single(DataGridView2.ColumnCount - 1) {}
        For i As Integer = 0 To DataGridView2.ColumnCount - 1
            values(i) = CSng(DataGridView2.Columns(i).Width)
        Next
        Return values
    End Function

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If (ComboBox4.Items.Count <= 0) Then
            MsgBox("Error no se puede iniciar un proceso de Exportacion a Excel si no existe una muestra asociada")
        Else
            Dim auxMuestra = ComboBox4.Text

            Label1.Text = "Inciando Proceso de Exportación a excel"
            llenarExcel(DataGridView2)
            Label1.Text = "Datos exportados a excel exitosamente"

        End If


    End Sub
End Class