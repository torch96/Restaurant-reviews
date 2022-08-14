using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurant.Models;
using Restaurant.Models.Responses;
using Restaurant.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Restaurnt.Controllers
{
    [route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly UsersRepository _userRepository;
        private readonly ReviewssRepository _reviewsRepository;

        public UserController(UsersRepository usersRepository,
            ReviewsRepository reviewsRepository,
            IOptions<JwtAuthentication> jwtAuthentication)
        {
            _userRepository = usersRepository;
            _reviewsRepository = reviewsRepository;
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

    
        [HttpGet]
        public async Task<ActionResult> Get([RequiredFromQuery] string email)
        {
            var user = await _userRepository.GetUserAsync(email);
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            return Ok(user);
        }

        [HttpPost("/api/v1/user/signup")]
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
                return BadRequest(new ErrorResponse(errors));
            }
            await _userRepository.AddUserAsync(user);
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            return Ok(user);
        }
    }

    [HttpPost("/api/v1/user/login")]
    public async Task<ActionResult> Login([FromBody] User user)
    {
        user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
        var result = await _userRepository.LoginUserAsync(user);
        return result.User != null ? Ok(new UserResponse(result.User)) : Ok(result);
    }

    [HttpPost("/api/v1/user/logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Logout()
    {
        var user = await _userRepository.GetUserAsync(User.Identity.Name);
        user.AuthToken = null;
        await _userRepository.UpdateUserAsync(user);
        return Ok();
    }
}
