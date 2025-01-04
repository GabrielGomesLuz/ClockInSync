using ClockInSync.Repositories.ClockInSync.Dtos.User;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClockInSync.API.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/[controller]/v1")]
    [ApiController]
    public class UserController(IUserService userService) : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto userCreationDto)
        {
            if (userCreationDto == null)
                return BadRequest("Invalid user data");

            var userExists = await userService.VerifyUserExistsByEmailAsync(userCreationDto.Email);

            if (userExists)
                return BadRequest("Email já cadastrado!");

            var response = await userService.CreateUserAsync(userCreationDto);
            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(UserLoginDto userLoginDto)
        {

            if (userLoginDto == null)
                return BadRequest("Dados inválidos!");

            var userExists = await userService.VerifyUserExistsByEmailAsync(userLoginDto.Email);

            if (userExists)
            {
                var passwordMatch = await userService.LoginUserAsync(userLoginDto);
                if (passwordMatch != null)
                    return Ok(passwordMatch);
                return BadRequest("Senha inválida.");

            }

            return BadRequest("Email não cadastrado no sistema.");
        }
    }
}
