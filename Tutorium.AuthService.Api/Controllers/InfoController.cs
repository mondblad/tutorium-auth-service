using Microsoft.AspNetCore.Mvc;

namespace Tutorium.AuthService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        /// <summary>
        /// Получение информации
        /// </summary>
        [HttpGet(template: "version")]
        public async Task<ActionResult<string>> GetCounterObjectMeasuringValues()
        {
            return "Auth Service 1.1";
        }
    }
}
