using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using Serilog;
using Serilog.Events;
using TracingGrpcService.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .CreateBootstrapLogger(); // <-- Change this line!

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCorrelate(options => 
    options.RequestHeaders =
    [
        // List of incoming headers possible. First that is set on given request is used and also returned in the response.
        "X-Correlation-ID"
    ]
);

builder.Services
    .AddGrpc();

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

var app = builder.Build();
app.UseCorrelate();
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();