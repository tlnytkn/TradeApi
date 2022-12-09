using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeApi.Domain;
using TradeApi.Domain.Enums;
using TradeApi.Dtos;
using TradeApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TradeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly TradeApiDbContext _context;

        public TradesController(TradeApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllTrades")]
        public async Task<ActionResult<IEnumerable<TradeDto>>> GetTrades()
        {
            return await _context.Trades
                .Select(s => new TradeDto
                {
                    TradeId = s.TradeId,
                    ShareId = s.ShareId,
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
                    Type = s.Type,
                    TradeDate = s.TradeDate,
                    Price = s.Price,
                    Amount = s.Amount,
                })
                .ToListAsync();
        }

        [HttpGet("GetTradesByTraderId/{id}")]
        public async Task<ActionResult<IEnumerable<TradeDto>>> GetTradesByTraderId(Guid id)
        {
            return await _context.Trades
                .Include(i => i.Trader)
                .Include(i => i.Share)
                .Where(w => w.TraderId == id)
                .OrderBy(o => o.TradeDate)
                .Select(s => new TradeDto
                {
                    TradeId = s.TradeId,
                    ShareId = s.ShareId,
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
                    Type = s.Type,
                    TradeDate = s.TradeDate,
                    Price = s.Price,
                    Amount = s.Amount,
                })
                .ToListAsync();
        }

        [HttpGet("GetTradesByShareId/{id}")]
        public async Task<ActionResult<IEnumerable<TradeDto>>> GetTradesByShareId(Guid id)
        {
            return await _context.Trades
                .Where(w => w.ShareId == id)
                .OrderBy(o => o.TradeDate)
                .Select(s => new TradeDto
                {
                    TradeId = s.TradeId,
                    ShareId = s.ShareId,
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
                    Type = s.Type,
                    TradeDate = s.TradeDate,
                    Price = s.Price,
                    Amount = s.Amount,
                })
                .ToListAsync();
        }

        [HttpGet("GetTrade/{id}")]
        public async Task<ActionResult<TradeDto>> GetTrade(Guid id)
        {
            var trade = await _context.Trades.FindAsync(id);

            if (trade == null)
            {
                return NotFound();
            }

            var result = new TradeDto()
            {
                TradeId = trade.TradeId,
                ShareId = trade.ShareId,
                Share = new ShareDto
                {
                    Name = trade.Share.Name,
                    Price = trade.Share.Price,
                    ShareId = trade.Share.ShareId,
                    Symbol = trade.Share.Symbol
                },
                TraderId = trade.TraderId,
                Trader = new TraderDto
                {
                    TraderId = trade.Trader.TraderId,
                    Email = trade.Trader.Email,
                    Name = trade.Trader.Name,
                    Phone = trade.Trader.Phone
                },
                Type = trade.Type,
                TradeDate = trade.TradeDate,
                Price = trade.Price,
                Amount = trade.Amount,
            };

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrade(Guid id, TradeUpdateRequest trade)
        {
            if (id != trade.TradeId)
            {
                return BadRequest("Trade Id not exist!");
            }

            var updatedTrade = await _context.Trades.FindAsync(id);

            if (updatedTrade == null)
            {
                return NotFound(nameof(trade));
            }

            updatedTrade.TraderId = trade.TraderId;
            updatedTrade.ShareId = trade.ShareId;
            updatedTrade.Price = trade.Price;
            updatedTrade.Type = trade.Type;
            updatedTrade.Amount = trade.Amount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(Guid id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
            {
                return NotFound();
            }

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("BuyTrade")]
        public async Task<IActionResult> BuyTrade(BuySellTradeRequest request)
        {
           

            var share = await _context.Shares.FindAsync(request.ShareId);
            if (share == null)
            {
                return NotFound();
            }

            var trader = await _context.Traders
                .Include(i => i.TraderPortfolios)
                .Where(w => w.TraderId == request.TraderId)
                .FirstOrDefaultAsync();

            if (trader == null)
            {
                return NotFound();
            }

            var transection = _context.Database.BeginTransaction();

            try
            {
                _context.Trades.Add(new Trade
                {
                    TradeId = Guid.NewGuid(),
                    TraderId = request.TraderId,
                    ShareId = request.ShareId,
                    TradeDate = DateTime.UtcNow,
                    Type = TradeType.Buy,
                    Price = share.Price,
                    Amount = request.Amount
                });

                var portfolio = trader.TraderPortfolios
                    .Where(w => w.ShareId == request.ShareId)
                    .FirstOrDefault();

                if (portfolio == null)
                {
                    _context.TraderPortfolioes.Add(new TraderPortfolio
                    {
                        TraderPortfolioId = Guid.NewGuid(),
                        ShareId = request.ShareId,
                        Amount = request.Amount,
                        TraderId = trader.TraderId
                    });
                }
                else
                {
                    portfolio.Amount += request.Amount;
                }

                await _context.SaveChangesAsync();
                await transection.CommitAsync();
            }
            catch (Exception)
            {
                await transection.RollbackAsync();
            }

            return NoContent();
        }

        [HttpPost("SellTrade")]
        public async Task<IActionResult> SellTrade(BuySellTradeRequest request)
        {
            var share = await _context.Shares.FindAsync(request.ShareId);
            if (share == null)
            {
                return NotFound();
            }

            var trader = await _context.Traders
                .Include(i => i.TraderPortfolios)
                .Where(w => w.TraderId == request.TraderId)
                .FirstOrDefaultAsync();

            if (trader == null)
            {
                return NotFound();
            }

            var portfolio = trader.TraderPortfolios
                    .Where(w => w.ShareId == request.ShareId)
                    .FirstOrDefault();

            if (portfolio == null || portfolio.Amount < request.Amount)
            {
                return BadRequest("Portfolio not found or insufficient balance");
            }

            try
            {
                _context.Database.BeginTransaction();

                _context.Trades.Add(new Trade
                {
                    TradeId = Guid.NewGuid(),
                    TraderId = request.TraderId,
                    ShareId = request.ShareId,
                    TradeDate = DateTime.UtcNow,
                    Type = TradeType.Sell,
                    Price = share.Price,
                    Amount = request.Amount
                });

                portfolio.Amount -= request.Amount;

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _context.Database.RollbackTransactionAsync();
                throw;
            }

            return NoContent();
        }

        private bool TradeExists(Guid id)
        {
            return _context.Trades.Any(e => e.TradeId == id);
        }
    }
}
