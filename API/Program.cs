using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// builder.Services.AddDbContext<DataContext>(opt => //moved to service collection
// {
//     opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });


// builder.Services.AddCors();
// builder.Services.AddScoped<ITokenService,TokenService>(); moved to AppExtensions

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme). //moved to IdentityServiceExtension
//             AddJwtBearer(options =>
//             {
//                 options.TokenValidationParameters = new TokenValidationParameters //how to validate the token for API
//                 {
//                     ValidateIssuerSigningKey = true,
//                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
//                     ValidateIssuer = false,
//                     ValidateAudience = false
//                 };
//             });

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));//before author
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
