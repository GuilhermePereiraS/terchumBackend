using terchum;
using Microsoft.EntityFrameworkCore;
using terchum.service;
using terchum.ws;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddWebSocketInitializer();
builder.Services.AddSingleton<RoomManager>();
builder.Services.AddScoped<MessageBoardService>();

var app = builder.Build();
app.UseWebSockets();
WebSocketController.Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
