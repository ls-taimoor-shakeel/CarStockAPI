using System.Text;
using CarStockAPI.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// JWT Authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KeyGeneration.GetSecureKey())),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarStockAPI", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
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

KeyGeneration.SetConfiguration(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddDbContext<CarContext>(options => options.UseInMemoryDatabase("CarStockDb"));
builder.Services.AddDbContext<DealerContext>(options => options.UseInMemoryDatabase("DealerDb"));
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDealerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDealerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CarValidator>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();  // Add Authentication middleware
app.UseAuthorization();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarStockAPI v1");
c.RoutePrefix = string.Empty;
});

// Map controller endpoints
app.MapControllers();

app.Run();
