using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace logic_app_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StampsController : ControllerBase
    {
        public StampsController()
        {

        }

        [HttpPost]
        [Authorize]
        public string StampsReportEmulator()
        {
            return "Success";
        }
    }
}
