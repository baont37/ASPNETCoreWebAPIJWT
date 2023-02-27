using ASPNETCoreWebAPIJWT.Model;
using ASPNETCoreWebAPIJWT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace ASPNETCoreWebAPIJWT.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IJWTManagerRepository _jWTManager;

        public UsersController(IJWTManagerRepository jWTManager)
        {
            this._jWTManager = jWTManager;
        }

        [HttpGet]
        public List<string> Get()
        {
            var users = new List<string>
        {
            "Thai Bao",
            "Loc Tan",
            "Huu Tuan"
        };

            return users;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] User usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrent()
        {
            var authHeader = this.HttpContext.Request.Headers["Authorization"].ToString();
            var parts = authHeader.Split(' ');
            var accessToken = parts[1];

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var payload = token.Payload;


            return Ok(_jWTManager.GetUserById(payload["user_id"].ToString()));
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("newToken")]
        public IActionResult NewToken(Token token)
        {
            if (_jWTManager.ValidateToken(token.RefreshToken) == true)
            {
                Token newToken = _jWTManager.NewAccessToken();          
                return Ok(newToken);
            }
            return null;
        }

    }
}
