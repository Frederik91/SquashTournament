using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SquashTournament.Server.Data;
using Microsoft.OpenApi.Models;
using SquashTournament.Server.Endpoints;
using SquashTournament.Server.Requirements;
using Microsoft.AspNetCore.Authorization;
using SquashTournament.Server.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<AuthAppSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("ADMIN", p => p.AddRequirements(new AdminRoleRequirement("ADMIN")));
});

builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddSingleton<IAuthorizationHandler, AdminRoleRequirementHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddLogging(builde =>
{
    builde.AddConsole();
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SquashTournament API V1");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseCors(options =>
  {
      options.WithOrigins("http://localhost:3000");
      options.AllowAnyMethod();
      options.AllowAnyHeader();
  });

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        // log request details
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Request {context.Request.Method} {context.Request.Path}");

        await next.Invoke();

        // log response details
        logger.LogInformation($"Response {context.Response.StatusCode}");
        if (context.Response.Body is not null && context.Response.Body.CanRead)
        {
            using var reader = new StreamReader(context.Response.Body);
            var body = new StreamReader(context.Response.Body).ReadToEnd();
            logger.LogInformation($"Response body: {body}");
        }
    });
}

app.AddAuthenticationEndpoints();
app.AddTournamentEndpoints();

app.Run();
