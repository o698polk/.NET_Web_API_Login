using ApisEjmploApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ApisEjmploApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserApi : ControllerBase
    {
        private readonly generaldbContext _context;
        public IConfiguration _configuration;
        public UserApi(generaldbContext context, IConfiguration iConfiguration)
        {
            _context = context;
            _configuration = iConfiguration;
        }
        [HttpGet]
        [Route("Datos")]
        [Authorize]
        public dynamic GetAllUser()
        {
            try
            {
                var ident = HttpContext.User.Identity as ClaimsIdentity;
            
                if (ident.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Error: Token incorrecto, Verifique su Token" ,
                        result = ""
                    };
                }
                else
                {
                   /* var id = ident.Claims.FirstOrDefault(x => x.Type == "id").Value;
                    var userdata = _context.Userdts.Where(x => x.UserId.ToString() == id);
                   */
                    return new
                    {
                        success = true,
                        message = "Exito",
                        result = _context.Userdts.ToList()
                    };
                }
               
            }
            catch(Exception ex)
            {
                return new
                {
                    success = true,
                    message = "Error"+ex,
                    result = ""
                };
            }
            
        }
        [HttpPost]
        [Route("login")]
        public dynamic LoginUser([FromBody] Object datos)
        {
            var dat = JsonConvert.DeserializeObject<dynamic>(datos.ToString());
            string user = dat.nameUsuarios.ToString();
            string password = dat.password.ToString();

            Userdt usd = _context.Userdts.Where(x => x.NameUser == user && x.PasswordUser == password).FirstOrDefault();
     

            if(usd == null)
            {
                return new
                {
                    success = false,
                    message = "Cedenciales incorrectas",
                    result = ""

                };
            }
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                 new Claim("id",usd.UserId.ToString()),
                  new Claim("user",usd.NameUser)
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: singIn

                );
            return new
            {
                success = true,
                message = "exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }
        }
}
