using ApiEmailHavan.Models;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public MessageController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailSender>>> sendMessages(Cliente cliente)
        {
            var message = new Message(new string[] { cliente.email }, "Test email async", "This is the content from our async email.", null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

    }
}
