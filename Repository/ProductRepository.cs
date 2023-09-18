using backend.Data;
using backend.Models;

namespace backend.Repository;

public class ProductRepository{
    private readonly AppDbContext dbContext;

    public ProductRepository(AppDbContext appDbContext){
        dbContext = appDbContext;
    }

    public List<Product> GetAllProducts(User? user, int pageNumber = 1, int itemPerPage = 20){
        if (user != null){
            return dbContext.Products
                .Where(product => product.UserId == user.UserId && product.IsVisible)
                .Skip((pageNumber-1)*itemPerPage)
                .Take(itemPerPage)
                .ToList();
        }
        return dbContext.Products
            .Where(product => product.IsVisible)
            .Skip((pageNumber-1)*itemPerPage)
            .Take(itemPerPage)
            .ToList();
    }

    public Product CreateProduct(User user){
        return new Product();
    }
}