using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiEmailHavan.Models;
using System.Collections.Generic;

namespace APIHavan.Test
{
    public class HistoricoPrecoTest
    {
        Produto produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                   .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
                   .Options;

        private readonly IEmailSender _emailSender;
        
        private HistoricoPreco[] dadosHistorico()
        {
            var historicos = new[]
            {
                new HistoricoPreco{ id=1, Produto = produto, preco = 10.2},
                new HistoricoPreco{ id=2, Produto = produto, preco = 10.28},
                new HistoricoPreco{ id=3, Produto = produto, preco = 10.50}
            };
            return historicos;
        }

        private void PopularHistoricos(AppDbContext context)
        {
            context.HistoricoPrecos.AddRange(dadosHistorico());
            context.SaveChanges();
        }

        private void RemoverProduto(AppDbContext context, Produto produto)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.Remove(produto);
                context.SaveChanges();
            }
        }

        private void RemoverListaHistorico(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.HistoricoPrecos.RemoveRange(dadosHistorico());
                context.SaveChanges();
            }
        }

        [Test]
        public async Task testePegaTodosHistoricos()
        {
            var context = new AppDbContext(options);

            PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPrecos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<HistoricoPreco>>>(result);
            Assert.AreEqual(3, result.Value.Count());

            RemoverListaHistorico(context);
            RemoverProduto(context, produto);
        }

        [Test]
        public async Task testePegaHistoricoPorId()
        {
            var historico = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var context = new AppDbContext(options);

            PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPreco(1);

            Assert.AreEqual(historico.id, result.Value.id);

            RemoverListaHistorico(context);
            RemoverProduto(context, produto);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var context = new AppDbContext(options);

            PopularHistoricos(context);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.GetHistoricoPreco(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            RemoverListaHistorico(context);
            RemoverProduto(context, produto);
        }

        [Test]
        public async Task testeRemoveProdutos()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };
            var historico = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.PostHistoricoPreco(historico);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as HistoricoPreco;

            Assert.AreEqual(1, item.id);

            var resultDelete = await query.DeleteHistoricoPreco(historico.id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);

            RemoverProduto(context, produto);
        }

        [Test]
        public async Task RetornaNaoEncontradoQuandoIdEInvalido()
        {
            var historico = new HistoricoPreco { id = 1, Produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" }, preco = 10.2 };

            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.PutHistoricoPreco(99, historico);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var resultDelete = await query.DeleteHistoricoPreco(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }
    }
}