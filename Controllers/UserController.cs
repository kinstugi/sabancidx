using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase{
    private readonly UserRepository _userRepo;
    private readonly ILogger<UserController> _logger;

    public UserController(UserRepository userRepository, ILogger<UserController> logger){
        _userRepo = userRepository;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserAuthDTO userLogin){
        try{
            var token = await _userRepo.AuthenticateUser(userLogin);
            return Ok(token);
        }catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> CreateUser(UserAuthDTO userAuth){
        bool res = await _userRepo.CreateUser(userAuth);
        if (!res) return BadRequest("an account with similar details already exists");
        return Ok("account created");
    }
}