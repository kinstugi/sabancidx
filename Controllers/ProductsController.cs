using System.Security.Claims;
using backend.Models;
using backend.Repository;
using backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController: ControllerBase{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductRepository _productRepo;

    public ProductsController(ProductRepository productRepository, ILogger<ProductsController> logger){
        _productRepo = productRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Product>> FetchAllProducts(){
        var page = Request.Query["page"];
        return await _productRepo.GetAllProducts(null);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(int id){
        try{
            var product = await _productRepo.GetProductAsync(id);
            return Ok(product);
        }catch(ProductNotFoundException){
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductDTO productDTO){
        var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value);
        var createdProduct = await _productRepo.CreateProduct(userId, productDTO);
        return CreatedAtAction(nameof(GetProduct),new { id = createdProduct.ProductId }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO){
        var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value);
        try{
            var res = await _productRepo.UpdateProduct(userId, id, productDTO);
            return Ok(res);
        }catch(ProductNotFoundException){
            return NotFound();
        }catch(UnAuthorizedAccessException){
            return Unauthorized();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id){
        var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.PrimarySid)!.Value);
        try{
            var res = await _productRepo.DeleteProduct(userId, id);
            return Ok("product deleted");
        }catch(UnAuthorizedAccessException){
            return Unauthorized();
        }catch(ProductNotFoundException){
            return NotFound();
        }
    }
}