using Microsoft.EntityFrameworkCore;
using SatAlert.Infrastructure.Data;
using SatAlert.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseOracle(builder.Configuration.GetConnectionString("Oracle")
        ?? throw new InvalidOperationException("Connection string 'Oracle' não encontrada.")));

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<NotificacaoRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new() { Title = "SatAlert API", Version = "v1", Description = "API de gerenciamento de usuários e notificações de alerta satelital." });
    var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xml)) o.IncludeXmlComments(xml, includeControllerXmlComments: true);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "SatAlert API v1");
    o.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
