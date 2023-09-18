using System.Text.Json.Serialization;

namespace backend.Models;

public enum Currency{
    TRY, 
    USD,
    EUR
}

public class Product{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Currency Currency { get; set; } = Currency.TRY;
    public int StockCount { get; set; }
    [JsonIgnore]
    public bool IsVisible { get; set; } = true; // set this to false when a product is deleted

    public bool DeleteProduct(int userId){
        if (userId != UserId) return false;
        IsVisible = false;
        return true;
    }
}

// productDTO
public class ProductDTO{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Currency Currency { get; set; } = Currency.TRY;
    public int StockCount { get; set; }
}