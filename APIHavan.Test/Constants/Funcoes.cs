using APIHavan.Data;
using Microsoft.EntityFrameworkCore;

namespace APIHavan.Test.Constants
{
    public class Funcoes
    {

        public static Produto produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

        public static DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
            .Options;

        //clientes
        public static Cliente[] dadosCliente()
        {
            var cliente = new[] {
                new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email="email1@email.com" },
                new Cliente { clienteId = 2, cnpj = "cnpjtest2", razaoSocial = "razaosocialtest2", email="email2@email.com" },
            };

            return cliente;
        }

        public static void PopularClientes(AppDbContext context)
        {
            context.Clientes.AddRange(dadosCliente());
            context.SaveChanges();
        }

        public static void RemoverListaClientes(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.RemoveRange(dadosCliente());
                context.SaveChanges();
            }
        }

        public static void RemoverCliente(AppDbContext context, Cliente cliente)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.Remove(cliente);
                context.SaveChanges();
            }
        }

        //condicao pagamento
        public static CondicaoPagamento[] dadosCondicoesPagamentos()
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

        public static void PopularCondicao(AppDbContext context)
        {
            context.CondicoesPagamentos.AddRange(dadosCondicoesPagamentos());
            context.SaveChanges();
        }

        public static void RemoverListaCondicao(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.CondicoesPagamentos.RemoveRange(dadosCondicoesPagamentos());
                context.SaveChanges();
            }

        }

        public static void RemoverCondicao(AppDbContext context, CondicaoPagamento condicaoPagamento)
        {
            using (context = new AppDbContext(options))
            {
                context.CondicoesPagamentos.Remove(condicaoPagamento);
                context.SaveChanges();
            }
        }

        //historico preco
        public static HistoricoPreco[] dadosHistorico()
        {
            var historicos = new[]
            {
                new HistoricoPreco{ id=1, Produto = produto, preco = 10.2},
                new HistoricoPreco{ id=2, Produto = produto, preco = 10.28},
                new HistoricoPreco{ id=3, Produto = produto, preco = 10.50}
            };
            return historicos;
        }

        public static void PopularHistoricos(AppDbContext context)
        {
            context.HistoricoPrecos.AddRange(dadosHistorico());
            context.SaveChanges();
        }

        public static void RemoverListaHistorico(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.HistoricoPrecos.RemoveRange(dadosHistorico());
                context.SaveChanges();
            }
        }

        public static void RemoverHistorico(AppDbContext context, HistoricoPreco historicoPreco)
        {
            using (context = new AppDbContext(options))
            {
                context.HistoricoPrecos.Remove(historicoPreco);
                context.SaveChanges();
            }
        }

        //produto
        public static Produto[] dadosProdutos()
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

        public static void PopularProduto(AppDbContext context)
        {
            context.Produtos.AddRange(dadosProdutos());
            context.SaveChanges();
        }

        public static void RemoverListaProduto(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.RemoveRange(dadosProdutos());
                context.SaveChanges();
            }
        }

        public static void RemoverProduto(AppDbContext context, Produto produto)
        {
            using (context = new AppDbContext(options))
            {
                context.Produtos.Remove(produto);
                context.SaveChanges();
            }
        }

        //dados relatorio
        public static RelatorioPagamento[] dadosRelatorio()
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

        public static void PopularRelatorios(AppDbContext context)
        {
            context.RelatorioPagamentos.AddRange(dadosRelatorio());
            context.SaveChanges();
        }

        public static void RemoverListaRelatorios(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.RelatorioPagamentos.RemoveRange(dadosRelatorio());
                context.SaveChanges();
            }
        }

        public static void RemoverRelatorio(AppDbContext context, RelatorioPagamento rel)
        {
            using (context = new AppDbContext(options))
            {
                context.RelatorioPagamentos.Remove(rel);
                context.SaveChanges();
            }
        }
    }
}
