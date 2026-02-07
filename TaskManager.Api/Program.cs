using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskManager.Api.Middleware;
using TaskManager.Modules.Auth;
using TaskManager.Modules.Tasks.Data;
using TaskManager.Modules.Tasks.Mapping;
using TaskManager.Modules.Tasks.Repository;
using TaskManager.Modules.Tasks.Service;
using TaskManager.Modules.Tasks.Validator;




var builder = WebApplication.CreateBuilder(args);

//Fail Fast - Required Secrets
var jwtKey = builder.Configuration["Jwt:Key"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("JWT Key is missing. Set Jwt:Key via User Secrets or Environment Variables.");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Database connection string is missing. Set ConnectionStrings:DefaultConnection.");
}

// -------------------- SERVICES --------------------

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddAutoMapper(typeof(TaskProfile));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskRequestValidator>();

builder.Services.AddAuthModule(builder.Configuration);

builder.Services.AddHttpContextAccessor();

//------------AUTHENTICATION-------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// -----------CORS----------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// -------------------- APP PIPELINE--------------------

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrWhiteSpace(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

// Set CurrentUserId for DbContext
app.Use(async (context, next) =>
{
    var db = context.RequestServices.GetRequiredService<TasksDbContext>();

    var userId = context.User.FindFirst("id")?.Value;

    if (int.TryParse(userId, out var parsedUserId))
    {
        db.CurrentUserId = parsedUserId;
    }

    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
