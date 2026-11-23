
using ETFunds.Application.Category.Commands;
using ETFunds.Application.Interfaces;
using ETFunds.Infrastructure.Persistence.Data;
using ETFunds.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ETFunds.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ETFDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("ETFundsDatabase")).LogTo(Console.WriteLine, LogLevel.Information)
.EnableSensitiveDataLogging());
builder.Services.AddControllersWithViews(); builder.Services.AddOpenApi();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});


builder.Services.AddScoped<ICampaignService, CampaignService>();
var uploadPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "wwwroot",
    "uploads",
    "campaigns"
);

builder.Services.AddSingleton<IImageStorage>(
    new LocalImageStorage(uploadPath)
);
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient<IChapaPaymentService, ChapaPaymentService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(AssemblyReference).Assembly
    );
});



var allowedOrigins = builder.Configuration
.GetSection("AllowedOrigins").Get<string[]>()
?? ["http://localhost:4200"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("CETFunds", policy =>
    {
        policy.WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
   // .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
   ;
    });
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["access_token"];

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!
            )
        )
    };
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ETFunds API Reference")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp,
        ScalarClient.HttpClient);

    });
    Console.WriteLine("indev mode");
}
app.UseCors("CETFunds");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();
app.Run();
