using Context.Models;
using IServices_Repository_Layer;
using Microsoft.EntityFrameworkCore;
using Services_Repository_Layer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CapmasTestContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("Capmas"),
   b => b.MigrationsAssembly(typeof(CapmasTestContext).Assembly.FullName)));


#region DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<INewsRepository, NewsRepository>();
builder.Services.AddTransient<IPublicationRepository, PublicationRepository>();
builder.Services.AddTransient<IIndicatorsRepository, IndicatorsRepository>();

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
