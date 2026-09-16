using System.Reflection;
using BusinessLayer;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAcessLayer.Abstract;
using DataAcessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDBAdmin.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAppUserDal, EFAppUser>();
builder.Services.AddScoped<IAppUserService, AppUserManager>();

builder.Services.AddScoped<ISliderDal, EFSliderDal>();
builder.Services.AddScoped<ISliderService, SliderManager>();

builder.Services.AddScoped<IBrandDal, EFBrandDal>();
builder.Services.AddScoped<IBrandService, BrandManager>();

builder.Services.AddScoped<IOfferDal, EFOfferDal>();
builder.Services.AddScoped<IOfferService, OfferManager>();

builder.Services.AddScoped<IAboutDal, EFAboutDal>();
builder.Services.AddScoped<IAboutService, AboutManager>();

builder.Services.AddScoped<IGetInTouchDal, EFGetInTouchDal>();
builder.Services.AddScoped<IGetInTouchService, GetInTouchManager>();

builder.Services.AddScoped<IHowItWorkDal, EFHowItWorkDal>();
builder.Services.AddScoped<IHowItWorkService, HowItWorkManager>();

builder.Services.AddScoped<ICloudinaryService, CloudinaryManager>();


builder.Services.AddScoped<IDatabaseSettings, DatabaseSettings>();
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddScoped<IDatabaseSettings>(opt =>
{
    return opt.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});



builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
})
.AddMongoDbStores<AppUser, AppRole, Guid>(
    databaseSettings.ConnectionString,
    databaseSettings.DatabaseName
)
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.AddAutoMapper(cfg=>cfg.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Slider/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapHub<SignalRHub>("/SignalRHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
