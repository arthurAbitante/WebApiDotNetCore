using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHavan.Models;

namespace WebHavan.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
