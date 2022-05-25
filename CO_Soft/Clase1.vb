
Imports System.Data.SQLite


Public Class Clase1
    Public SQLiteDA As New SQLiteDataAdapter

    Sub consultaDGW(ByVal Tabla As DataGridView, ByVal Sql As String)

        Try



            ':::Instruccion Try para capturar errores

            ':::Creamos el objeto DataAdapter y le pasamos los dos parametros (Instruccion, conexión)
            SQLiteDA = New SQLiteDataAdapter(Sql, DB_Path)

            ':::Creamos el objeto DataTable que recibe la informacion del DataAdapter
            Dim DT As New DataTable
            ':::Pasamos la informacion del DataAdapter al DataTable mediante la propiedad Fill
            SQLiteDA.Fill(DT)
            ':::Ahora mostramos los datos en el DataGridView
            Tabla.DataSource = DT
        Catch ex As Exception

        End Try

    End Sub

End Class
