
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\WIN24\\Y1\\4.Datalagring\\Data\\DataStorage_Assigment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30");

        return new DataContext(optionsBuilder.Options);
    }
}

