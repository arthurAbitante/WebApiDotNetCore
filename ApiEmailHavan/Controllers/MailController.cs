using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebHavan.Models;
using WebHavan.Services;

namespace WebHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;

        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm]MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch(Exception ex) 
            {
                throw;
            }
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> teste(MailRequest email, string fromEmail)
        {

            using (MailMessage message = new MailMessage(fromEmail,email.ToEmail))
            {
                message.Subject = email.Subject;
                message.Body = email.Body;
                message.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential cred = new NetworkCredential(fromEmail,"4rthur4bit4nt&");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = cred;
                    smtp.Port = 587;
                    smtp.Send(message);
                    //ViewBag.Message = "Mensagem Enviada com sucesso"; mandar como response
                }
            }
            return Ok();
        }



    }
}
