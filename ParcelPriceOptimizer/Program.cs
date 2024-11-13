using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParcelPriceOptimizer.DAL;
using ParcelPriceOptimizer.BLL;
using ParcelPriceOptimizer.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ParcelPriceOptimizer.DAL.DbInitializer;
using Microsoft.AspNetCore.Identity.UI.Services;
using ParcelPriceOptimizer.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllersWithViews(); 
builder.Services.AddHttpClient();
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/"); 
    options.Conventions.AllowAnonymousToPage("/Account/Login"); 
});
builder.Services.AddDbContext<ApplicationDbContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { 
    options.SignIn.RequireConfirmedAccount = true; 
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); 
    options.Lockout.MaxFailedAccessAttempts = 5; 
    options.Password.RequireDigit = true; 
    options.Password.RequiredLength = 8; 
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequireUppercase = true; 
    options.Password.RequireLowercase = true; 
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.RegisterBLLServices(); 
builder.Services.RegisterDALServices(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<MailJetSettings>(builder.Configuration.GetSection("MailJet"));

var jwtIssuer = builder.Configuration["JwtSettings:Issuer"]; 
var jwtAudience = builder.Configuration["JwtSettings:Audience"]; 
var jwtKey = builder.Configuration["JwtSettings:Secret"]; 

if (string.IsNullOrEmpty(jwtKey)) { 
    throw new ArgumentNullException("JwtSettings:Secret is missing in configuration."); 
} 

builder.Services.AddAuthentication(options => { 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
}) .AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters { 
    ValidateIssuer = true, 
    ValidateAudience = true, 
    ValidateLifetime = true, 
    ValidateIssuerSigningKey = true, 
    ValidIssuer = jwtIssuer, 
    ValidAudience = jwtAudience, 
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) 
}; 
    options.Events = new JwtBearerEvents { 
        OnMessageReceived = context => { 
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>(); 
            logger.LogInformation("Token received: " + context.Token); 
            context.Token = context.Request.Cookies["jwt"]; 
            return Task.CompletedTask; 
        }, 
        OnAuthenticationFailed = context => { 
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>(); 
            logger.LogError("Authentication failed: " + context.Exception.Message); 
            return Task.CompletedTask; 
        }, 
        OnTokenValidated = context => { 
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>(); 
            logger.LogInformation("Token validated for user: " + context.Principal.Identity.Name); 
            return Task.CompletedTask; 
        } 
    }; 
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parcel Menagement Order", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddCors(options => 
{ 
    options.AddPolicy("AllowAllOrigins", builder => 
    { 
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); 
    }); 
});

builder.Services.AddControllers(); 
builder.Services.AddCors(options => { 
    options.AddPolicy("AllowFrontend", policy => { 
        policy.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod(); 
    }); 
}); 
builder.Services.AddScoped<IDbInitializer, DbInitializer>(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 
var app = builder.Build(); 
await SeedDatabaseAsync(); 

if (app.Environment.IsDevelopment()) 
{ 
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parcel Menagement Order"); 
      }); 
} 
app.UseHttpsRedirection(); 
app.UseCors("AllowFrontend");
app.UseCors("AllowAllOrigins");
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting(); 
app.UseAuthentication(); 
app.UseAuthorization();

app.MapGet("/payment-success", context =>
{
    context.Response.Redirect("/api/payment/payment-success");
    return Task.CompletedTask;
});

app.MapGet("/payment-cancelled", context =>
{
    context.Response.Redirect("api/payment/payment-cancelled");
    return Task.CompletedTask;
});

app.UseEndpoints(endpoints => { 
    endpoints.MapRazorPages(); 
    endpoints.MapControllers(); 
}); 
app.MapControllers(); 
app.Run(); 

async Task SeedDatabaseAsync() { 
    using (var scope = app.Services.CreateScope()) { 
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); 
        await dbInitializer.InitializeAsync(); 
    } 
}