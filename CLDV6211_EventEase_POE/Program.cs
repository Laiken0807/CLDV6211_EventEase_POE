using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CLDV6211_EventEase_POE.Data;
using CLDV6211_EventEase_POE.Models;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<CLDV6211_EventEase_POEContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Register BlobServiceClient
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new BlobServiceClient(config["AzureBlobStorage:ConnectionString"]);
});

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ✅ Correct for serving static files (e.g., CSS, JS, images)

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
