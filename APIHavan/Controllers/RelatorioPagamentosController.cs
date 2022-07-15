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

        // GET: api/RelatorioPagamentoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelatorioPagamento>>> GetRelatorioPagamentos()
        {
            return await _context.RelatorioPagamentos.ToListAsync();
        }

        // GET: api/RelatorioPagamentoes/5
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

        // POST: api/RelatorioPagamentoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RelatorioPagamento>> PostRelatorioPagamento(RelatorioPagamento relatorioPagamento)
        {
            _context.RelatorioPagamentos.Add(relatorioPagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelatorioPagamento", new { id = relatorioPagamento.id }, relatorioPagamento);
        }

        // DELETE: api/RelatorioPagamentoes/5
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

        private bool RelatorioPagamentoExists(int id)
        {
            return _context.RelatorioPagamentos.Any(e => e.id == id);
        }
    }
}
