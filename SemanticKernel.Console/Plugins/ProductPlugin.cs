using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SemanticKernel.Console.Plugins;

public class ProductPlugin
{
    private readonly List<Product> _products =
    [
        new(1, "Laptop", true, 999.99m),
        new(2, "Smartphone", false, 499.99m),
        new(3, "Tablet", true, 299.99m)
    ];
    
    [KernelFunction("get_products")]
    [Description("Returns a list of products")]
    public async Task <List<Product>> GetProductsAsync()
    {
        await Task.Delay(1);
        return _products.ToList();
    }
    
    [KernelFunction("get_product_by_id")]
    [Description("Returns a product by its ID")]
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await Task.Delay(1);
        return _products.FirstOrDefault(p => p.Id == id);
    }
}

public record Product(int Id, string Name, bool IsActive, decimal Price);