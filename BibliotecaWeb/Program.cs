using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data; // Ajuste para o namespace correto do seu DbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionar suporte a Controllers com Views (HTML/MVC) e suporte a APIs
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configurar o DbContext para SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=biblioteca.db"));

var app = builder.Build();

// 3. Criar a base de dados SQLite automaticamente no arranque (EnsureCreated)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar a base de dados.");
    }
}

// 4. Configuração do Swagger
// Agora o Swagger fica acessível em /swagger para não sobrescrever o seu site!
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
    });
}

app.UseStaticFiles(); // Garante o carregamento dos ficheiros CSS/JS da pasta wwwroot

app.UseRouting();

app.UseAuthorization();

// 5. Configuração da rota padrão para as telas do site (MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapeia também os controllers de API
app.MapControllers();

app.Run();