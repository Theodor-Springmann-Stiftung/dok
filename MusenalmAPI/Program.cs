using MusenalmAPI.Data;
using Microsoft.EntityFrameworkCore;

// iCo Reset
// if (File.Exists("./TEST.db"))
//     File.Delete("./Test.db");
// if (File.Exists("./TEST.db-shm")) 
//     File.Delete("./TEST.db-shm");
// if (File.Exists("./TEST.db-wal"))
//     File.Delete("./TEST.db-wal");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MusenalmContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MusenalmSQLite")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    // app.UseMigrationsEndPoint(); ???
}

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MusenalmContext>();
    context.Database.EnsureCreated();
    // Seed the DB
    DbInitializer.Initialize(context);
}

// iCo app.UseStaticFiles(); Static Files Routing e.g. images
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
