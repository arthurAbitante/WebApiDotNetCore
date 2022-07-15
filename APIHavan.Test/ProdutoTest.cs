using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace APIHavan.Test
{
    public class ProdutoTest
    {
        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
            .Options;

        private Produto[] dadosProdutos()
        {
            var produtos = new[] {
                new Produto{Id=1, Sku="Sku1", Descricao="teste1" },
                new Produto{Id=2, Sku="Sku2", Descricao="teste2" },
                new Produto{Id=3, Sku="Sku3", Descricao="teste3" },
                new Produto{Id=4, Sku="Sku4", Descricao="teste4" },
                new Produto{Id=5, Sku="Sku5", Descricao="teste5" },
                new Produto{Id=6, Sku="Sku6", Descricao="teste6" }
            };

            return produtos;
        }

        private void Popular(AppDbContext context)
        {
            context.Produtos.AddRange(dadosProdutos());
            context.SaveChanges();
        }

        private void RemoverLista(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.RemoveRange(dadosProdutos());
                context.SaveChanges();
            }
        }

        private void RemoverProduto(AppDbContext context, Produto produto)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.Remove(produto);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task testePegaTodosProdutos()
        {
            var context = new AppDbContext(options);

            Popular(context);

            var query = new ProdutosController(context);

            var result = await query.GetProdutos();

            Assert.AreEqual(6, result.Value.Count());

            RemoverLista(context);
        }

        [Test]
        public async Task testePegaProdutosPorId()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(options);

            Popular(context);

            var query = new ProdutosController(context);

            var result = await query.GetProduto(1);

            Assert.AreEqual(produto.Id, result.Value.Id);

            RemoverLista(context);
        }

        [Test]
        public async Task testeInsereProdutos()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(options);

            var query = new ProdutosController(context);

            var result = await query.PostProduto(produto);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Produto;

            Assert.AreEqual(1, item.Id);

            RemoverProduto(context, item);
        }

        [Test]
        public async Task testeEditaClientes()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(options);

            var query = new ProdutosController(context);

            await query.PostProduto(produto);

            produto.Sku = "SkuNovo";

            var resultNovo = await query.PutProduto(produto.Id, produto) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            RemoverProduto(context, produto);
        }

        [Test]
        public async Task testeRemoveProdutos()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(options);

            var query = new ProdutosController(context);

            var result = await query.PostProduto(produto);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Produto;

            Assert.AreEqual(1, item.Id);

            var resultDelete = await query.DeleteProduto(produto.Id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }
    }
}