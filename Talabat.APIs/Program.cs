using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddelWares;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositores;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
});
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddSingleton<IConnectionMultiplexer>(options=>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});
builder.Services.AddCors(Options =>
{
Options.AddPolicy("MyPolicy", option=>
        {
            option.AllowAnyHeader();
            option.AllowAnyMethod();
            // option.WithOrigins(builder.Configuration["FrontEndUrl"]);
            option.WithOrigins("http://localhost:4200");
            option.AllowCredentials();
        });
});
//All Interface Ask ClR Object ,Create Obj in Class

builder.Services.AddApplicationServies();

//Extentaion Method
builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build();

#region Updata-DataBase
using var Scope = app.Services.CreateScope();
var Service=Scope.ServiceProvider;

var loggerFactory=Service.GetRequiredService<ILoggerFactory>();
try
{
var DbContext=Service.GetRequiredService<StoreDbContext>();
    var IdentityDbContext = Service.GetRequiredService<AppIdentityDbContext>();
    await IdentityDbContext.Database.MigrateAsync();
    await DbContext.Database.MigrateAsync();
    var usermanager = Service.GetRequiredService<UserManager<AppUser>>();
  await  AppIdentityDbContextSeed.SeedUserAsync(usermanager);
    await StoreContextSeed.SeedAsync(DbContext);
}
catch(Exception ex)
{
    var Logger = loggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "An Error Occured During Appling The Migration");
}


#endregion


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddelWare>();
if (app.Environment.IsDevelopment())
{
    

    app.UseSwaggerExtention();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
