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
    public class TraderPortfoliosController : ControllerBase
    {
        private readonly TradeApiDbContext _context;

        public TraderPortfoliosController(TradeApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllTraderPortfolio")]
        public async Task<ActionResult<IEnumerable<TraderPortfolioDto>>> GetTraderPortfolioes()
        {
            return await _context.TraderPortfolioes
                .Select(s => new TraderPortfolioDto
                {
                    TraderPortfolioId = s.TraderPortfolioId,
                    TraderId = s.TraderId,
                    ShareId = s.ShareId,
                    Amount = s.Amount,
                    Share = new ShareDto
                    {
                        Name = s.Share.Name,
                        Price = s.Share.Price,
                        ShareId = s.Share.ShareId,
                        Symbol = s.Share.Symbol
                    },
                    Trader = new TraderDto
                    {
                        TraderId = s.Trader.TraderId,
                        Email = s.Trader.Email,
                        Name = s.Trader.Name,
                        Phone = s.Trader.Phone
                    },
                })
                .ToListAsync();
        }

        [HttpGet("GetTraderPortfolioesByTraderId/{id}")]
        public async Task<ActionResult<IEnumerable<TraderPortfolioDto>>> GetTraderPortfolioesByTraderId(Guid id)
        {
            return await _context.TraderPortfolioes
                .Include(i => i.Trader)
                .Include(i => i.Share)
                .Where(w => w.TraderId == id)
                .OrderBy(o => o.TraderId)
                .Select(s => new TraderPortfolioDto
                {
                    ShareId = s.ShareId,
                    TraderPortfolioId = s.TraderPortfolioId,

                    Share = new ShareDto
                    {
                        Name = s.Share.Name,
                        Price = s.Share.Price,
                        ShareId = s.Share.ShareId,
                        Symbol = s.Share.Symbol
                    },
                    TraderId = s.TraderId,
                    Trader = new TraderDto
                    {
                        TraderId = s.Trader.TraderId,
                        Email = s.Trader.Email,
                        Name = s.Trader.Name,
                        Phone = s.Trader.Phone
                    },
                    Amount = s.Amount,
                })
                .ToListAsync();
        }

        [HttpGet("GetTraderPortfolioesByShareId/{id}")]
        public async Task<ActionResult<IEnumerable<TraderPortfolioDto>>> GetTraderPortfolioesByShareId(Guid id)
        {
            return await _context.TraderPortfolioes
                .Where(w => w.ShareId == id)
                .OrderBy(o => o.ShareId)
                .Select(s => new TraderPortfolioDto
                {
                    ShareId = s.ShareId,
                    TraderPortfolioId = s.TraderPortfolioId,
                    Share = new ShareDto
                    {
                        Name = s.Share.Name,
                        Price = s.Share.Price,
                        ShareId = s.Share.ShareId,
                        Symbol = s.Share.Symbol
                    },
                    TraderId = s.TraderId,
                    Trader = new TraderDto
                    {
                        TraderId = s.Trader.TraderId,
                        Email = s.Trader.Email,
                        Name = s.Trader.Name,
                        Phone = s.Trader.Phone
                    },
                    Amount = s.Amount,
                })
                .ToListAsync();
        }

        // GetTraderPortfolioesByTraderId ve GetTraderPortfolioesByShareId uclarinin yerine bu sekildede bir uc tasarlanabilir
        [HttpPost("GetTraderPortfolioes")]
        public async Task<ActionResult<IEnumerable<TraderPortfolioDto>>> GetTraderPortfolioes(GetTraderPortfolioes request)
        {
            var query = _context.TraderPortfolioes.AsQueryable();

            if (request.TraderId == Guid.Empty && request.ShareId == Guid.Empty)
            {
                return BadRequest();
            }

            if (request.TraderId != Guid.Empty)
            {
                query = query.Where(w => w.TraderId == request.TraderId);
            }

            if (request.ShareId != Guid.Empty)
            {
                query = query.Where(w => w.TraderId == request.ShareId);
            }

            var result = await query.Select(s => new TraderPortfolioDto
            {
                ShareId = s.ShareId,
                TraderPortfolioId = s.TraderPortfolioId,
                Share = new ShareDto
                {
                    Name = s.Share.Name,
                    Price = s.Share.Price,
                    ShareId = s.Share.ShareId,
                    Symbol = s.Share.Symbol
                },
                TraderId = s.TraderId,
                Trader = new TraderDto
                {
                    TraderId = s.Trader.TraderId,
                    Email = s.Trader.Email,
                    Name = s.Trader.Name,
                    Phone = s.Trader.Phone
                },
                Amount = s.Amount,
            }).ToListAsync();

            return result.Count == 0 ? NotFound() : result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TraderPortfolioDto>> GetTraderPortfolio(Guid id)
        {
            var traderPortfolio = await _context.TraderPortfolioes
                .Where(w => w.TraderPortfolioId == id)
                .Select(s => new TraderPortfolioDto
                {
                    ShareId = s.ShareId,
                    TraderPortfolioId = s.TraderPortfolioId,
                    Share = new ShareDto
                    {
                        Name = s.Share.Name,
                        Price = s.Share.Price,
                        ShareId = s.Share.ShareId,
                        Symbol = s.Share.Symbol
                    },
                    TraderId = s.TraderId,
                    Trader = new TraderDto
                    {
                        TraderId = s.Trader.TraderId,
                        Email = s.Trader.Email,
                        Name = s.Trader.Name,
                        Phone = s.Trader.Phone
                    },
                    Amount = s.Amount,
                }).FirstOrDefaultAsync();

            if (traderPortfolio == null)
            {
                return NotFound();
            }

            return traderPortfolio;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraderPortfolio(Guid id, TraderPortfolioUpdateRequest traderPortfolio)
        {
            if (id != traderPortfolio.TraderPortfolioId)
            {
                return BadRequest("TraderPortfolio Id not exist!");
            }

            var updatedTraderPortfolio = await _context.TraderPortfolioes.FindAsync(id);

            if (updatedTraderPortfolio == null)
            {
                return NotFound(nameof(traderPortfolio));
            }

            updatedTraderPortfolio.TraderId = traderPortfolio.TraderId;
            updatedTraderPortfolio.ShareId = traderPortfolio.ShareId;
            updatedTraderPortfolio.Amount = traderPortfolio.Amount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraderPortfolioExists(id))
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
        public async Task<ActionResult<TraderPortfolioDto>> PostTraderPortfolio(TraderPortfolioUpdateRequest traderPortfolio)
        {
            _context.TraderPortfolioes.Add(new TraderPortfolio()
            {
                TraderPortfolioId = traderPortfolio.TraderPortfolioId,
                ShareId = traderPortfolio.ShareId,
                TraderId = traderPortfolio.TraderId,
                Amount = traderPortfolio.Amount,
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTraderPortfolio", new { id = traderPortfolio.TraderPortfolioId }, traderPortfolio);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraderPortfolio(Guid id)
        {
            var traderPortfolio = await _context.TraderPortfolioes.FindAsync(id);
            if (traderPortfolio == null)
            {
                return NotFound();
            }

            _context.TraderPortfolioes.Remove(traderPortfolio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TraderPortfolioExists(Guid id)
        {
            return _context.TraderPortfolioes.Any(e => e.TraderPortfolioId == id);
        }
    }
}
