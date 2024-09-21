using Entities.Entidades;
using Infra.Configuracao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;
using WebApi.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomFluentValidation();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financeiro_2023", Version = "v1" });
});

builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextBase>();

builder.Services.InjecaoDependencia();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(option =>
             {
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,

                     ValidIssuer = "Teste.Securiry.Bearer",
                     ValidAudience = "Teste.Securiry.Bearer",
                     IssuerSigningKey = JwtSecurityKey.Create("43443FDFDF34DF34343fdf344SDFSDFSDFSDFSDF4545354345SDFGDFGDFGDFGdffgfdGDFGDGR%")
                 };

                 option.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = context =>
                     {
                         Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                         return Task.CompletedTask;
                     }
                 };
             });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financeiro_2023"); });
}

//var devClient = "http://localhost:4200";
var devClient = "http://localhost:40";

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithOrigins(devClient));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
