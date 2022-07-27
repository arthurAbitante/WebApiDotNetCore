using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIHavan.Test
{
    public class RelatorioPagamentoTest
    {
        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
                    .Options;

        Produto produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

        private RelatorioPagamento[] dadosRelatorio()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };
           
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };


            var relatorios = new[]
            {
                new RelatorioPagamento{ id=1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento, Cliente = cliente},
                new RelatorioPagamento{ id=2, HistorioPreco = historicoPreco, CondicaoPagamento= condicaoPagamento2, Cliente = cliente},
            };

            return relatorios;
        }

        private void PopularRelatorios(AppDbContext context)
        {
            context.RelatorioPagamentos.AddRange(dadosRelatorio());
            context.SaveChanges();
        }

        private void RemoverListaRelatorios(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.RelatorioPagamentos.RemoveRange(dadosRelatorio());
                context.SaveChanges();
            }
        }

        private void RemoverRelatorio(AppDbContext context, RelatorioPagamento rel)
        {
            using (context = new AppDbContext(options))
            {
                context.RelatorioPagamentos.Remove(rel);
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

        private void RemoverProduto(AppDbContext context, Produto produto)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.Remove(produto);
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

        private void RemoverCliente(AppDbContext context, Cliente cliente)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.Remove(cliente);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task testePegaTodosRelatorios()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamentos();

            Assert.AreEqual(2, result.Value.Count());

            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverProduto(context, produto);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverCliente(context, cliente);
        }


        [Test]
        public async Task testePegaHistoricoPorId()
        {

            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };

            var context = new AppDbContext(options);

            PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamento(1);

            Assert.AreEqual(relatorioPagamento.id, result.Value.id);

            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverProduto(context, produto);
            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeInsereRelatorios()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento, Cliente = cliente };

            var context = new AppDbContext(options);

            var query = new RelatorioPagamentosController(context);

            var result = await query.PostRelatorioPagamento(relatorioPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as RelatorioPagamento;

            Assert.AreEqual(1, item.id);

            RemoverRelatorio(context, item);
            RemoverHistorico(context, historicoPreco);
            RemoverProduto(context, produto);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeRemoveRelatorioPagamento()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };

            var context = new AppDbContext(options);

            var query = new RelatorioPagamentosController(context);

            var result = await query.PostRelatorioPagamento(relatorioPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as RelatorioPagamento;

            Assert.AreEqual(1, item.id);

            var resultDelete = await query.DeleteRelatorioPagamento(relatorioPagamento.id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
            RemoverHistorico(context, historicoPreco);
            RemoverProduto(context, produto);
            RemoverCondicao(context, condicaoPagamento);
        }

        [Test]
        public async Task RetornaTipoCorreto()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamentos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<RelatorioPagamento>>>(result);
            
            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverProduto(context, produto);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamento(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverProduto(context, produto);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(options);

            var query = new RelatorioPagamentosController(context);

            var resultDelete = await query.DeleteRelatorioPagamento(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }

        [Test]
        public async Task testeInsereHistoricosParaCnpjFiltrado()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);
            PopularRelatorios(context);

            var query = new RelatorioFiltradoController(context);

            var result = query.pegaRelatorioCnpjRazao("cnpjteste", "");
            //result.Result.Value
            var response = result.Result as OkObjectResult;
            var item = response.Value as List<KeyValuePair<string, double>>;
            List<string> chaves = new List<string>();
            List<double> valores = new List<double>();

            foreach (KeyValuePair<string, double> kvp in item)
            {
                chaves.Add(kvp.Key);
                valores.Add(kvp.Value);
            }

            Assert.AreEqual(historicoPreco.Produto.Descricao, chaves[0]);
            Assert.AreEqual(historicoPreco.preco, valores[0]);

            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverProduto(context, produto);
            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeInsereHistoricosParaRazaoSocialFiltrado()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);
            PopularRelatorios(context);

            var query = new RelatorioFiltradoController(context);

            var result = query.pegaRelatorioCnpjRazao("", "razaosocialteste");
            
            var response = result.Result as OkObjectResult;
            var item = response.Value as List<KeyValuePair<string, double>>;
            List<string> chaves = new List<string>();
            List<double> valores = new List<double>();

            foreach (KeyValuePair<string, double> kvp in item)
            {
                chaves.Add(kvp.Key);
                valores.Add(kvp.Value);
            }

            Assert.AreEqual(historicoPreco.Produto.Descricao, chaves[0]);
            Assert.AreEqual(historicoPreco.preco, valores[0]);

            RemoverListaRelatorios(context);
            RemoverHistorico(context, historicoPreco);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverProduto(context, produto);
            RemoverCliente(context, cliente);
        }
    }
}