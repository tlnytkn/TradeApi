using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeApi.Domain;
using TradeApi.Dtos;
using TradeApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradersController : ControllerBase
    {
        private readonly TradeApiDbContext _context;

        public TradersController(TradeApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraderDto>>> GetTraders()
        {
            return await _context.Traders
                .Select(s => new TraderDto
                {
                    TraderId = s.TraderId,
                    Email = s.Email,
                    Name = s.Name,
                    Phone = s.Phone
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TraderDto>> GetTrader(Guid id)
        {
            var trader = await _context.Traders
                .Where(w => w.TraderId == id)
                .Select(s => new TraderDto
                {
                    TraderId = s.TraderId,
                    Email = s.Email,
                    Name = s.Name,
                    Phone = s.Phone
                })
                .FirstOrDefaultAsync();

            if (trader == null)
            {
                return NotFound();
            }

            return trader;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrader(Guid id, TraderDto trader)
        {
            if (id != trader.TraderId)
            {
                return BadRequest("Trader Id not exist!");
            }

            var updatedTrader = await _context.Traders.FindAsync(id);

            if (updatedTrader == null)
            {
                return NotFound(nameof(trader));
            }

            updatedTrader.Email = trader.Email;
            updatedTrader.Phone = trader.Phone;
            updatedTrader.Name = trader.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraderExists(id))
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

        [HttpPost]
        public async Task<ActionResult<TraderDto>> PostTrader(TraderDto trader)
        {
            var record = new Trader
            {
                TraderId = trader.TraderId,
                Email = trader.Email,
                Name = trader.Name,
                Phone = trader.Phone
            };

            _context.Traders.Add(record);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrader", new { id = trader.TraderId }, trader);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrader(Guid id)
        {
            var trader = await _context.Traders.FindAsync(id);
            if (trader == null)
            {
                return NotFound();
            }
            // Foreign key ile bagli olan child tablolardaki kayitlar cascade ile otomatik silinecektir.
            _context.Traders.Remove(trader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TraderExists(Guid id)
        {
            return _context.Traders.Any(e => e.TraderId == id);
        }
    }
}
