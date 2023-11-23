using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : APIBaseController
    {
        private readonly StoreDbContext dbContext;

        public BuggyController(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var prod=dbContext.Products.Find(100);
            if(prod ==null) return NotFound(new APIResponse(404));
            return Ok(prod);
        }

        [HttpGet("ServerError")]
        public ActionResult GetServerErrorRequest()
        {
            var prod = dbContext.Products.Find(100);
            var ProdToReturn = prod.ToString();
            return Ok(ProdToReturn);
        }


        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
           return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetbadRequest(int id)
        {
            return Ok();
        }


    }
}
