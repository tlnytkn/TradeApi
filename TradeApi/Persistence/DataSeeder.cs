using TradeApi.Domain;

namespace TradeApi.Persistence;

public class DataSeeder
{
    private readonly TradeApiDbContext dbContext;

    public DataSeeder(TradeApiDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Seed()
    {
        if (!dbContext.Shares.Any())
        {
            var shares = new List<Share>()
            {
                new Share
                {
                    ShareId = Guid.NewGuid(),
                    Symbol = "THY",
                    Name = "Turk Hava Yollari",
                    Price = 1.25m
                },
                new Share
                {
                    ShareId = Guid.NewGuid(),
                    Symbol = "TCL",
                    Name = "Turkcell",
                    Price = 10m
                },
                new Share
                {
                    ShareId = Guid.NewGuid(),
                    Symbol = "MST",
                    Name = "Microsoft Turkiye",
                    Price = 0.25m
                },
                new Share
                {
                    ShareId = Guid.NewGuid(),
                    Symbol = "TLN",
                    Name = "Tolunay Coin",
                    Price = 5.2m
                }
            };

            dbContext.Shares.AddRange(shares);
            dbContext.SaveChanges();
        }
    }
}