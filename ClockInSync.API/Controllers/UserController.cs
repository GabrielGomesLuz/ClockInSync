using ClockInSync.Repositories.ClockInSync.Dtos.User;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
                return BadRequest(new { message = "Senha inválida" });

            }

            return BadRequest(new { message = "Email não cadastrado no sistema." });
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] int offset, [FromQuery] int limit)
        {
            return Ok(await userService.GetUsersInformationAsync(offset, limit));
        }

        [HttpGet("user/infos")]
        public async Task<ActionResult> GetAllInfosUser([Required][FromHeader] Guid userId)
        {
            return Ok(await userService.GetUserAllDetails(userId));
        }

        [HttpPut("put/user/edit")]
        public async Task<ActionResult> PutUser([FromBody]UserEditDto userEditDto)
        {
            if(userEditDto == null)
                return BadRequest("Ocorreu um erro ao atualizar os dados");

            var message = await userService.UpdateUserAsync(userEditDto);
                
            return Ok(new { message });



        }

        [HttpGet("user/edit/infos")]
        public async Task<ActionResult> GetInfoToEditUser([FromHeader] Guid userId)
        {
            return Ok(await userService.GetUserInfoToEditAsync(userId));
        }
       
    }
}
