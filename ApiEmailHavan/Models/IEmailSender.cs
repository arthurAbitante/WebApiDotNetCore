using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmailHavan.Models
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
