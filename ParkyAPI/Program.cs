global using ParkyAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var appSettingsSesstion = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSesstion);
var appSettings = appSettingsSesstion.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
//    {
//        Title = "Parky API NP",
//        Version = "1",
//        Description = "Lan's Parky API NP"
//    });
//    //options.SwaggerDoc("ParkyOpenAPISpecTrail", new Microsoft.OpenApi.Models.OpenApiInfo()
//    //{
//    //    Title = "Parky API Trail",
//    //    Version = "1",
//    //    Description = "Lan's Parky API Trail"
//    //});
//    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
//    options.IncludeXmlComments(cmlCommentsFullPath);
//});

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }
    });

    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API NP");
    //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrail/swagger.json", "Parky API Trail");
    //});
}

app.UseHttpsRedirection();

app.UseCors(x =>

    x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
