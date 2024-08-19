using crud_app.Controllers.Request;
using crud_app.Domain;
using crud_app.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace crud_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isLoggedIn = await _userRepository.Login(request.Username, request.Password);
            if (isLoggedIn is not null)
            {
                return Ok("Logged in successfully");
            }
            return Unauthorized(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if(ModelState.IsValid)
            {
                await _userRepository.CreateAndSave(user);
                return Created(string.Empty, user);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.UpdateAndSave(user);
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.DeleteAndSave(id);
            return NoContent();
        }
    }
}
