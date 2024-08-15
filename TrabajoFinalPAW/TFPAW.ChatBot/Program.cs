//program chatbot

using TFPAW.Service;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllersWithViews();

// Lee configuración de OpenAI
var openAIOptions = builder.Configuration.GetSection("OpenAI").Get<OpenAIOptions>();
builder.Services.AddSingleton<IOpenAIService>(new OpenAIService(openAIOptions.ApiKey, openAIOptions.Endpoint));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Travel}/{action=Index}/{id?}");

app.Run();


public class OpenAIOptions
{
    public string Endpoint { get; set; }
    public string ApiKey { get; set; }
}
