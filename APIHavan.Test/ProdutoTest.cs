using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIHavan.Test.Constants;

namespace APIHavan.Test
{
    public class ProdutoTest
    {
        [Test]
        public async Task testePegaTodosProdutos()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularProduto(context);

            var query = new ProdutosController(context);

            var result = await query.GetProdutos();

            Assert.AreEqual(6, result.Value.Count());

            Funcoes.RemoverListaProduto(context);
        }

        [Test]
        public async Task testePegaProdutosPorId()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularProduto(context);

            var query = new ProdutosController(context);

            var result = await query.GetProduto(1);

            Assert.AreEqual(produto.Id, result.Value.Id);

            Funcoes.RemoverListaProduto(context);
        }

        [Test]
        public async Task testeInsereProdutos()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ProdutosController(context);

            var result = await query.PostProduto(produto);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Produto;

            Assert.AreEqual(1, item.Id);

            Funcoes.RemoverProduto(context, item);
        }

        [Test]
        public async Task testeEditaProdutos()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ProdutosController(context);

            await query.PostProduto(produto);

            produto.Sku = "SkuNovo";

            var resultNovo = await query.PutProduto(produto.Id, produto) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            Funcoes.RemoverProduto(context, produto);
        }

        [Test]
        public async Task testeRemoveProdutos()
        {
            var produto = new Produto{ Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ProdutosController(context);

            var result = await query.PostProduto(produto);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Produto;

            Assert.AreEqual(1, item.Id);

            var resultDelete = await query.DeleteProduto(produto.Id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }

        [Test]
        public async Task RetornaTipoCorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularProduto(context);

            var query = new ProdutosController(context);

            var result = await query.GetProdutos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<Produto>>>(result);
            Funcoes.RemoverListaProduto(context);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularProduto(context);

            var query = new ProdutosController(context);

            var result = await query.GetProduto(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            Funcoes.RemoverListaProduto(context);
        }

        [Test]
        public async Task RetornaNaoEncontradoQuandoIdEInvalido()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ProdutosController(context);

            var result = await query.PutProduto(99, produto);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(Funcoes.options);

            var query = new ProdutosController(context);

            var resultDelete = await query.DeleteProduto(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }
    }
}