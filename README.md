##  Requisitos Previos

- Visual Studio 2022 (con complementos de **ASP.NET y desarrollo web**)  
- .NET 8 SDK  
- SQL Server 2022 + SSMS  
- Node.js v18+  
- Git  


1. **Clonar el repositorio**
   git clone https://github.com/Czza66/CRMEsar-Orientame-F1.git
   Abrir en Visual Studio 2022

2. **Abrir en Visual Studio 2022 y cambiar cadena de conexion**
   Configurar la cadena de conexión en appsettings.json dentro de la carpeta CRMEsar

   "ConnectionStrings": {
   "DefaultConnection": "Server=TU_SERVIDOR;Database=CRM_ESAR;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }

   Una vez cambies esto, debes abrir la consola de administrador de paquetes y ejecutar los siguientes comenados para que nos creen las migraciones hacia la DB Sql (Actuacion del ORM)
   Update-Database

2. **Ejecutar proyecto**
   Una vez se ejecuten correctamente las  migraciones y creaciones de tablas... vamos a ejecutar el proyecto
