using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Test
{
    public class ClienteTest
    {
        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
                    .Options;

        Produto produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };


        private Cliente[] dadosCliente()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };

            List<RelatorioPagamento> relatorio1 = new List<RelatorioPagamento>();
            relatorio1.Add(relatorioPagamento1);

            List<RelatorioPagamento> relatorio2 = new List<RelatorioPagamento>();
            relatorio2.Add(relatorioPagamento2);

            var cliente = new[] { 
                new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", RelatorioPagamento = relatorio1 }, 
                new Cliente { clienteId = 2, cnpj = "cnpjtest2", razaoSocial = "razaosocialtest2", RelatorioPagamento = relatorio2 },
            };

            return cliente;
        }

        private void PopularClientes(AppDbContext context)
        {
            context.Clientes.AddRange(dadosCliente());
            context.SaveChanges();
        }

        private void RemoverListaClientes(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.RemoveRange(dadosCliente());
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

        private void RemoverHistorico(AppDbContext context, HistoricoPreco historico)
        {
            using (context = new AppDbContext(options))
            {
                context.HistoricoPrecos.Remove(historico);
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

        private void RemoverRelatorio(AppDbContext context, RelatorioPagamento rel)
        {
            using (context = new AppDbContext(options))
            {
                context.RelatorioPagamentos.Remove(rel);
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
        public async Task testePegaTodosClientes()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };

            List<RelatorioPagamento> relatorio1 = new List<RelatorioPagamento>();
            relatorio1.Add(relatorioPagamento1);

            List<RelatorioPagamento> relatorio2 = new List<RelatorioPagamento>();
            relatorio2.Add(relatorioPagamento2);

            var context = new AppDbContext(options);

            PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetClientes();

            Assert.AreEqual(2, result.Value.Count());

            RemoverRelatorio(context, relatorioPagamento1);
            RemoverRelatorio(context, relatorioPagamento2);
            RemoverHistorico(context, historicoPreco);
            RemoverCondicao(context, condicaoPagamento);
            RemoverCondicao(context, condicaoPagamento2);
            RemoverProduto(context, produto);

            RemoverListaClientes(context);

        }


        [Test]
        public async Task testePegaClientePorId()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };

            List<RelatorioPagamento> relatorios = new List<RelatorioPagamento>();
            relatorios.Add(relatorioPagamento1);
            relatorios.Add(relatorioPagamento2);

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", RelatorioPagamento = relatorios };
            
            var context = new AppDbContext(options);

            PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetCliente(1);

            Assert.AreEqual(cliente.clienteId, result.Value.clienteId);

            RemoverListaClientes(context);
        }

        [Test]
        public async Task testeInsereClientes()
        {
            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };
            
            List<RelatorioPagamento> relatorios = new List<RelatorioPagamento>();
            relatorios.Add(relatorioPagamento1);
            relatorios.Add(relatorioPagamento2);

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", RelatorioPagamento = relatorios };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            RemoverCliente(context, item);
        }

        [Test]
        public async Task testeEditaClientes()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };
            List<RelatorioPagamento> relatorios = new List<RelatorioPagamento>();
            relatorios.Add(relatorioPagamento1);
            relatorios.Add(relatorioPagamento2);

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", RelatorioPagamento = relatorios };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            await query.PostCliente(cliente);

            cliente.cnpj = "novoCnpj";
            cliente.razaoSocial = "novaRazaoSocial";


            var resultNovo = await query.PutCliente(cliente.clienteId, cliente) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeRemoveClientes()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };

            var relatorioPagamento1 = new RelatorioPagamento { id = 1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento };
            var relatorioPagamento2 = new RelatorioPagamento { id = 2, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento2 };

            List<RelatorioPagamento> relatorios = new List<RelatorioPagamento>();
            relatorios.Add(relatorioPagamento1);
            relatorios.Add(relatorioPagamento2);

            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", RelatorioPagamento = relatorios };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            var resultDelete = await query.DeleteCliente(cliente.clienteId) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }
    }
}