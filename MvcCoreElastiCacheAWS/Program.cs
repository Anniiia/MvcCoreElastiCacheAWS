using Microsoft.Extensions.Options;
using MvcCoreElastiCacheAWS.Repositories;
using MvcCoreElastiCacheAWS.Services;
using NuGet.Protocol.Core.Types;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "cache-coches.q0786q.ng.0001.use1.cache.amazonaws.com:6379";
    options.InstanceName = "ElastiCacheExample";
});

builder.Services.AddTransient<RepositoryCoches>();
builder.Services.AddTransient<ServiceAWSCache>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
