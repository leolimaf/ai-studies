using Pgvector;

namespace MyPgVectorStore.Web.Models;

public class Recommendation
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Vector Embedding { get; set; } = null!;
}