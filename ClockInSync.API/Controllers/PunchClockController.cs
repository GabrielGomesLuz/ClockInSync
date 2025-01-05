using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Services.PunchClockServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClockInSync.API.Controllers
{

    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/[controller]/v1")]
    [ApiController]
    public class PunchClockController(IPunchClockService _clockService) : Controller
    {
        [HttpPost("punch-clock")]
        public async Task<ActionResult> RegisterPunchClock(RegisterPunchClock registerPunchClock)
        {

            if (registerPunchClock == null)
                return BadRequest("Dados inválidos.");


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return NotFound("User não encontrado.");
            var isSucess = await _clockService.RegisterPunchClock(registerPunchClock, Guid.Parse(userId!));

            if(isSucess)
                return Ok(new PunchClockResponse { Message = "Ponto registrado com sucesso",TimeStamp = DateTime.Now});

            return BadRequest("Não foi possível registrar o seu ponto.");


        }


        [HttpGet("history")]
        public async Task<ActionResult> GetHystoryPunchClocks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return BadRequest("User não encontrado.");

            var punchClocksSummaries = await _clockService.GetPunchClockSummaries(Guid.Parse(userId));
            return Ok(punchClocksSummaries);
        }


        [HttpGet("admin/punch-clock")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllPunchClocks([FromQuery] Guid employeeId , [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var allPunchClocks = await _clockService.GetPunchClockAll(employeeId, startDate, endDate);
            return Ok(allPunchClocks);
        }

        [HttpGet("admin/reports")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ExportAllPunchClocks([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var exportAllPunchClocks = await _clockService.ExportPunchClockAll(startDate, endDate);
            // Retornar o CSV como um arquivo
            var fileName = $"PunchClocks_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(exportAllPunchClocks, "text/csv", fileName);
        }
    }
}
