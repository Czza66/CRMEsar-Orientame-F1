##  Requisitos Previos para descargar

- Visual Studio 2022 community (con complementos de **ASP.NET y desarrollo web**)  : https://visualstudio.microsoft.com/es/vs/community/
- .NET 8 SDK  : https://dotnet.microsoft.com/es-es/download/dotnet/8.0
- SQL Server 2022 + SSMS  
- Node.js v18+  : https://nodejs.org/en
- Git  : https://git-scm.com/install/windows


1. **Clonar el repositorio en una carpeta local**
   git clone: https://github.com/Czza66/CRMEsar-Orientame-F1.git 
   Luego de eso abrir Visual Studio 2022, seleccionar opcion "Abrir un proyecto o una solucion" luego de esto en las carpetas del proyecto abrir: CRMEsar -> CRMEsar.sln

2. **Cargar backup de BD en equipo local**
   Descargar el backup de la base de datos CRMPrestadoras: https://drive.google.com/file/d/1mlCVaGZnYXuiAMBWGRsphQ7b6TCdFoKg/view?usp=sharing
   Crear base de datos en SQLServer con nombre "CRMPrestadoras"

3. **Abrir en Visual Studio 2022 y cambiar cadena de conexion**
   Configurar la cadena de conexión: Dentro de la carpeta del proyecto y en el archivo CRMEsar -> appsettings.json

   "ConnectionStrings": {
   "ConexionSQL": "Server=TUINSTANCIABD;Database=CRMPrestadoras;User ID=USUARIO;Password=CONTRASEÑA;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true"
    }

4. **Ejecutar proyecto**
   En la cinta de opciones superior de Visual Studio seleccionar la opcion de Reproducir o Ejecutar https 
