using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using APIHavan.Test.Constants;

namespace APIHavan.Test
{
    public class Tests
    {     
        [Test]
        public async Task testePegaTodasCondicoesPagamentos()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularCondicao(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicoesPagamentos();
            //o assert deverá contar a lista
            Assert.AreEqual(6, result.Value.Count());

            Funcoes.RemoverListaCondicao(context);
        }

        [Test]
        public async Task testePegaCondicoesPagamentosPorId()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularCondicao(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicaoPagamento(1);

            Assert.AreEqual(condicaoPagamento.condicaoPagamentoId, result.Value.condicaoPagamentoId);

            Funcoes.RemoverListaCondicao(context);
        }

        [Test]
        public async Task testeInsereCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(Funcoes.options);

            var query = new CondicaoPagamentosController(context);

            var result = await query.PostCondicaoPagamento(condicaoPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as CondicaoPagamento;

            Assert.AreEqual(1, item.condicaoPagamentoId);

            Funcoes.RemoverCondicao(context, item);
        }

        [Test]
        public async Task testeEditaCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(Funcoes.options);

            var query = new CondicaoPagamentosController(context);

            await query.PostCondicaoPagamento(condicaoPagamento);

            condicaoPagamento.descricao = "Atrasado";

            var resultNovo = await query.PutCondicaoPagamento(condicaoPagamento.condicaoPagamentoId, condicaoPagamento) as StatusCodeResult;
               
            Assert.AreEqual(204, resultNovo.StatusCode);

            Funcoes.RemoverCondicao(context, condicaoPagamento);
        }

        [Test]
        public async Task testeRemoveCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(Funcoes.options);

            var query = new CondicaoPagamentosController(context);

            var result = await query.PostCondicaoPagamento(condicaoPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as CondicaoPagamento;

            Assert.AreEqual(1, item.condicaoPagamentoId);

            var resultDelete = await query.DeleteCondicaoPagamento(condicaoPagamento.condicaoPagamentoId) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }

        [Test]
        public async Task RetornaTipoCorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularCondicao(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicoesPagamentos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<CondicaoPagamento>>>(result);
            Funcoes.RemoverListaCondicao(context);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularCondicao(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicaoPagamento(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            Funcoes.RemoverListaCondicao(context);
        }

        [Test]
        public async Task RetornaNaoEncontradoQuandoIdEInvalido()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(Funcoes.options);

            var query = new CondicaoPagamentosController(context);

            var result = await query.PutCondicaoPagamento(99, condicaoPagamento);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(Funcoes.options);

            var query = new CondicaoPagamentosController(context);

            var resultDelete = await query.DeleteCondicaoPagamento(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }

    }
}