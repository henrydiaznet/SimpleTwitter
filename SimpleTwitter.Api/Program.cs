using Microsoft.EntityFrameworkCore;
using SimpleTwitter.Api.Contracts;
using SimpleTwitter.Api.Extensions;
using SimpleTwitter.Api.Filters;
using SimpleTwitter.Api.Infrastructure;
using SimpleTwitter.Api.Infrastructure.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SimpleTwitterDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        pb => pb.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAllOrigins");
}

app.UseHttpsRedirection();
app.InitializeDb();

app.MapGet("/posts", async ( SimpleTwitterDbContext dbContext) =>
    {
        var result = await dbContext.Posts
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
        
        return result;
    })
    .WithName("GetAllPosts")
    .WithSummary("Get all posts")
    .WithOpenApi();

app.MapGet("/posts/{id:long}", async ( long id, SimpleTwitterDbContext dbContext) =>
    {
        var result = await dbContext.Posts.FindAsync(id);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetById")
    .WithSummary("Get a Post by id")
    .WithOpenApi();

app.MapPost("/posts", async ( AddPostRequest request, SimpleTwitterDbContext dbContext) =>
    {
        var toAdd = new Post
        {
            Content = request.Content,
            Username = request.Username,
        };
        
        var result = await dbContext.Posts.AddAsync(toAdd);
        await dbContext.SaveChangesAsync();
        
        return Results.Created("/posts/{id}", new {id = result.Entity.Id});
    })
    .AddEndpointFilter<ValidationFilter<AddPostRequest>>()
    .WithName("AddPost")
    .WithSummary("Add a new post")
    .WithOpenApi();

app.Run();

public partial class Program 
{ 
//required for integration tests
}
