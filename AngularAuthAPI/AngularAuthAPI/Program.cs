
#region Usings
using AngularAuthAPI.Context;
using AngularAuthApplication.Contracts;
using AngularAuthInfrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

#endregion

#region Services
var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AuthMicroService"));
});
builder.Services.AddScoped<IAuthenticate, Authenticate>();
#endregion

#region General Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Cross Policy Service

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

#region Authentication Service By JWT

builder.Services.AddAuthentication(x =>
{
    //تعیین توکن به عنوان روش پیش فرض احراز هویت
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    //اگر کاربر احراز هویت نشده باشد یک خطای مناسب برگردانده شود 
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
//احراز هویت با توکن
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        //بررسی امضای دیجیتال:فعال
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!@#34sdf&*78kjh^rghw%$mnKJLN23a1234567890abcdefghijklmnopqrstuvwxyz")),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});


 #endregion

#endregion

#region Middleware 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Authenticate");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowSpecificOrigins"); 

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
#endregion
