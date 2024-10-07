using GK.Application.Engines;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadApplicationConfigurations();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

