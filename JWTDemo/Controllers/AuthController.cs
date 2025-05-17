using JWTDemo.Data;
using JWTDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JWTDemo.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            // 示例：验证用户名和密码（实际应连接数据库）
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = GenerateJwtToken();
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult SecureData()
        {
            return Ok("这是需要认证的API数据");
        }


        [HttpGet("GetData")]
        //[Authorize]
        [EnableCors("AllowAll")]
        public IEnumerable<Student> GetData()
        {
            string sFilePath = Path.Combine(AppContext.BaseDirectory, "Data/data.json");
            using var stream =System.IO.File.OpenRead(sFilePath);
            var data = JsonSerializer.Deserialize<IEnumerable<Student>>(stream);
            return data;
        }

        [HttpGet("GetToken1")]
        public AuthModel GetToken1()
        {
            return new AuthModel
            {
                access_token = Guid.NewGuid().ToString(),
                token_type = "T",
                expires_in = 3600,
                scope = "test",
                error = "",
                error_description = ""
            };
        }

        [HttpPost ("GetToken2")]
        public AuthModel GetToken2()
        {
            return new AuthModel
            {
                access_token = Guid.NewGuid().ToString(),
                token_type = "T2",
                expires_in = 4000,
                scope = "test2",
                error = "",
                error_description = ""
            };
        }

        private string GenerateJwtToken()
        {
            var secretKey = _config["Jwt:SecretKey"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiryInHours = int.Parse(_config["Jwt:TokenExpiryInHours"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(ClaimTypes.Name, "admin")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expiryInHours),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
