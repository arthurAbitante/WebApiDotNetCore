using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioFiltradoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioFiltradoController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/<RelatorioFiltradoController>/5
        [HttpGet, ActionName("GetRelatorioFiltrado")]
        public ActionResult<IEnumerable<KeyValuePair<string, double>>> pegaRelatorioCnpjRazao(string cnpjCliente, string razaoSocialCliente)
        {
            List <RelatorioPagamento> relatorios = new List<RelatorioPagamento>();
            
            var historicos = _context.HistoricoPrecos.ToList();
            List<string> valoresProduto = new List<string>();
            List<double> valoresPreco = new List<double>();
            List<KeyValuePair<string, double>> juncaoValores = new List<KeyValuePair<string, double>>();

            Cliente clienteFiltrado = new Cliente();

            if (cnpjCliente != "")
            {
                clienteFiltrado = _context.Clientes.Where(x => x.cnpj == cnpjCliente).FirstOrDefault();
            }
            
            if (razaoSocialCliente != "")
            {
                clienteFiltrado = _context.Clientes.Where(x => x.razaoSocial == razaoSocialCliente).FirstOrDefault();
            }

            var relatoriosFiltrados = _context.RelatorioPagamentos.Where(r => r.Cliente.clienteId  == clienteFiltrado.clienteId).ToList();

            foreach (var relatorioFiltrado in relatoriosFiltrados)
            {
                KeyValuePair<string, double> juncao = new KeyValuePair<string, double>(relatorioFiltrado.HistorioPreco.Produto.Descricao, relatorioFiltrado.HistorioPreco.preco);
                juncaoValores.Add(juncao);
            }
          
            return Ok(juncaoValores);
        }
    }
}
