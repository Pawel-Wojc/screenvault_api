using api_screenvault.Data;
using api_screenvault.Helpers;
using api_screenvault.Model;
using api_screenvault.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//CORS
builder.Services.AddCors(o => o.AddPolicy("MyCorsPolicyScreenvault", builder => {
    builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();
    //.WithMethods("PUT", "DELETE", "GET", "POST");

}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
       options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddScoped<IAnonymousPostService, AnonymousPostService>();
builder.Services.AddScoped<IAnonymousAzureBlobHandling, AnonymousAzureBlobHandling>();
builder.Services.AddScoped<ISharedPostIdGenerator, SharedPostIdGenerator>();
builder.Services.AddTransient<AssetsFileManager>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddApiEndpoints(); //setting identity core

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme); //setting using jwt
builder.Services.AddAuthorizationBuilder();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyCorsPolicyScreenvault");
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<User>();

app.Run();
