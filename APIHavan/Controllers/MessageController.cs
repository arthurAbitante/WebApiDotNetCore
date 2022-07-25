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
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "messages";
        private readonly IEmailSender _emailSender;


        public MessageController(IEmailSender emailSender)
        {
            _emailSender = emailSender;

            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        // POST api/<MessageController>
        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageInputModel message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailSender>>> sendMessages()
        {
            var message = new Message(new string[] { "jiwomi7721@5k2u.com" }, "Test email async", "This is the content from our async email.", null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

    }
}
