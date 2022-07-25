using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace APIHavan.Test
{
    public class MessageTest
    {

        //criar configurações
        //testar mensagem enviada
        //testar mensagem não enviada
        

        [Test]
        public async Task testeEnviaMensagem()
        {
            //   var query = new MessageController();
            // MessageInputModel message = new MessageInputModel();

            //            message.FromId = 1;
            // message.CreatedAt = new System.DateTime(2020, 12, 12);
            //message.ToId = 2;
            // message.Content = "teste teste";

            //            var result = query.PostMessage(message);

            //var response = result as AcceptedResult;

            // Assert.AreEqual(202, response.StatusCode);
        }
    }
}