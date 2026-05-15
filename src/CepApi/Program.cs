using CepApi.Interfaces;
using CepApi.Middleware;
using CepApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICepService, CepService>();
builder.Services.AddHttpClient<IViaCepClient, ViaCepClient>(client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/");
    client.Timeout = TimeSpan.FromSeconds(1);
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();

public partial class Program;
