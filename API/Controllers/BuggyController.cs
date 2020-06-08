using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context) => _context = context;

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
           Product thing = _context.Products.Find(42);

           if (thing == null)
           {
               return NotFound();
           }
            return Ok();
        }
        
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            Product thing = _context.Products.Find(42);

            string thingToReturn = thing.ToString();
            
            return Ok();
        }
        
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }

}