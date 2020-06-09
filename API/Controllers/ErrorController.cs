using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("errors/{code}")]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code) => new ObjectResult(new ApiResponse(code));
    }

}