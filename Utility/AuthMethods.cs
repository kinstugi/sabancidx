using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Utility;

public class AuthMethods{
    public static void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash){
        using(var hmac = new HMACSHA512()){
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt){
        using (var hmac = new HMACSHA512(passwordSalt)){
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    public static string CreateAuthToken(User user, IConfiguration configuration){
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.PrimarySid, user.UserId.ToString())
        };
        string? secretKey = configuration.GetSection("AppSettings:SecretKey").Value;
        if (secretKey  == null)
            throw new Exception("could not found secret key");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokens = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(2),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokens);
    }
}