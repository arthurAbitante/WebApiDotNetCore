using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace APIHavan.Test
{
    public class Tests
    {
        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
            .Options;

        private CondicaoPagamento[] dadosCondicoesPagamentos()
        {
            var condicoesPagamentos = new[] {
                new CondicaoPagamento{ condicaoPagamentoId = 1, descricao="Pago", dias="01/01/2020"},
                new CondicaoPagamento{ condicaoPagamentoId = 2, descricao="Pago", dias="01/02/2020"},
                new CondicaoPagamento{ condicaoPagamentoId = 3, descricao="Pago", dias="01/03/2020"},
                new CondicaoPagamento{ condicaoPagamentoId = 4, descricao="Pago", dias="01/04/2020"},
                new CondicaoPagamento{ condicaoPagamentoId = 5, descricao="Atrasado", dias="01/05/2020"},
                new CondicaoPagamento{ condicaoPagamentoId = 6, descricao="A pagar", dias="01/06/2020"},
            };
            return condicoesPagamentos;
        }

        private void Popular(AppDbContext context)
        {
            context.CondicoesPagamentos.AddRange(dadosCondicoesPagamentos());
            context.SaveChanges();
        }

        private void RemoverLista(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.CondicoesPagamentos.RemoveRange(dadosCondicoesPagamentos());
                context.SaveChanges();
            }

        }

        private void RemoverCondicao(AppDbContext context, CondicaoPagamento condicaoPagamento)
        {
            using (context = new AppDbContext(options))
            {
                context.CondicoesPagamentos.Remove(condicaoPagamento);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task testePegaTodasCondicoesPagamentos()
        {
            var context = new AppDbContext(options);

            Popular(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicoesPagamentos();
            //o assert deverá contar a lista
            Assert.AreEqual(6, result.Value.Count());

            RemoverLista(context);
        }

        [Test]
        public async Task testePegaCondicoesPagamentosPorId()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(options);

            Popular(context);

            var query = new CondicaoPagamentosController(context);

            var result = await query.GetCondicaoPagamento(1);

            Assert.AreEqual(condicaoPagamento.condicaoPagamentoId, result.Value.condicaoPagamentoId);

            RemoverLista(context);
        }

        [Test]
        public async Task testeInsereCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(options);

            var query = new CondicaoPagamentosController(context);

            var result = await query.PostCondicaoPagamento(condicaoPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as CondicaoPagamento;

            Assert.AreEqual(1, item.condicaoPagamentoId);

            RemoverCondicao(context, item);
           // Assert.AreEqual(condicaoPagamento.condicaoPagamentoId, result.Value.condicaoPagamentoId);
        }

        [Test]
        public async Task testeEditaCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(options);

            var query = new CondicaoPagamentosController(context);

            await query.PostCondicaoPagamento(condicaoPagamento);

            condicaoPagamento.descricao = "Atrasado";

            var resultNovo = await query.PutCondicaoPagamento(condicaoPagamento.condicaoPagamentoId, condicaoPagamento) as StatusCodeResult;
               
            Assert.AreEqual(204, resultNovo.StatusCode);

            RemoverCondicao(context, condicaoPagamento);
        }

        [Test]
        public async Task testeRemoveCondicoesPagamentos()
        {
            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var context = new AppDbContext(options);

            var query = new CondicaoPagamentosController(context);

            var result = await query.PostCondicaoPagamento(condicaoPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as CondicaoPagamento;

            Assert.AreEqual(1, item.condicaoPagamentoId);

            var resultDelete = await query.DeleteCondicaoPagamento(condicaoPagamento.condicaoPagamentoId) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }


    }
}