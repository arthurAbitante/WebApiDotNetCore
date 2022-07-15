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
    public class CondicaoPagamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CondicaoPagamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CondicaoPagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CondicaoPagamento>>> GetCondicoesPagamentos()
        {
            return await _context.CondicoesPagamentos.ToListAsync();
        }

        // GET: api/CondicaoPagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CondicaoPagamento>> GetCondicaoPagamento(int id)
        {
            var condicaoPagamento = await _context.CondicoesPagamentos.FindAsync(id);

            if (condicaoPagamento == null)
            {
                return NotFound();
            }

            return condicaoPagamento;
        }

        // PUT: api/CondicaoPagamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCondicaoPagamento(int id, CondicaoPagamento condicaoPagamento)
        {
            if (id != condicaoPagamento.condicaoPagamentoId)
            {
                return BadRequest();
            }

            _context.Entry(condicaoPagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CondicaoPagamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CondicaoPagamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CondicaoPagamento>> PostCondicaoPagamento(CondicaoPagamento condicaoPagamento)
        {
            _context.CondicoesPagamentos.Add(condicaoPagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCondicaoPagamento", new { id = condicaoPagamento.condicaoPagamentoId }, condicaoPagamento);
        }

        // DELETE: api/CondicaoPagamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCondicaoPagamento(int id)
        {
            var condicaoPagamento = await _context.CondicoesPagamentos.FindAsync(id);
            if (condicaoPagamento == null)
            {
                return NotFound();
            }

            _context.CondicoesPagamentos.Remove(condicaoPagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CondicaoPagamentoExists(int id)
        {
            return _context.CondicoesPagamentos.Any(e => e.condicaoPagamentoId == id);
        }
    }
}
