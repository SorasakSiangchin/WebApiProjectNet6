using BackEnd.Installers;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.MyInstallerExtensions(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//-------------------------
app.UseAuthentication();
app.UseCors(CORSInstaller.MyAllowAnyOrigins);

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//-------------------------
app.MapControllers();

app.UseStaticFiles();

app.Run();
