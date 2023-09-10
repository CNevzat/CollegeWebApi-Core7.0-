var builder = WebApplication.CreateBuilder(args); // for ILogger (already defined)
builder.Logging.ClearProviders(); //
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



// NOTES:
 
// AddSingleton -->  Client istekte bulunduðunda DI Engine tek bir object oluþturur ve tüm isteklerde bunu kullanýr
// AddScoped    -->  Client istekte bulunduðunda DI Engine her bir istek için ayrý obje oluþturur, diðer tüm process Singleton ile aynýdýr.
// AddTransient -->  Componentler her bir istekte bulunduðunda (DI Engine'dan) ayrý bir obje oluþturur. Örneðin Controller 1 için object oluþturuldu
//                   o nesneden Controller 1' e dönüþ oldu ardýndan Controller1 Controller2'ye istekte bulundu Controller2 DI Engine'dan istekte bulunduðunda
//                   2. bir nesne oluþturulur.