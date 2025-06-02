using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null; // lub ReferenceHandler.IgnoreCycles
    });

builder.Services.AddDbContext<MasterContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
app.Run();