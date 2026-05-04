using Etiquetas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Etiquetas.Infrastructure.Repositories;
using Etiquetas.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();


builder.Services.AddDbContext<EtiquetaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();