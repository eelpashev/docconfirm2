using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LawFakeApi.Controllers
{
    [Produces("application/json")]
    public class ValuesController : Controller
    {        
        [HttpPost("/requests/{id}")]
        public Object Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if (id.Length > 10)
                return NotFound();

            return new {
                notaryId = 942000005,
                docDate = DateTime.Now.ToShortDateString(),
                docNum = new Guid().ToString(),
                docBody = "string"
                };
        }
    }
}
