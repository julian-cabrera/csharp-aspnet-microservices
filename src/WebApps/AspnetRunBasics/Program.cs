using AspnetRunBasics.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
builder.Services.AddHttpClient<IBasketService, BasketService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
builder.Services.AddHttpClient<IOrderService, OrderService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
