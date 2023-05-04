using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Core.Services;
using Movie.Core.UnifOfWorks;
using Movie.Repository.EntityFramework;
using Movie.Repository.Repositories;
using Movie.Repository.UnitOfWorks;
using Movie.Service.Services;
using SharedLibrary.Configurations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DI Register
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); // Herhangi bir constructor'da IAuthenticationService interface'i ile karşılaştığında AuthenticationService'ten bir nesne örneği al.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        // option.MigrationsAssembly("Movie.Repository.EntityFramework");
    });
});

builder.Services.AddIdentity<UserApp, IdentityRole>(option => {
    option.User.RequireUniqueEmail = true;
    option.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); // Şifre sıfırlama işleminde token üretebilmek için AddDefaultTokenProviders ile default bir token sağlıyorum.

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption")); // appsettings.json içerisindeki (Configuration ile appsettings'e erişiyorum) TokenOption section'ını al. CustomTokenOption sınıfı, appsettings.json'daki TokenOption içerisindeki parametreleri doldurup bir nesne örneği verecek. // Options Pattern


/* builder.Services.AddAuthentication(x =>
{// Farklı üyelik sistemleri de (kurumsal, bireysel, öğrenci vs. olsaydı) AuthenticationScheme ile eklenebilir. ↓
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, y =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        // Kontrol ↓
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
}); // Authentication'daki sheme ile JwtBearer'dan gelen şemanın iletişime geçmesi için Authentication'ın JwtBearer'ı kullanacağını belirtiyorum.*/
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
