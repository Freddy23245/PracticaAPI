using AbmAPI.AccesoDatos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

var claveSecreta = Encoding.ASCII.GetBytes("clave_secreta_aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // Cambia esto con una clave segura

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(claveSecreta)
        };
    });

// Add services to the container.
builder.Services.AddSingleton<DatosUsuario>();
builder.Services.AddSingleton<DatosCliente>();
builder.Services.AddSingleton<DatosProducto>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("UsuarioConexion");
app.Services.GetRequiredService<DatosUsuario>().ConfigurarConexion(connectionString);
app.Services.GetRequiredService<DatosCliente>().ConfigurarConexion(connectionString);
app.Services.GetRequiredService<DatosProducto>().ConfigurarConexion(connectionString);
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();
app.UseDeveloperExceptionPage();
app.Run();
