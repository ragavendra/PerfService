using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PerfLoader.Data;
using PerfLoader.Helper;
using Polly.Registry;

var builder = WebApplication.CreateBuilder(args);


// I'm using Serilog, if you are using something else, you can add it here
PollyPolicies circuitBreakerManager = new PollyPolicies();

// The creation of the policies is inside the PollyPolicies class. This is done
// just so that they are grouped together in one place alongside the logging
// var googleApiCircuitBreakerPolicy = circuitBreakerManager.GetGoogleApiCircuitBreakerPolicy();
var redisCircuitBreakerPolicy = circuitBreakerManager.GetGrpcCircuitBreakerPolicy();

// We add our policies to a Registry which is just a glorified List<>. 
builder.Services.AddPolicyRegistry(new PolicyRegistry()
{
   //  { PollyPolicies.GooglePolicyName, googleApiCircuitBreakerPolicy },
    { PollyPolicies.GrpcPolicyName, redisCircuitBreakerPolicy }
});

// When we setup our HttpClient, we need to also add the "addPolicyHandler" call as shown:
// builder.Services.AddHttpClient<IGoogleTimezoneService, GoogleTimezoneService>()
  //  .AddPolicyHandler(googleApiCircuitBreakerPolicy);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

/*/ Google auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication().AddGoogle(options => 
{
    var clientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientId = clientId;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:profile", "link");
    options.ClaimActions.MapJsonKey("urn:google:image", "picture");
});*/

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<HttpContextAccessor>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

// try with a service - bg - in turn calls the Grpc to the server
// builder.Services.AddHostedService<Worker>();

// 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Google auth
app.UseCookiePolicy();
app.UseAuthentication();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
