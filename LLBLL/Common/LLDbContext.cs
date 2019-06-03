using LLBLL.Model;
using Microsoft.EntityFrameworkCore;

namespace LLBLL.Common
{
    public class LLDbContext : DbContext
    {

        public LLDbContext(DbContextOptions<LLDbContext> options)
        : base(options)
        {
          
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //指定表
            modelBuilder.Entity<Users>().ToTable("LL_Users");

            // ef core 并发字段 要添加 rowVerSion字段
            //            modelBuilder.Entity<LL_Users>()
            //.Property(p => p.RowVersion).IsConcurrencyToken();
            base.OnModelCreating(modelBuilder);
        }
    }
}
