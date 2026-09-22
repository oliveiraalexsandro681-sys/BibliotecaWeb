using Microsoft.EntityFrameworkCore;
using BibliotecaWeb.Data; // Ajuste para o namespace correto do seu DbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Controllers com Views e Swagger
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRO DO SERVIÇO QUE ESTAVA FALTANDO:
builder.Services.AddHttpClient();
builder.Services.AddScoped<BibliotecaWeb.Services.LivroApiService>();

// 2. Configurar o DbContext para SQLite (Ajuste o nome do seu DbContext se necessário)
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

// 5. Rota padrão para carregar as telas visuais do site
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();