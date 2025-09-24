using Microsoft.EntityFrameworkCore;
using APIDotnet6.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseInMemoryDatabase("CrmDb"));

builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7000", "http://localhost:5000", "https://localhost:7001", "http://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CRM API", Version = "v1" });
});

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowRazorApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
