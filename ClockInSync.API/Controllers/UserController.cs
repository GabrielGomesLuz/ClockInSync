using ClockInSync.Repositories.ClockInSync.Dtos.User;
using ClockInSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClockInSync.API.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/[controller]/v1")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody]UserCreationDto userCreationDto)
        {
            if(userCreationDto != null)
            {
                var response = await userService.CreateUserAsync(userCreationDto);
                return Ok(response);
            }
            return BadRequest("Invalid user data");
        }
    }
}
