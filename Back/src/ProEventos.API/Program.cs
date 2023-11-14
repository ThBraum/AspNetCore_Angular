using Microsoft.EntityFrameworkCore;
using ProEventos.Application;
using ProEventos.Persistence;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProEventosContext>( //Adiciona o contexto do banco de dados
    context => context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IEventoService, EventoService>(); //Se um controller pedir um IEventoService, vai receber um EventoService
builder.Services.AddScoped<IGeralPersistence, GeralPersist>();
builder.Services.AddScoped<IEventoPersistence, EventoPersist>();

builder.Services.AddCors(options => //Configuração do CORS
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddControllers().AddNewtonsoftJson(
    x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Mapeamento automático
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy"); //Configuração do CORS

app.MapControllers();

app.Run();