using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIHavan.Data;

namespace APIHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioPagamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioPagamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RelatorioPagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelatorioPagamento>>> GetRelatorioPagamentos()
        {
            return await _context.RelatorioPagamentos.ToListAsync();
        }

        // GET: api/RelatorioPagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RelatorioPagamento>> GetRelatorioPagamento(int id)
        {
            var relatorioPagamento = await _context.RelatorioPagamentos.FindAsync(id);

            if (relatorioPagamento == null)
            {
                return NotFound();
            }

            return relatorioPagamento;
        }

        // POST: api/RelatorioPagamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RelatorioPagamento>> PostRelatorioPagamento(RelatorioPagamento relatorioPagamento)
        {
            _context.RelatorioPagamentos.Add(relatorioPagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelatorioPagamento", new { id = relatorioPagamento.id }, relatorioPagamento);
        }

        // DELETE: api/RelatorioPagamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelatorioPagamento(int id)
        {
            var relatorioPagamento = await _context.RelatorioPagamentos.FindAsync(id);
            if (relatorioPagamento == null)
            {
                return NotFound();
            }

            _context.RelatorioPagamentos.Remove(relatorioPagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}"), ActionName("GetRelatorioCnpj")]
        protected ActionResult<IEnumerable<KeyValuePair<string, double>>> pegaRelatorioCnpj(string cnpjCliente)
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

        [HttpGet("{id}"), ActionName("GetRazaoSocial")]
        protected ActionResult<IEnumerable<KeyValuePair<string, double>>> pegaRelatorioRazao(string razaoSocialCliente)
        {
            var clientes = _context.Clientes.ToList();
            var relatorios = _context.RelatorioPagamentos.ToList();
            var historicos = _context.HistoricoPrecos.ToList();
            List<string> valoresProduto = new List<string>();
            List<double> valoresPreco = new List<double>();

            Cliente clienteFiltrado = null;

            if (razaoSocialCliente != "")
            {
                clienteFiltrado = _context.Clientes.Where(x => x.razaoSocial == razaoSocialCliente).FirstOrDefault();
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

        private bool RelatorioPagamentoExists(int id)
        {
            return _context.RelatorioPagamentos.Any(e => e.id == id);
        }
    }
}
