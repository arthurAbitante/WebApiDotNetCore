using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiEmailHavan.Models;

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

        private void RemoverHistorico(AppDbContext context, HistoricoPreco historico)
        {
            using (context = new AppDbContext(options))
            {
                context.HistoricoPrecos.Remove(historico);
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
        public async Task testeInsereHistoricos()
        {
            var historico = new HistoricoPreco { id = 1, Produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" }, preco = 10.2 };

            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.PostHistoricoPreco(historico);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as HistoricoPreco;

            Assert.AreEqual(1, item.id);

            RemoverHistorico(context, item);
            RemoverProduto(context, produto);
        }

        [Test]
        public async Task testeEditaClientes()
        {
            var historico = new HistoricoPreco { id = 1, Produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" }, preco = 10.2 };

            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            await query.PostHistoricoPreco(historico);

            historico.preco = 15.9 ;

            var resultNovo = await query.PutHistoricoPreco(historico.id, historico) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            RemoverHistorico(context, historico);
            RemoverProduto(context, produto);
        }

        [Test]
        public async Task testeRemoveProdutos()
        {
            var historico = new HistoricoPreco { id = 1, Produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" }, preco = 10.2 };

            var context = new AppDbContext(options);

            var query = new HistoricoPrecosController(context, _emailSender);

            var result = await query.PostHistoricoPreco(historico);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as HistoricoPreco;

            Assert.AreEqual(1, item.id);

            var resultDelete = await query.DeleteHistoricoPreco(historico.id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }
    }
}