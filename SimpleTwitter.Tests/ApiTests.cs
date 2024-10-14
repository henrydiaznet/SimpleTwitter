using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SimpleTwitter.Api.Contracts;
using SimpleTwitter.Api.Infrastructure;
using SimpleTwitter.Api.Infrastructure.Entity;

namespace SimpleTwitter.Tests;

public class ApiTests: IClassFixture<SimpleTwitterWebApplicationFactory>
{
    private readonly SimpleTwitterDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly Fixture _fixture = new();

    public ApiTests(SimpleTwitterWebApplicationFactory factory)
    {
        _dbContext = factory.Services.GetRequiredService<SimpleTwitterDbContext>();
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("http://localhost:8080")
        });
     }
    
    [Fact]
    public async Task GetById_ReturnsOk()
    {
        //arrange
        var expected = _fixture
            .Build<Post>()
            .Without(x => x.Id)
            .Create();
        
        var entity = _dbContext.Posts.Add(expected);
        await _dbContext.SaveChangesAsync();
        
        //act
        var response = await _client.GetAsync($"/posts/{entity.Entity.Id}");
        
        //assert
        response.EnsureSuccessStatusCode();
        var actual = await response.Content.ReadFromJsonAsync<Post>();
        Assert.NotNull(actual);
        Assert.Equal(entity.Entity.Id, actual.Id);
        Assert.Equal(expected.Content, actual.Content);
        Assert.Equal(expected.Username, actual.Username);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound()
    {
        //arrange
        var expected = _fixture
            .Build<Post>()
            .Create();
        
        //act
        var response = await _client.GetAsync($"/posts/{expected.Id}");
        
        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        //arrange
        var expected = _fixture
            .Build<Post>()
            .Without(x => x.Id)
            .CreateMany(10)
            .ToArray();
        _dbContext.Posts.AddRange(expected);
        await _dbContext.SaveChangesAsync();
        
        //act
        var response = await _client.GetAsync($"/posts");
        
        //assert
        response.EnsureSuccessStatusCode();
        var actual = await response.Content.ReadFromJsonAsync<Post[]>();
        Assert.NotNull(actual);
        actual.Length.Should().Be(10);
        actual.Should().BeEquivalentTo(expected.OrderByDescending(x => x.CreatedDate));
    }
    
    [Fact]
    public async Task Post_ReturnsOk()
    {
        //arrange
        var post = _fixture
            .Build<AddPostRequest>()
            .Create();
        
        //act
        var response = await _client.PostAsJsonAsync($"/posts", post);
        
        //assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadAsStringAsync();
        Assert.NotNull(actual);
    }
    
    [Fact]
    public async Task Post_ReturnsBadRequest()
    {
        //arrange
        var post = _fixture
            .Build<AddPostRequest>()
            .Without(x => x.Content)
            .Without(x => x.Username)
            .Create();
        
        //act
        var response = await _client.PostAsJsonAsync($"/posts", post);
        
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationErrors = await response.Content.ReadAsStringAsync();
        Assert.NotNull(validationErrors);
    }
}