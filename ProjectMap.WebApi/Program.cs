using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Repositories;
using ProjectMap.WebApi.Services; //toegevoegd door troubleshoot users

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);




//zelf
builder.Services.AddAuthorization();

builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString;
    });


//builder voor user accounts
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();





builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(o => new Object2DRepository(sqlConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.|




app.UseAuthorization();



//zelf voor login
app.MapGroup("/account")
   .MapIdentityApi<IdentityUser>();

//zelf voor uitloggen
app.MapPost(pattern: "/account/logout",
    async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) => {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .RequireAuthorization();




app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? "" : "")}");



app.MapOpenApi();

app.UseHttpsRedirection();







app.MapControllers().RequireAuthorization();

app.Run();