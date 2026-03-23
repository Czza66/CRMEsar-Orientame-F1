using CRMEsar.AccesoDatos.Data.Repository;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.AccesoDatos.Services.Logs;
using CRMEsar.AccesoDatos.Services.Menu;
using CRMEsar.AccesoDatos.Services.Notificaciones;
using CRMEsar.AccesoDatos.Services.Slider;
using CRMEsar.Data;
using CRMEsar.Models;
using CRMEsar.Services.Prestadores;
using CRMEsar.Services.Usuarios;
using CRMEsar.Utilidades;
using CRMEsar.Utilidades.ConfirmarCorreo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Configuramos la conexion SQL
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Agregamos el servicio de Identity a la aplicacion
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Agrega esta línea para que los claims del rol se agreguen correctamente
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
    CustomClaimsPrincipalFactory>();

//Esta linea es para la URL de retorno para acceder o login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/"); //Ruta del Login
    options.AccessDeniedPath = new PathString("/User/Home/Index/Bloqueado");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);// Tiempo de inactividad
    options.SlidingExpiration = true;//Renueva el tiempo si el usuario sigue activo
});

builder.Services.AddControllersWithViews();

//Builder Utilidad de Encriptar GUID
builder.Services.AddSingleton<ProtectorUtils>();

//--------------------------------------InyectarServicios

//Agregar contenedor de trabajo al contenedor IoC de inyeccion de dependiencias
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();
builder.Services.AddScoped<IPrestadorCodigoService, PrestadorCodigoService>();
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ILogService, LogService>(); // LogService
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<INotificacionesService, NotificacionesService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IConfirmarCorreo, ConfirmarCorreo>();

builder.Services.AddScoped<ISliderService, SliderService>();

builder.Services.AddScoped<IEliminadoLogicoService, EliminadoLogicoService>();

//---------------------------------------------------

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error/500");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Personalizacion de URL's
app.MapControllerRoute(
    name: "registro",
    pattern: "registro",
    defaults: new { area = "User", controller = "Home", action = "Register" });

app.MapControllerRoute(
    name: "authenticator",
    pattern: "authenticator",
    defaults: new { area = "User", controller = "Home", action = "VerificarCodigoAutenticador" });

app.MapControllerRoute(
    name: "ActivarAuthenticator",
    pattern: "ActivarAuthenticator",
    defaults: new { area = "User", controller = "Home", action = "ActivarAutenticador" });

app.MapControllerRoute(
    name: "/",
    pattern: "/",
    defaults: new { area = "User", controller = "Home", action = "Index" });


//Rutas de errores generales
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Primero autenticación (establece User)
app.UseAuthorization();   // Luego autorización (evalúa User)


app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
