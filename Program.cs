using api_screenvault.Data;
using api_screenvault.Model;
using api_screenvault.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//CORS
builder.Services.AddCors(o => o.AddPolicy("MyCorsPolicyScreenvaoult", builder => {
    builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();
    //.WithMethods("PUT", "DELETE", "GET", "POST");

}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileService>();
builder.Services.AddDbContext<ApplicationDbContext>(
       options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //setting db context for api

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
app.UseCors("MyCorsPolicyScreenvaoult");
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<User>();

app.Run();
