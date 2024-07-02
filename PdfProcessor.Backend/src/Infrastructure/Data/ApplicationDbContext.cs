using Microsoft.EntityFrameworkCore;
using PdfProcessor.Backend.src.Domain.Models;

namespace PdfProcessor.Backend.src.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TrainedModel> TrainedModels { get; set; }
    }

    public class TrainedModel
    {
        public int Id { get; set; }
        public byte[] ModelData { get; set; }
    }
}
