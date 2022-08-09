using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musference.Data;
using Musference.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Musference.Services;
using Musference;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Musference.Middleware;

var builder = WebApplication.CreateBuilder(args);
string connStr = builder.Configuration.GetConnectionString("robert");
// Add services to the container.
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connStr)); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var authenticationModel = new AuthenticationModel();
builder.Configuration.GetSection("Authentication").Bind(authenticationModel);
builder.Services.AddSingleton(authenticationModel);
builder.Services.AddTransient<Seeder>();
builder.Services.AddControllers();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = true;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationModel.JwtIssuer,
        ValidAudience = authenticationModel.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationModel.JwtKey)),
    };
});
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
