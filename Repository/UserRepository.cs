using backend.Data;
using backend.Models;
using backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository;

public class UserRepository{
    private readonly AppDbContext dbContext;
    private readonly IConfiguration _configuration;
    public UserRepository(AppDbContext appDbContext, IConfiguration configuration){
        dbContext = appDbContext;
        _configuration = configuration;
    }

    public async Task<bool> CreateUser(UserAuthDTO userAuth){
        var query = dbContext.Users.Where(u => u.Email == userAuth.Email).ToList();
        if (query.Count > 0) return false; // user already exists
        byte[] passwordSalt, passwordHash;
        AuthMethods.CreatePasswordHash(userAuth.Password, out passwordSalt, out passwordHash);
        var nUser = new User {
            Email = userAuth.Email, 
            PasswordHash = passwordHash, 
            PasswordSalt = passwordSalt
        };
        await dbContext.Users.AddAsync(nUser);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<string> AuthenticateUser(UserAuthDTO userAuthDTO){
        var dbUser = await dbContext.Users.Where(u=> u.Email == userAuthDTO.Email).FirstOrDefaultAsync();
        if (dbUser == null)
            throw new Exception("account does not exist, please check creds and try again");
        if (!AuthMethods.VerifyPasswordHash(userAuthDTO.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
            throw new Exception("account does not exist, please check creds and try again");
        try
        {
            var token = AuthMethods.CreateAuthToken(dbUser, _configuration);
            return token;   
        }catch (Exception ex){
            throw ex;
        }
    }
}