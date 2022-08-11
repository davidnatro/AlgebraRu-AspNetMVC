using mednik.Data;
using mednik.Data.Repositories.Contacts;
using mednik.Data.Repositories.Groups;
using mednik.Data.Repositories.Posts;
using mednik.Models;
using mednik.Data.Repositories.Services;
using mednik.Data.Repositories.Subjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

#region Databases

builder.Services.AddDbContext<AppDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("Azure")));

builder.Services.Configure<MsSqlSettings>(builder.Configuration.GetSection("MsSQLSettings"));

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

#endregion

#region Repositories

builder.Services.AddScoped<IPostsRepository, PostsRepositoryDapper>();
builder.Services.AddScoped<IServicesRepository, ServicesRepositoryDapper>();
builder.Services.AddScoped<IContactsRepository, ContactsRepositoryDapper>();
builder.Services.AddScoped<ISubjectsRepository, SubjectsRepositoryDapper>();
builder.Services.AddScoped<IGroupsRepository, GroupsRepositoryDapper>();

#endregion

#region Identity

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Login/Index");

#endregion

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();