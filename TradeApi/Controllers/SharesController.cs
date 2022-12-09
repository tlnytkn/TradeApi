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
    public class SharesController : ControllerBase
    {
        private readonly TradeApiDbContext _context;

        public SharesController(TradeApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShareDto>>> GetShares()
        {
            return await _context.Shares
                .Select(s => new ShareDto
                {
                    ShareId = s.ShareId,
                    Symbol = s.Symbol,
                    Name = s.Name,
                    Price = s.Price
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShareDto>> GetShare(Guid id)
        {
            var share = await _context.Shares
                .FindAsync(id);

            if (share == null)
            {
                return NotFound();
            }

            ShareDto result = new ShareDto()
            {
                ShareId = share.ShareId,
                Symbol = share.Symbol,
                Name = share.Name,
                Price = share.Price
            };

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShare(Guid id, ShareDto share)
        {
            if (id != share.ShareId)
            {
                return BadRequest("Share Id not exist!");
            }

            var updatedShare = await _context.Shares.FindAsync(id);

            if (updatedShare == null)
            {
                return NotFound(nameof(share));
            }

            updatedShare.Price = share.Price;
            updatedShare.Name = share.Name;
            updatedShare.Symbol = share.Symbol;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShareExists(id))
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
        public async Task<ActionResult<ShareDto>> PostShare(ShareDto share)
        {
            if(_context.Shares.Any(a=>a.Symbol == share.Symbol))
            {
                return BadRequest("Symbol name already exist.");
            }

            var record = new Share()
            {
                ShareId = Guid.NewGuid(),
                Symbol = share.Symbol,
                Name = share.Name,
                Price = share.Price,
            };

            _context.Shares.Add(record);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShare", new { id = share.ShareId }, share);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShare(Guid id)
        {
            var share = await _context.Shares.FindAsync(id);
            if (share == null)
            {
                return NotFound();
            }

            _context.Shares.Remove(share);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShareExists(Guid id)
        {
            return _context.Shares.Any(e => e.ShareId == id);
        }
    }
}
