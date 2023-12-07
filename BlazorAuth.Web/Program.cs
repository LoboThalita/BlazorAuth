using BlazorAuth.Web.Services;
using BlazorAuth.Web.Services.Auth;
using BlazorAuth.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<BlazorAppLoginService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7016/");
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<BlazorAppLoginService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomBlazorAuthStateProvider>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddScoped<IProdutoService, ProdutoService>();

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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
