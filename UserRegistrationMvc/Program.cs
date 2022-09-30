using Microsoft.EntityFrameworkCore;
using UserRegistrationMvc.DataContext;
using UserRegistrationMvc.Hubs;
using UserRegistrationMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAuthService,AuthService>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Login.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<Context>(x => x.UseSqlServer("server=DESKTOP-FHK353D;database=MVC_Registration;integrated security=true""));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR();

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

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=register}/{id?}");

app.Run();
