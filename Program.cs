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
 
// AddSingleton -->  Client istekte bulundu�unda DI Engine tek bir object olu�turur ve t�m isteklerde bunu kullan�r
// AddScoped    -->  Client istekte bulundu�unda DI Engine her bir istek i�in ayr� obje olu�turur, di�er t�m process Singleton ile ayn�d�r.
// AddTransient -->  Componentler her bir istekte bulundu�unda (DI Engine'dan) ayr� bir obje olu�turur. �rne�in Controller 1 i�in object olu�turuldu
//                   o nesneden Controller 1' e d�n�� oldu ard�ndan Controller1 Controller2'ye istekte bulundu Controller2 DI Engine'dan istekte bulundu�unda
//                   2. bir nesne olu�turulur.