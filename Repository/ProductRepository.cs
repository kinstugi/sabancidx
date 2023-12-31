using backend.Data;
using backend.Models;
using backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository;
public interface IProductRepository{
    public Task<List<Product>> GetAllProducts(int userId = -1, int pageNumber = 1, int pageSize = 10);
    public Task<Product> GetProductAsync(int productId);
    public  Task<Product> CreateProduct(int userId, ProductDTO productDTO);
    public Task<Product> UpdateProduct(int userId,int productId, ProductDTO productDTO);
    public  Task<bool> DeleteProduct(int userId, int productId);
    public Task<List<Product>> FilterProducts(
        string name = "",
        string code = "",
        string brand = "",
        double minPrice = double.NegativeInfinity,
        double maxPrice = double.PositiveInfinity,
        int page = 1,
        int pageSize = 10 
    );
}

public class ProductRepository: IProductRepository{
    private readonly AppDbContext dbContext;

    public ProductRepository(AppDbContext appDbContext){
        dbContext = appDbContext;
    }

    public async Task<List<Product>> GetAllProducts(int userId = -1, int pageNumber = 1, int itemPerPage = 20){
        if (userId != -1){
            return await dbContext.Products
                .Where(product => product.UserId == userId && product.IsVisible)
                .Skip((pageNumber-1)*itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();
        }
        return await dbContext.Products
            .Where(product => product.IsVisible)
            .Skip((pageNumber-1)*itemPerPage)
            .Take(itemPerPage)
            .ToListAsync();
    }

    public async Task<Product> GetProductAsync(int productId){
        var product = await dbContext.Products.Where(pd => pd.ProductId == productId && pd.IsVisible).FirstOrDefaultAsync();
        if (product == null) throw new ProductNotFoundException();
        return product;
    }

    public async Task<Product> CreateProduct(int userId, ProductDTO productDTO){
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null) throw new Exception();

        var product = new Product{ 
            UserId = userId, 
            Brand = productDTO.Brand,
            Price = productDTO.Price,
            Currency = productDTO.Currency,
            Name = productDTO.Name,
            Description = productDTO.Description,
            StockCount = productDTO.StockCount,
            Code = productDTO.Code
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        user.AddProduct(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProduct(int userId,int productId, ProductDTO productDTO){
        var product = await dbContext.Products.Where(pd=> pd.ProductId == productId && pd.IsVisible).FirstOrDefaultAsync();
        if (product == null) throw new ProductNotFoundException();
        if (product.UserId != userId) throw new UnAuthorizedAccessException();
        product.Brand =  productDTO.Brand;
        product.Name = productDTO.Name;
        product.Currency = productDTO.Currency;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        product.StockCount = productDTO.StockCount;
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(int userId, int productId){
        var product = await dbContext.Products.Where(pd=> pd.ProductId == productId && pd.IsVisible).FirstOrDefaultAsync();
        if (product == null) throw new ProductNotFoundException();
        if (product.UserId != userId) throw new UnAuthorizedAccessException();
        var res = product.DeleteProduct(userId);
        await dbContext.SaveChangesAsync();
        return res;
    }

    public async Task<List<Product>> FilterProducts(
        string name = "",
        string code = "",
        string brand = "",
        double minPrice = double.NegativeInfinity,
        double maxPrice = double.PositiveInfinity,
        int page = 1,
        int pageSize = 10 
    ){
        var productQuery = dbContext.Products.Where(pd => minPrice <= pd.Price && pd.Price <= maxPrice);
        productQuery = productQuery.Where(pd => pd.IsVisible);
        if (!string.IsNullOrEmpty(name))
            productQuery = productQuery.Where(pd => pd.Name == name);
        if (!string.IsNullOrEmpty(code))
            productQuery = productQuery.Where(pd=> pd.Code == code);
        if (!string.IsNullOrEmpty(brand))
            productQuery = productQuery.Where(pd => pd.Brand == brand);
        
        return await productQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}