# Sistema de Nómina

Sistema de gestión de nómina desarrollado en ASP.NET Core 8 MVC con C#.

## Tecnologías
- ASP.NET Core 8 MVC
- Entity Framework Core 8
- SQL Server
- BCrypt.Net para cifrado de contraseñas
- iTextSharp para exportación PDF
- Bootstrap 5

## Requisitos
- Visual Studio 2022
- SQL Server / SQL Server Express
- .NET 8 SDK

## Instrucciones de ejecución

1. Clonar el repositorio:
   git clone https://github.com/fractalbots/sistema-nomina.git

2. Abrir la solución en Visual Studio:
   app/SistemaNomina/SistemaNomina.sln

3. Configurar la cadena de conexión en appsettings.json:
   "DefaultConnection": "Server=localhost\\SQLEXPRESS02;Database=NominaDB;Trusted_Connection=True;TrustServerCertificate=True"

4. Ejecutar las migraciones en la Consola del Administrador de paquetes:
   Update-Database

5. Insertar usuario administrador en SQL Server:
   USE NominaDB;
   SET IDENTITY_INSERT users ON;
   INSERT INTO users (emp_no, usuario, clave, rol)
   VALUES (1, 'admin', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.', 'Administrador');
   SET IDENTITY_INSERT users OFF;

6. Ejecutar el proyecto con F5

7. Acceder en el navegador:
   https://localhost:7215

8. Credenciales de acceso:
   Usuario: admin
   Clave: admin123

## Módulos disponibles
- /Auth/Login — Inicio de sesión
- /Employees — Gestión de empleados
- /Departments — Gestión de departamentos
- /DeptEmp — Asignación empleado-departamento
- /DeptManager — Gerentes de departamento
- /Titles — Títulos y cargos
- /Salaries — Salarios y auditoría
- /Reportes — Reportes exportables PDF

## Estructura del repositorio
- /app — Código fuente ASP.NET Core MVC
- /db — Migraciones y scripts SQL

## Ramas
- main — Versión estable
- develop — Integración
- feature/empleados — Desarrollo de módulos
