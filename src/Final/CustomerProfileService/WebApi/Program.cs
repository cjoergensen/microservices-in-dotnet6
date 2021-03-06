using CustomerProfileService.Data;
using Microsoft.AspNetCore.Mvc.Versioning;
using NServiceBus;
using Serilog;
using Shared.Telemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Shared.Logging.LogFactory.BuildLogger());
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("CustomerProfileService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});


// Add services to the container.
builder.Services.AddTelemetry("CustomerProfileService", "1.0.0");
builder.Services.AddSingleton<INotificationSettingsRepository, NotificationSettingsRepository>();
builder.Services.AddSingleton<ICustomerProfileRepository, CustomerProfileRepository>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();