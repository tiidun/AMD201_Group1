
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add configuration to read ocelot.json
builder.Configuration.AddJsonFile("gateway.json", optional: false, reloadOnChange: true);

// Add Ocelot services
builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use Ocelot middleware
app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseOcelot().Wait();

app.Run();