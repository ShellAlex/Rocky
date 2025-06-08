using Rocky_DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Rocky_Utility;
using Rocky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_DataAccess.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

/*
builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromMinutes(10);
    Options.Cookie.HttpOnly = true;
    Options.Cookie.IsEssential = true;
});
*/

builder.Services.AddSession(Options =>
{
    Options.Cookie.IsEssential = true; 
    Options.Cookie.HttpOnly = true;                
    Options.IdleTimeout = TimeSpan.FromMinutes(30); 
    Options.IOTimeout = TimeSpan.FromMinutes(30); 
});
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IApplicationTypeRepository,ApplicationTypeRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IInquiryHeaderRepository,InquiryHeaderRepository>();
builder.Services.AddScoped<IInquiryDetailRepository,InquiryDetailRepository>();

builder.Services.AddControllersWithViews();

string connstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(
    connstring
));


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<IEmailSender, GmailSender>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 0;
    

}
);

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
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
