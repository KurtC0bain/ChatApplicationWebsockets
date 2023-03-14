using ChatApplicationWebsockets.Extensions;
using ChatApplicationWebsockets.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddWebSocketManager();

var app = builder.Build();


var serviceScopeFactory = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>();
var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

app.UseWebSockets();
app.MapWebSocketManager("/ws", serviceProvider.GetService<ChatMessageHandler>());
app.UseStaticFiles();
app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();

