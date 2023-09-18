using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public UserController(AppDbContext context, ILogger logger){
        _context = context;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserAuthDTO userLogin){
        await Task.Delay(1);
        return Ok();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> CreateUser(UserAuthDTO userAuth){
        await Task.Delay(1);
        return Ok();
    }
}