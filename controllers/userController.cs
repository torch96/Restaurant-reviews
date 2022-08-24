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

        [HttpPost("/api/v1/users/register")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {

            var response = await _userRepository.CreateUserAsync(user.Name, user.Password,  user.Email);

            if( response.User != null) response.User.AuthToken = _jwtAuthentication.Value.GenerateToken(response.User);
            if(!response.Success)
            {
                return BadRequest(new { error = response.ErrorMessage });
            }
            return Ok(response.User);
        }
    

        [HttpPost("/api/v1/users/signin")]
        public async Task<ActionResult> Login([ FromBody] User user)
        {

            var theUser = new User{
                Email = user.Email,
                Password = user.Password,
                AuthToken = _jwtAuthentication.Value.GenerateToken(user)
            };
          
           
            var result = await _userRepository.LoginUserAsync(theUser.Email, theUser.Password, theUser.AuthToken);

           Console.WriteLine(result.User.AuthToken);
           Console.WriteLine(result.User.Email);
              Console.WriteLine(result.User.Password);
           
            if(result.Success)
            {
                return Ok(result.User);
            }
            return BadRequest(new { error = result.ErrorMessage });
            
        }

        [HttpPost("/api/v1/users/logout")]
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
