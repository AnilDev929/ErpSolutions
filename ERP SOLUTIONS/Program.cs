
using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using ERP_SOLUTIONS.Services.Interfaces;
using ERP_SOLUTIONS.Services.Implementations;


var builder = WebApplication.CreateBuilder(args);
//Services registered before Build()

// Add logging providers (optional, Console is default)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();   // Logs to console
builder.Logging.AddDebug();     // Logs to Visual Studio debug output

// =====================
// 1 REGISTER SERVICES
// =====================

// MVC (UI + Controllers)
builder.Services.AddControllersWithViews();

// ✅ Add Cookie Authentication (for MVC login)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";        // Redirect here if unauthorized
        options.AccessDeniedPath = "/Account/AccessDenied"; // Optional
    });


// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStudentProfileService, StudentProfileService>();
builder.Services.AddScoped<IClassService, ClassService>();

// Swagger (for future API use)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




// Helpers
builder.Services.AddScoped<JwtHelper>(); // keep for future JWT use

//// ✅ 👉 PUT AUTHENTICATION HERE
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "MyCookieAuth";
//    options.DefaultChallengeScheme = "MyCookieAuth";
//})
//.AddCookie("MyCookieAuth", options =>
//{
//    options.LoginPath = "/Account/Login";
//})
//.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
//        )
//    };
//});

//builder.Services.AddAuthorization();

// Session + HTTP
builder.Services.AddSession();
builder.Services.AddHttpClient();


builder.Services.AddAuthorization();

// =====================
// BUILD APP
// =====================

//Always put your middleware like this:
var app = builder.Build();

// =====================
// MIDDLEWARE ORDER (FIXED)
// =====================

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");
    app.UseHsts();

    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // ✅ FIRST establish routing

// Authentication & Authorization MUST come before MVC endpoints
app.UseAuthentication(); // ✅ must come before Authorization
app.UseAuthorization();

app.UseSession();

// Add cache-control headers here, after auth but before endpoints
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

// =====================
// ROUTING
// =====================

// Default route → Login page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

// API Controllers (for future use)
app.MapControllers();

app.Run();