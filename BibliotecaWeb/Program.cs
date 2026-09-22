using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data; // Nome da pasta onde está o DbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionar suporte a Controllers com Views (HTML/MVC) e APIs
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configurar o DbContext para SQLite (Troca 'AppDbContext' pelo nome real do teu contexto)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=biblioteca.db"));

var app = builder.Build();

// 3. Criar a base de dados SQLite automaticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar a base de dados.");
    }
}

// 4. Configuração do Swagger em /swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
    });
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 5. Rota padrão para as páginas HTML do site
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();