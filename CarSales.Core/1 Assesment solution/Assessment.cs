using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Core.Assesment_solution
{
    public class CarMake
    {
        public int MakeId { get; set; }
        public string MakeName { get; set; } = string.Empty;

        public List<CarModel> Models { get; set; } = new();
    }

    public class CarModel
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;

        public int MakeId { get; set; }
        public CarMake Make { get; set; } = null!;

        public List<CarSubModel> SubModels { get; set; } = new();
    }

    public class CarSubModel
    {
        public int SubModelId { get; set; }
        public string SubModelName { get; set; } = string.Empty;

        public int ModelId { get; set; }
        public CarModel Model { get; set; } = null!;

        public List<Car> Cars { get; set; } = new();
    }

    public class ZipCodes
    {
        public int ZipCodeId { get; set; }
        public string ZipCode { get; set; } = string.Empty;

        public List<Car> Cars { get; set; } = new();
        public List<BuyerCoverageZipCodes> BuyerCoverages { get; set; } = new();
    }

    public class Car
    {
        public int CarId { get; set; }
        public int SubModelId { get; set; }
        public CarSubModel SubModel { get; set; } = null!;
        public short CarYear { get; set; }
        public int ZipCodeId { get; set; }
        public ZipCodes ZipCode { get; set; } = null!;

        public CarCurrentQuote? CarCurrentQuote { get; set; }
        public List<CarStatus> CarStatuses { get; set; } = new();
    }

    public class Buyer
    {
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<BuyerCoverageZipCodes> CoverageZipCodes { get; set; } = new();
        public List<BuyerQuoteRule> QuoteRules { get; set; } = new();
        public List<CarCurrentQuote> CurrentQuotes { get; set; } = new();
    }

    public class BuyerCoverageZipCodes
    {
        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; } = null!;

        public int ZipCodeId { get; set; }
        public ZipCodes ZipCode { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

    }

    public class BuyerQuoteRule
    {
        public int RuleId { get; set; }

        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; } = null!;

        public int? MakeId { get; set; }
        public CarMake? Make { get; set; }

        public int? ModelId { get; set; }
        public CarModel? Model { get; set; }

        public int? SubModelId { get; set; }
        public CarSubModel? SubModel { get; set; }

        public short? YearFrom { get; set; }
        public short? YearTo { get; set; }

        public decimal Amount { get; set; }

        public int RulePriority { get; set; } = 100;
        public bool IsActive { get; set; } = true;
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public List<BuyerQuoteRuleZip> RuleZips { get; set; } = new();
    }


    public class BuyerQuoteRuleZip
    {
        public int RuleId { get; set; }
        public BuyerQuoteRule Rule { get; set; } = null!;

        public int ZipCodeId { get; set; }
        public ZipCodes ZipCode { get; set; } = null!;
    }

    public class CarCurrentQuote
    {
        public int CarCurrentQuoteId { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime ChosenAt { get; set; }
        public string? ChosenBy { get; set; }
    }

    public class StatusType
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public bool IsDateRequired { get; set; }

        public List<CarStatus> CarStatuses { get; set; } = new();
    }

    public class CarStatus
    {
        public int CarStatusId { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        public int StatusId { get; set; }
        public StatusType StatusType { get; set; } = null!;

        public DateTime? StatusDate { get; set; } // obligatorio si StatusType.IsDateRequired=true
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CarMake> CarMake => Set<CarMake>();
        public DbSet<CarModel> CarModel => Set<CarModel>();
        public DbSet<CarSubModel> CarSubModel => Set<CarSubModel>();
        public DbSet<ZipCodes> ZipCodes => Set<ZipCodes>();
        public DbSet<Car> Car => Set<Car>();

        public DbSet<Buyer> Buyer => Set<Buyer>();
        public DbSet<BuyerCoverageZipCodes> BuyerCoverageZipCodes => Set<BuyerCoverageZipCodes>();
        public DbSet<BuyerQuoteRule> BuyerQuoteRule => Set<BuyerQuoteRule>();
        public DbSet<BuyerQuoteRuleZip> BuyerQuoteRuleZip => Set<BuyerQuoteRuleZip>();

        public DbSet<CarCurrentQuote> CarCurrentQuote => Set<CarCurrentQuote>();

        public DbSet<StatusType> StatusType => Set<StatusType>();
        public DbSet<CarStatus> CarStatus => Set<CarStatus>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<CarMake>()
                .HasIndex(x => x.MakeName).IsUnique();

           
            modelBuilder.Entity<CarModel>()
                .HasIndex(x => new { x.MakeId, x.ModelName }).IsUnique();

         
            modelBuilder.Entity<CarSubModel>()
                .HasIndex(x => new { x.ModelId, x.SubModelName }).IsUnique();

            
            modelBuilder.Entity<ZipCodes>()
                .HasIndex(x => x.ZipCode).IsUnique();

          
            modelBuilder.Entity<BuyerCoverageZipCodes>()
                .HasKey(x => new { x.BuyerId, x.ZipCodeId });

            modelBuilder.Entity<BuyerCoverageZipCodes>()
                .HasOne(x => x.Buyer)
                .WithMany(b => b.CoverageZipCodes)
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuyerCoverageZipCodes>()
                .HasOne(x => x.ZipCode)
                .WithMany(z => z.BuyerCoverages)
                .HasForeignKey(x => x.ZipCodeId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<BuyerQuoteRule>()
                .HasOne(r => r.Buyer)
                .WithMany(b => b.QuoteRules)
                .HasForeignKey(r => r.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<BuyerQuoteRuleZip>()
                .HasKey(x => new { x.RuleId, x.ZipCodeId });

            modelBuilder.Entity<BuyerQuoteRuleZip>()
                .HasOne(x => x.Rule)
                .WithMany(r => r.RuleZips)
                .HasForeignKey(x => x.RuleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuyerQuoteRuleZip>()
                .HasOne(x => x.ZipCode)
                .WithMany()
                .HasForeignKey(x => x.ZipCodeId);

          
            modelBuilder.Entity<CarCurrentQuote>()
                .HasIndex(x => x.CarId).IsUnique();

            
            modelBuilder.Entity<CarStatus>()
                .HasIndex(x => new { x.CarId, x.IsCurrent })
                .IsUnique();

          
            modelBuilder.Entity<StatusType>()
                .HasIndex(x => x.StatusName).IsUnique();
        }
    }

  
    public sealed class CarSummaryDto
    {
        public int CarId { get; init; }
        public string MakeName { get; init; } = string.Empty;
        public string ModelName { get; init; } = string.Empty;
        public string SubModelName { get; init; } = string.Empty;
        public short CarYear { get; init; }
        public string ZipCode { get; init; } = string.Empty;
        public string? CurrentBuyerName { get; init; }
        public decimal? CurrentBuyerQuote { get; init; }
        public string? CurrentStatusName { get; init; }
        public DateTime? CurrentStatusDate { get; init; }
    }

    public static class CarQueries
    {
        // Point 1 EF Core
        public static async Task<CarSummaryDto?> GetCarSummaryByIdAsync(this AppDbContext ctx, int carId)
        {
            var result = await ctx.Car
                        .AsNoTracking()
                        .Where(c => c.CarId == carId)
                        .Select(c => new CarSummaryDto
                        {
                            CarId = c.CarId,
                            MakeName = c.SubModel.Model.Make.MakeName,
                            ModelName = c.SubModel.Model.ModelName,
                            SubModelName = c.SubModel.SubModelName,
                            CarYear = c.CarYear,
                            ZipCode = c.ZipCode.ZipCode,
                            CurrentBuyerName = c.CarCurrentQuote != null ? c.CarCurrentQuote.Buyer.BuyerName : null,
                            CurrentBuyerQuote = c.CarCurrentQuote != null ? (decimal?)c.CarCurrentQuote.Amount : null,
                            CurrentStatusName = c.CarStatuses.Where(s => s.IsCurrent).Select(s => s.StatusType.StatusName).FirstOrDefault(),
                            CurrentStatusDate = c.CarStatuses.Where(s => s.IsCurrent).Select(s => (DateTime?)s.StatusDate).FirstOrDefault()
                        })
                        .SingleOrDefaultAsync();
            return result;

        }

      
    }

     //assessment: calculate the best quote and upsert
    public static class QuoteCalculator
    {
  
        public static async Task UpsertCurrentQuoteAsync(this AppDbContext ctx, int carId)
        {
            var car = await ctx.Car
                .Include(c => c.SubModel)
                    .ThenInclude(sm => sm.Model)
                        .ThenInclude(m => m.Make)
                .SingleAsync(c => c.CarId == carId);

            var makeId = car.SubModel.Model.Make.MakeId;
            var modelId = car.SubModel.Model.ModelId;
            var subModelId = car.SubModel.SubModelId;
            var year = car.CarYear;
            var zipId = car.ZipCodeId;
            var now = DateTime.UtcNow;

           
            var best = await (
                from cov in ctx.BuyerCoverageZipCodes
                where cov.ZipCodeId == zipId
                join r in ctx.BuyerQuoteRule on cov.BuyerId equals r.BuyerId
                where r.IsActive
                   && (r.EffectiveFrom == null || r.EffectiveFrom <= now)
                   && (r.EffectiveTo == null || r.EffectiveTo >= now)
                   && (r.MakeId == null || r.MakeId == makeId)
                   && (r.ModelId == null || r.ModelId == modelId)
                   && (r.SubModelId == null || r.SubModelId == subModelId)
                   && (r.YearFrom == null || r.YearFrom <= year)
                   && (r.YearTo == null || r.YearTo >= year)
                  
                   && (!ctx.BuyerQuoteRuleZip.Any(z => z.RuleId == r.RuleId) || ctx.BuyerQuoteRuleZip.Any(z => z.RuleId == r.RuleId && z.ZipCodeId == zipId))
                orderby r.RulePriority, r.Amount descending, r.BuyerId
                select new { r.BuyerId, r.Amount }
            ).FirstOrDefaultAsync();

          
            
            var existing = await ctx.CarCurrentQuote.SingleOrDefaultAsync(x => x.CarId == carId);

            if (best == null)
            {
                if (existing != null)
                {
                    ctx.CarCurrentQuote.Remove(existing);
                    await ctx.SaveChangesAsync();
                }
                return;
            }

            if (existing == null)
            {
                ctx.CarCurrentQuote.Add(new CarCurrentQuote
                {
                    CarId = carId,
                    BuyerId = best.BuyerId,
                    Amount = best.Amount,
                    ChosenAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.BuyerId = best.BuyerId;
                existing.Amount = best.Amount;
                existing.ChosenAt = DateTime.UtcNow;
            }

            await ctx.SaveChangesAsync();
        }
    }
}
