using Dapper;
using E_commerce.Data;
using E_commerce.Repositories.Implementations;
using E_commerce.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
SqlMapper.AddTypeHandler(new UploadStatusTypeHandler());

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageUploadAttemptRepository, ImageUploadAttemptRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.Run();
