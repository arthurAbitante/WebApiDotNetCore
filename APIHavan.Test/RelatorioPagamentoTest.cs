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
    public class RelatorioPagamentoTest
    {
        [Test]
        public async Task testePegaTodosRelatorios()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamentos();

            Assert.AreEqual(2, result.Value.Count());

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverCliente(context, cliente);
        }


        [Test]
        public async Task testePegaRelatorioPorId()
        {

            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamento(1);

            Assert.AreEqual(relatorioPagamento.id, result.Value.id);

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeInsereRelatorios()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento, Cliente = cliente };

            var context = new AppDbContext(Funcoes.options);

            var query = new RelatorioPagamentosController(context);

            var result = await query.PostRelatorioPagamento(relatorioPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as RelatorioPagamento;

            Assert.AreEqual(1, item.id);

            Funcoes.RemoverRelatorio(context, item);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeRemoveRelatorioPagamento()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };

            var relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };

            var context = new AppDbContext(Funcoes.options);

            var query = new RelatorioPagamentosController(context);

            var result = await query.PostRelatorioPagamento(relatorioPagamento);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as RelatorioPagamento;

            Assert.AreEqual(1, item.id);

            var resultDelete = await query.DeleteRelatorioPagamento(relatorioPagamento.id) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
        }

        [Test]
        public async Task RetornaTipoCorreto()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamentos();

            Assert.IsInstanceOf<ActionResult<IEnumerable<RelatorioPagamento>>>(result);

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularRelatorios(context);

            var query = new RelatorioPagamentosController(context);

            var result = await query.GetRelatorioPagamento(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(Funcoes.options);

            var query = new RelatorioPagamentosController(context);

            var resultDelete = await query.DeleteRelatorioPagamento(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }

        [Test]
        public async Task testeInsereRelatorioParaCnpjFiltrado()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);
            Funcoes.PopularRelatorios(context);

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

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeInsereRelatorioParaRazaoSocialFiltrado()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = Funcoes.produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);
            Funcoes.PopularRelatorios(context);

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

            Funcoes.RemoverListaRelatorios(context);
            Funcoes.RemoverHistorico(context, historicoPreco);
            Funcoes.RemoverCondicao(context, condicaoPagamento);
            Funcoes.RemoverCondicao(context, condicaoPagamento2);
            Funcoes.RemoverProduto(context, Funcoes.produto);
            Funcoes.RemoverCliente(context, cliente);
        }
    }
}