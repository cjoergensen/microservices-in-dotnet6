using SelfService.WebApp.Backend.ApiClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ICustomerProfileServiceClient, CustomerProfileServiceClient>(client =>
    client.BaseAddress = new Uri("https://localhost:8001"));

builder.Services.AddHttpClient<IConsumptionNotificationSubscriptionServiceClient, ConsumptionNotificationSubscriptionServiceClient>(client =>
    client.BaseAddress = new Uri("https://localhost:8002"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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