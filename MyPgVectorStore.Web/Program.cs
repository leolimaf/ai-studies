using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using MyPgVectorStore.Web.Data;
using MyPgVectorStore.Web.Models;
using MyPgVectorStore.Web.ViewModels;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, o => o.UseVector());
});

builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient("http://localhost:11434", "mxbai-embed-large"));

var app = builder.Build();

app.MapGet("/v1/seed", async (AppDbContext context, OllamaApiClient ollamaApiClient) =>
{
    var products = await context.Products.ToListAsync();

    foreach (var product in products)
    {
        var service = ollamaApiClient.AsTextEmbeddingGenerationService();
        var embeddings = await service.GenerateEmbeddingAsync(product.Category);

        var recommendation = new Recommendation
        {
            Title = product.Title,
            Category = product.Category,
            Embedding = new Vector(embeddings)
        };

        await context.Recommendations.AddAsync(recommendation);
        await context.SaveChangesAsync();
    }
});

app.MapPost("v1/products", async (CreateProductViewModel model, AppDbContext context, OllamaApiClient ollamaApiClient) =>
{
    var service = ollamaApiClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(model.Category);
    
    var recommendation = new Recommendation
    {
        Title = model.Title,
        Category = model.Category,
        Embedding = new Vector(embeddings)
    };
    
    var product = new Product
    {
        Title = model.Title,
        Category = model.Category,
        Summary = model.Summary,
        Description = model.Description
    };
    
    await context.Recommendations.AddAsync(recommendation);
    await context.Products.AddAsync(product);
    await context.SaveChangesAsync();
});

app.MapPost("v1/prompt", async (QuestionViewModel model, AppDbContext context, OllamaApiClient ollamaApiClient) =>
{
    var service = ollamaApiClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(model.Prompt);
    
    var recommendations = await context.Recommendations
        .AsNoTracking()
        .OrderBy(x => x.Embedding.CosineDistance(new Vector(embeddings.ToArray())))
        .Take(3)
        .Select(x => new { x.Title, x.Category })
        .ToListAsync();
    
    return Results.Ok(recommendations);
});

app.Run();