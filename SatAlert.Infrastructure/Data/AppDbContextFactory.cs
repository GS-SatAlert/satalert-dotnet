using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SatAlert.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseOracle("Data Source=oracle.fiap.com.br:1521/orcl;User ID=RM562310;Password=270905;");
        return new AppDbContext(options.Options);
    }
}
