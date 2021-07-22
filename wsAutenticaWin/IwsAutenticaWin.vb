' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IwsAutenticaWin

    <OperationContract()>
    Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String) As Boolean

    <OperationContract()>
    Function GetGroups() As String

    ' TODO: agregue aquí sus operaciones de servicio

End Interface

' Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
' Puede agregar archivos XSD al proyecto. Después de compilar el proyecto, puede usar directamente los tipos de datos definidos aquí, con el espacio de nombres "wsAutenticaWin.ContractType".

