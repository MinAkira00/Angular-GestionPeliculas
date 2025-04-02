using backEnd;
using backEnd.Controllers;
using backEnd.Filtros;
using backEnd.Utilidades;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => { 
    options.Filters.Add(typeof(FiltroDeExcepcion));
}
    );

var configuration = builder.Configuration;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("defaultConnection")));
builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(builder => 
    {
        var frontendURL = configuration.GetValue<string>("frontend_url");
        builder.WithOrigins(frontendURL)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders(new string[] {"cantidadTotalRegistros"});
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
