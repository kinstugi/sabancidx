namespace backend.Models;

public class User{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordSalt { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public List<Product> Products { get; } = new List<Product>();

    public void AddProduct(Product product){
        Products.Add(product);
    }
}

// class to login User
public class UserAuthDTO{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
