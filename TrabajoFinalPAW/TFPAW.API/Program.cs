//program del api

using TFPAW.Architecture;
using TFPAW.Service;

var builder = WebApplication.CreateBuilder(args);

// Lee configuración de OpenAI
var openAIOptions = builder.Configuration.GetSection("OpenAI").Get<OpenAIOptions>();
builder.Services.AddSingleton<IOpenAIService>(new OpenAIService(openAIOptions.ApiKey, openAIOptions.Endpoint));

builder.Services.AddScoped<IRestProvider, RestProvider>();
builder.Services.AddScoped<OpenStreetMapService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra HttpClient y RestProvider
builder.Services.AddHttpClient<IRestProvider, RestProvider>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org"); // Base URL para OpenStreetMap
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


public class OpenAIOptions
{
    public string Endpoint { get; set; }
    public string ApiKey { get; set; }
}
