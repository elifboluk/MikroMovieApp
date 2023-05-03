using SharedLibrary.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption")); // appsettings.json i�erisindeki (Configuration ile appsettings'e eri�iyorum) TokenOption section'�n� al. CustomTokenOption s�n�f�, appsettings.json'daki TokenOption i�erisindeki parametreleri doldurup bir nesne �rne�i verecek. // Options Pattern


builder.Services.AddControllers();
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
