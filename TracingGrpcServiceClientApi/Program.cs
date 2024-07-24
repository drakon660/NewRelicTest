using Serilog;
using Serilog.Events;
using TracingGrpcService;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using TracingGrpcServiceClientApi;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // <-- Change this line!

// Add services to the container.

builder.Services.AddCorrelate(options => 
    options.RequestHeaders =
    [
        // List of incoming headers possible. First that is set on given request is used and also returned in the response.
        "X-Correlation-ID"
    ]
);

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

//builder.Services.AddCorrelationId(options=>options.AddToLoggingScope = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(options => options.Address = new Uri("http://localhost:5281"))
    .AddGrpcCorrelationIdForwarding();
    //.AddGrpcCorrelationIdForwarding();

var app = builder.Build();
app.UseCorrelate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();