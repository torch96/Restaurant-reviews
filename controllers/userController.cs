using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReview.Models;
using RestaurantReview.Models.Responses;
using RestaurantReview.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace RestaurantReview.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly UserRepository _userRepository;
        private readonly ReviewRepository _reviewsRepository;

        public UserController(UserRepository usersRepository,
            ReviewRepository reviewsRepository,
            IOptions<JwtAuthentication> jwtAuthentication)
        {
            _userRepository = usersRepository;
            _reviewsRepository = reviewsRepository;
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

    
        [HttpGet]
        public async Task<ActionResult> Get( string email)
        {
            var user = await _userRepository.GetUserAsync(email);
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            return Ok(user);
        }

        [HttpPost("/api/v1/user/register")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (user.Name.Length < 3)
            {
                errors.Add("name", "Name must be at least 3 characters");
            }
            if (user.Email.Length < 3)
            {
                errors.Add("email", "Email must be at least 3 characters");
            }
            if (user.Password.Length < 3)
            {
                errors.Add("password", "Password must be at least 3 characters");
            }
            if (errors.Count > 0)
            {
                return BadRequest(new {error = errors});
            }
            var response = await _userRepository.CreateUserAsync(user.Name, user.Password,  user.Email);
           if( response.User != null) response.User.AuthToken = _jwtAuthentication.Value.GenerateToken(response.User);
            if(!response.Success)
            {
                return BadRequest(new { error = response.ErrorMessage });
            }
            return Ok(user);
        }
    

        [HttpPost("/api/v1/user/signin")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            var result = await _userRepository.LoginUserAsync(user);
            return  Ok(new UserResponse(result.User));
        }

        [HttpPost("/api/v1/user/logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Logout()
        {
            var email = GetUserEmailFromToken(Request);
            if (email.StartsWith("Error")) return BadRequest(email);

            var result = await _userRepository.LogoutUserAsync(email);
            return Ok(result);
        }

        private static string GetUserEmailFromToken(HttpRequest request)
        {
            var bearer = request.Headers.ToArray().FirstOrDefault(x => x.Key == "Authorization").Value.First().Substring(7);
        
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(bearer);

            var claims = token.Claims;

            var email = claims.FirstOrDefault(x => x.Type == "email").Value;

            return email;
        }
        public static async Task<User> GetUserFromTokenAsync( UserRepository userRepository, HttpRequest request)
        {
            var email = GetUserEmailFromToken(request);
            
            return await userRepository.GetUserAsync(email);
        }
    }

    public class PasswordObject
    {
        public string Password { get; set; }
    }
}
