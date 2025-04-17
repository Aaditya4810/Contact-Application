using ContactApp;
using ContactApp.Repositories.Implementations;
using ContactApp.Repositories.Interfaces;
using ContactApp.Validators;
using FluentValidation;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//for fluent validator
builder.Services.AddValidatorsFromAssemblyContaining<RegisterVMValidator>();

builder.Services.AddSingleton<IContactInterface,ContactRepository>();
builder.Services.AddSingleton<IUserInterface,UserRepository>();  
// builder.Services.AddSingleton<NpgsqlConnection>((UserRepository)=>
// {
//     var connectionString=UserRepository.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
//     return new NpgsqlConnection(connectionString);
// });

builder.Services.AddSingleton<NpgsqlConnection>((Provider)=>
{
    var connectionString=Provider.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options=>
{
    options.IdleTimeout=TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly=true;
    options.Cookie.IsEssential=true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSession();

app.Run();
