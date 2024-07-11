using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tech_Arch_360.Models;
using Tech_Arch_360.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<MenuService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Use `Ignore` to remove `$id` and other metadata properties
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.WriteIndented = true;
});

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddTransient<ExcelService>();

// Generate and store the JWT key
var key = GenerateRandomKey();
var symmetricSecurityKey = new SymmetricSecurityKey(key);
builder.Services.AddSingleton(symmetricSecurityKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = symmetricSecurityKey,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<Tech_Arc_360Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TechArch")));

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech_Arch_360 API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech_Arch_360 API V1");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static byte[] GenerateRandomKey()
{
    // Disable the warning.
#pragma warning disable SYSLIB0023
    using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
    {
        var key = new byte[32]; // 256 bits
        rng.GetBytes(key);
        return key;
    }
}
