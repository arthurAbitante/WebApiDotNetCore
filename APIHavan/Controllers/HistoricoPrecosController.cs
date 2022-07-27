using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIHavan.Data;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using ApiEmailHavan.Models;

namespace APIHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoPrecosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ConnectionFactory _factory;
        private readonly IEmailSender _emailSender;

        public HistoricoPrecosController(AppDbContext context, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _context = context;
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        // GET: api/HistoricoPrecos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoPreco>>> GetHistoricoPrecos()
        {
            return await _context.HistoricoPrecos.ToListAsync();
        }

        // GET: api/HistoricoPrecos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoPreco>> GetHistoricoPreco(int id)
        {
            var historicoPreco = await _context.HistoricoPrecos.FindAsync(id);

            if (historicoPreco == null)
            {
                return NotFound();
            }

            return historicoPreco;
        }

        // PUT: api/HistoricoPrecos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricoPreco(int id, HistoricoPreco historicoPreco)
        {
            if (id != historicoPreco.id)
            {
                return BadRequest();
            }
            _context.Entry(historicoPreco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                //o put é somente utilizado para correção de um preço, então é enviado
                //automaticamente para o cliente

                //criar constante de mensagem
                var message = new Message(new string[] { Constants.Constants.emailCliente }, Constants.Constants.assunto, Constants.Constants.mensagem, null);
                await _emailSender.SendEmailAsync(message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricoPrecoExists(id))
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

        // POST: api/HistoricoPrecos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistoricoPreco>> PostHistoricoPreco(HistoricoPreco historicoPreco)
        {
            _context.HistoricoPrecos.Add(historicoPreco);
            await _context.SaveChangesAsync();

            //O historico preço não é editado. Mas adicionado. Existe a requisição PUT para caso
            //houver algum erro, mas será mandado também uma mensagem de correção para o preço 
            //aqui verifica se é maior do que 0 o que já existe no banco de dados para enviar
            //a atualização de preço                 
            var message = new Message(new string[] { Constants.Constants.emailCliente }, Constants.Constants.assunto, Constants.Constants.mensagem + historicoPreco.preco, null);
            await _emailSender.SendEmailAsync(message);   

            return CreatedAtAction("GetHistoricoPreco", new { id = historicoPreco.id }, historicoPreco);
        }

        // DELETE: api/HistoricoPrecos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricoPreco(int id)
        {
            var historicoPreco = await _context.HistoricoPrecos.FindAsync(id);
            if (historicoPreco == null)
            {
                return NotFound();
            }

            _context.HistoricoPrecos.Remove(historicoPreco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoricoPrecoExists(int id)
        {
            return _context.HistoricoPrecos.Any(e => e.id == id);
        }
    }
}
