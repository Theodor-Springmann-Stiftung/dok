using MusenalmAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

// iCo Reset
// if (File.Exists("./TEST.db"))
//     File.Delete("./Test.db");
// if (File.Exists("./TEST.db-shm")) 
//     File.Delete("./TEST.db-shm");
// if (File.Exists("./TEST.db-wal"))
//     File.Delete("./TEST.db-wal");

var builder = WebApplication.CreateBuilder(args);
var  DevelopmentOrigin = ( "LocalHostTestOrigin", "http://127.0.0.1:3000" );
var DevelopmentDBConnect = "MusenalmSQLite";
builder.Services.AddCors(options => 
    options.AddPolicy(name: DevelopmentOrigin.Item1, policy  => 
        policy.WithOrigins(DevelopmentOrigin.Item2)
            .AllowAnyHeader()
            .AllowAnyMethod()
));
builder.Services.AddDbContext<MusenalmContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString(DevelopmentDBConnect)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors(DevelopmentOrigin.Item1);
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