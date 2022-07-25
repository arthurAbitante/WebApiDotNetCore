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
        public ActionResult<IEnumerable<KeyValuePair<string, double>>> pegaRelatorioCnpj(string cnpjCliente, string razaoSocialCliente)
        {
            var clientes = _context.Clientes.ToList();
            var relatorios = _context.RelatorioPagamentos.ToList();
            var historicos = _context.HistoricoPrecos.ToList();
            List<string> valoresProduto = new List<string>();
            List<double> valoresPreco = new List<double>();

            Cliente clienteFiltrado = null;

            if (cnpjCliente != "")
            {
                clienteFiltrado = _context.Clientes.Where(x => x.cnpj == cnpjCliente).FirstOrDefault();
            }

            if (razaoSocialCliente != "")
            {
                clienteFiltrado = _context.Clientes.Where(x => x.cnpj == cnpjCliente).FirstOrDefault();
            }


            var relatoriosFiltrados = relatorios.Where(r => r.Cliente.clienteId == clienteFiltrado.clienteId).ToList();

            foreach (var relatorioFiltrado in relatoriosFiltrados)
            {
                valoresProduto.Add(relatorioFiltrado.HistorioPreco.Produto.Descricao);
                valoresPreco.Add(relatorioFiltrado.HistorioPreco.preco);
            }
            var query = from produto in valoresProduto
                        join preco in valoresPreco
                        on produto equals preco.ToString()
                        select new KeyValuePair<string, double>(produto, preco);
            return Ok(query);
        }
    }
}
