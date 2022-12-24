using BookLibrary.Domain.Services;
using BookLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureServices(builder);

var app = builder.Build();

Configure(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    //Log.Information("Configure Services");
    builder.Services.AddDbContext<BookContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionString:BookLibDB"]));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Library API", Version = "v1" });
    });
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IBookService, BookService>();
    builder.Services.AddScoped<IBookRepository, BookRepository>();
}
static void Configure(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}
public partial class Program { }