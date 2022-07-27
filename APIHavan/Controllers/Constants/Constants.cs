using APIHavan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Constants
{
    public class Constants
    {

        public static Produto produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };
        public static Cliente cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };
        public static HistoricoPreco historico = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };
        public static CondicaoPagamento condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
        public static RelatorioPagamento relatorioPagamento = new RelatorioPagamento { id = 1, HistorioPreco = historico, CondicaoPagamento = condicaoPagamento };

        public Cliente[] dadosCliente()
        {

            var cliente = new[] {
                new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email="email1@email.com" },
                new Cliente { clienteId = 2, cnpj = "cnpjtest2", razaoSocial = "razaosocialtest2", email="email2@email.com" },
            };

            return cliente;
        }

        public CondicaoPagamento[] dadosCondicoesPagamentos()
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

        public HistoricoPreco[] dadosHistorico()
        {
            var historicos = new[]
            {
                new HistoricoPreco{ id=1, Produto = produto, preco = 10.2},
                new HistoricoPreco{ id=2, Produto = produto, preco = 10.28},
                new HistoricoPreco{ id=3, Produto = produto, preco = 10.50}
            };
            return historicos;
        }

        public Produto[] dadosProdutos()
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

        public RelatorioPagamento[] dadosRelatorio()
        {
            var produto = new Produto { Id = 1, Sku = "Sku1", Descricao = "teste1" };

            var historicoPreco = new HistoricoPreco { id = 1, Produto = produto, preco = 10.2 };

            var condicaoPagamento = new CondicaoPagamento { condicaoPagamentoId = 1, descricao = "Pago", dias = "01/01/2020" };
            var condicaoPagamento2 = new CondicaoPagamento { condicaoPagamentoId = 2, descricao = "Atrasado", dias = "01/02/2020" };


            var relatorios = new[]
            {
                new RelatorioPagamento{ id=1, HistorioPreco = historicoPreco, CondicaoPagamento = condicaoPagamento},
                new RelatorioPagamento{ id=2, HistorioPreco = historicoPreco, CondicaoPagamento= condicaoPagamento2},
            };

            return relatorios;
        }

        public static string emailCliente = "jiwomi7721@5k2u.com";
    }
}
