using Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ! Add this connection string before 'var app = builder.Build();' is run
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data source=net-core_auth-login.db");
});

// ! Add this for Identity user and role configuration e.g. password rules, remove 'options => {}' for no configuraiton
builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    }
).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// ! Add for Identity redirect of path unauthorized routes
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
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

app.Run();
