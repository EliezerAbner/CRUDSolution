using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();


builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseMySQL(builder.Configuration["ConnectionStrings:DefaultConnection"]!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseRotativa();
app.Run();