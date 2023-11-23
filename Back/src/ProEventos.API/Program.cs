using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProEventos.API;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProEventosContext>(options  =>  //Adiciona o contexto do banco de dados
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
}
).AddRoles<Role>() //Adiciona a Role;
.AddRoleManager<RoleManager<Role>>()
.AddSignInManager<SignInManager<User>>()
.AddRoleValidator<RoleValidator<Role>>()
.AddEntityFrameworkStores<ProEventosContext>() //Adiciona o contexto do banco de dados
.AddDefaultTokenProviders(); //Configuração para gerar o token em caso de esquecimento de senha

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IEventoService, EventoService>(); //Se um controller pedir um IEventoService, vai receber um EventoService
builder.Services.AddScoped<ILoteService, LoteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IGeralPersistence, GeralPersist>();
builder.Services.AddScoped<IEventoPersistence, EventoPersist>();
builder.Services.AddScoped<ILotePersistence, LotePersist>();
builder.Services.AddScoped<IUserPersistence, UserPersist>();

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


builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).
                //Adiciona o JsonStringEnumConverter para que o Json entenda os enums como string e não como int
                AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    //Ignora o loop de referência, poderia ser feito com AddJsonOptions ReferenceHandler
                );
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Mapeamento automático
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => //Configuração do Swagger com o SwaggerExtensions
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Insira seu token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, 
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.DocumentFilter<SwaggerOrderFilter>(); //Configuração para ordenar os controllers no Swagger
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //Configuração do JWT

app.UseAuthorization();

app.UseCors("CorsPolicy"); //Configuração do CORS

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = new PathString("/Resources")
});

app.MapControllers();

app.Run();