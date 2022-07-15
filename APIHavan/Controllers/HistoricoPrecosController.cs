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

namespace APIHavan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoPrecosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "messages";

        public HistoricoPrecosController(AppDbContext context)
        {
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
                //enviar a requisição aqui

                if (historicoPreco.preco != _context.Entry(historicoPreco).Entity.preco)
                {
                    string message = "Valor do preço foi alterado!!!";

                    using (var connection = _factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(
                                queue: QUEUE_NAME,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                            var stringfiedMessage = JsonConvert.SerializeObject(message);
                            var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                            channel.BasicPublish(
                                exchange: "",
                                routingKey: QUEUE_NAME,
                                basicProperties: null,
                                body: bytesMessage);
                        }
                    }
                }

                





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
