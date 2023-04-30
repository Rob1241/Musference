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
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;
using Musference.Logic;
using Musference.Models.Entities;
using Musference.Middlewares;

var builder = WebApplication.CreateBuilder(args);
string connStr = builder.Configuration.GetConnectionString("robert");
// Add services to the container.
builder.Services.AddDbContext<DataBaseContext>(options => options.UseMySQL(connStr));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var authenticationModel = new AuthenticationModel();
builder.Configuration.GetSection("Authentication").Bind(authenticationModel);
builder.Services.AddSingleton(authenticationModel);
builder.Services.AddControllers();
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPagination, Pagination>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme \"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
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

void Seeder(IHost app)

{
    
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    

    

}
Seeder(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

app.UseCors(myAllowSpecificOrigins);

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class MysqlEntityFrameworkDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddEntityFrameworkMySQL();
        new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)
            .TryAddCoreServices();
    }
}