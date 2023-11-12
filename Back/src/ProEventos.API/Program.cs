using Microsoft.EntityFrameworkCore;
using ProEventos.Application;
using ProEventos.Persistence;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddDbContext<ProEventosContext>(
    context => context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IGeralPersistence, GeralPersist>();
builder.Services.AddScoped<IEventoPersistence, EventoPersist>();

builder.Services.AddControllers();

// Configure o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();

// Configure o pipeline de solicitações HTTP.
if (app.Environment.IsDevelopment())
{
    // Configurar o Swagger UI com opções específicas
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
        c.RoutePrefix = string.Empty; // Isso tornará o Swagger UI disponível na raiz do aplicativo
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
