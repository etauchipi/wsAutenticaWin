Imports System.Text
Imports System.Collections
Imports System.DirectoryServices

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
Public Class wsAutenticaWin
    Implements IwsAutenticaWin

    Dim _path As String
    Dim _filterAttribute As String

    Public Sub New(ByVal path As String)
        _path = path
    End Sub

    Public Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String) As Boolean Implements IwsAutenticaWin.IsAuthenticated

        Dim domainAndUsername As String = domain & "\" & username
        Dim entry As DirectoryEntry = New DirectoryEntry(_path, domainAndUsername, pwd)
        Dim retorno As Boolean

        retorno = False

        Try
            'Bind to the native AdsObject to force authentication.
            Dim obj As Object = entry.NativeObject
            Dim search As DirectorySearcher = New DirectorySearcher(entry)

            search.Filter = "(SAMAccountName=" & username & ")"
            search.PropertiesToLoad.Add("cn")
            Dim result As SearchResult = search.FindOne()

            If (result Is Nothing) Then
                retorno = False
            Else
                retorno = True
            End If

            'Update the new path to the user in the directory.
            _path = result.Path
            _filterAttribute = CType(result.Properties("cn")(0), String)

        Catch ex As Exception
            Throw New Exception("Error autenticando usuario. " & ex.Message)
        Finally

        End Try

        Return retorno

    End Function


    Public Function GetGroups() As String Implements IwsAutenticaWin.GetGroups

        Dim retorno As String
        Dim search As DirectorySearcher = New DirectorySearcher(_path)
        Dim groupNames As StringBuilder = New StringBuilder()

        search.Filter = "(cn=" & _filterAttribute & ")"
        search.PropertiesToLoad.Add("memberOf")
        retorno = String.Empty

        Try
            Dim result As SearchResult = search.FindOne()
            Dim propertyCount As Integer = result.Properties("memberOf").Count

            Dim dn As String
            Dim equalsIndex, commaIndex
            Dim propertyCounter As Integer

            For propertyCounter = 0 To propertyCount - 1
                dn = CType(result.Properties("memberOf")(propertyCounter), String)

                equalsIndex = dn.IndexOf("=", 1)
                commaIndex = dn.IndexOf(",", 1)

                If (equalsIndex = -1) Then
                    retorno = String.Empty
                    Exit For
                End If

                groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1))
                groupNames.Append("|")

            Next

            If (retorno = String.Empty) Then
            Else
                retorno = groupNames.ToString()
            End If

        Catch ex As Exception
            Throw New Exception("Error obteniendo nombres de grupo. " & ex.Message)
            retorno = String.Empty
        Finally

        End Try

        Return retorno
    End Function


End Class
