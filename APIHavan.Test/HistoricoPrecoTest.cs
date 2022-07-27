using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiEmailHavan.Models;
using System.Collections.Generic;
using APIHavan.Test.Constants;

namespace APIHavan.Test
{
    public class HistoricoPrecoTest
    {
        private readonly IEmailSender _emailSender;
        
        [Test]
        public async Task testePegaTodosHistoricos()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPrecos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<HistoricoPreco>>>(result);
            Assert.AreEqual(3, result.Value.Count());

            Funcoes.RemoverListaHistorico(context);
            Funcoes.RemoverProduto(context, Funcoes.produto);
        }

        [Test]
        public async Task testePegaHistoricoPorId()
        {
            var historico = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPreco(1);

            Assert.AreEqual(historico.id, result.Value.id);

            Funcoes.RemoverListaHistorico(context);
            Funcoes.RemoverProduto(context, Funcoes.produto);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPreco(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            Funcoes.RemoverListaHistorico(context);
            Funcoes.RemoverProduto(context, Funcoes.produto);
        }

        [Test]
        public async Task RetornaNaoEncontradoQuandoIdEInvalido()
        {
            var historico = new HistoricoPreco { id = 1, Produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" }, preco = 10.2 };

            var context = new AppDbContext(Funcoes.options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.PutHistoricoPreco(99, historico);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(Funcoes.options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var resultDelete = await query.DeleteHistoricoPreco(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }
    }
}