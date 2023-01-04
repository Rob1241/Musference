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
using MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;
using System.Configuration;
using System.Security.Principal;
using CloudinaryDotNet;
using System.Linq;
using Musference.Logic;
using Musference.Models.Entities;

var builder = WebApplication.CreateBuilder(args);
string connStr = builder.Configuration.GetConnectionString("robert");
// Add services to the container.
builder.Services.AddTransient<Seeder>();
builder.Services.AddDbContext<DataBaseContext>(options => options.UseMySQL(connStr));
var cloudName = builder.Configuration.GetValue<string>("AccountSettings:CloudName");
var apiKey = builder.Configuration.GetValue<string>("AccountSettings:ApiKey");
var apiSecret = builder.Configuration.GetValue<string>("AccountSettings:ApiSecret");
//builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connStr));
if (new[] { cloudName, apiSecret, apiKey }.Any(string.IsNullOrWhiteSpace))
    throw new ArgumentException("Specify Cloudinary account details");
builder.Services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));
//var sgApi = builder.Configuration.GetValue<string>("SendGrid");
//if (new[] { sgApi }.Any(string.IsNullOrWhiteSpace))
//    throw new ArgumentException("Specify SendGrid details");
//builder.Services.AddSingleton(new SendGridKey(sgApi));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var authenticationModel = new AuthenticationModel();
builder.Configuration.GetSection("Authentication").Bind(authenticationModel);
builder.Services.AddSingleton(authenticationModel);
//builder.Services.AddDbContext<DataBaseContext>();
//builder.Services.AddScoped<Seeder>();
builder.Services.AddControllers();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICloudinaryhandler, Cloudinaryhandler>();
builder.Services.AddScoped<IPagination, Pagination>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
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

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seeder>();
        service.Seed();
    }
}
Seeder(app);

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

public class MysqlEntityFrameworkDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddEntityFrameworkMySQL();
        new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)
            .TryAddCoreServices();
    }
}