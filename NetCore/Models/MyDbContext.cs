using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NetCore.Common.Tool;

namespace NetCore.Models
{
    public class MyDbContext : DbContext
    {
        //日志监控配置
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[]
    {
        //窗口输出
        new ConsoleLoggerProvider((category, level)
            => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information, true)
    });

        public MyDbContext(DbContextOptions<MyDbContext> options)
          : base(options)
        {
        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<MoviePrices> MoviePrices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            //ef 日志监控sql 
            //var loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new EFLoggerProvider());
            OptionsBuilder.UseLoggerFactory(MyLoggerFactory);
           // OptionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=Core;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //指定表
            modelBuilder.Entity<MoviePrices>().ToTable("MoviePrice");

            // ef core 并发字段
            modelBuilder.Entity<Movies>()
.Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}
