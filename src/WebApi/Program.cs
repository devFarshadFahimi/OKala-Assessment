using Application;
using Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

#region Hosted Service

//NOTE: If Uncomment this section if will provide a background hosted service that calls CMC sevices to collect Cryptocurrency maps and saved them into database.

//builder.Services.AddHostedService<CryptoCurrencyMapHostedService>();
//builder.Services.Configure<HostOptions>(p =>
//{
//    p.ServicesStartConcurrently = true;
//    p.ServicesStopConcurrently = true;
//});

#endregion

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 1;
    }));
var app = builder.Build();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
app.Run();

public partial class Program { }
