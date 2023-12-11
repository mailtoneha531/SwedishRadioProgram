using SwedishRadioProgram.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpClient<SwedishRadioService>(); // Registering HttpClient for SwedishRadioService
builder.Services.AddScoped<SwedishRadioService>(); // Registeristering SwedishRadioService


// Add services to the container.
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
    pattern: "{controller=RadioProgram}/{action=Index}/{id?}");

app.Run();
