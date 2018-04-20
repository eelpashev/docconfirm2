using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiMQSender.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiMQSender.Controllers
{
    [Produces("application/json")]
    public class RequestController : Controller
    {
        private readonly IMQSender _sender;

        public RequestController(IMQSender sender)
        {
            _sender = sender;
        }

        [HttpGet("/requests/{id}")]
        public ActionResult Proceed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            _sender.SendMessage(id);

            return Ok();

        }
    }
}