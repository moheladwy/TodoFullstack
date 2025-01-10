using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Todo.Infrastructure.DatabaseContexts;

public class TodoIdentityContextFactory : IDesignTimeDbContextFactory<TodoIdentityContext>
{
  public TodoIdentityContext CreateDbContext(string[] args)
  {
    const string sqlServerConnectionString = "Server=sql.bsite.net\\MSSQL2016;Database=aladawy_todo_fullstack;User Id=aladawy_todo_fullstack;Password=Al-Adawy@123;trustServerCertificate=true;";
    var optionsBuilder = new DbContextOptionsBuilder<TodoIdentityContext>();
    optionsBuilder.UseSqlServer(sqlServerConnectionString);

    return new TodoIdentityContext(optionsBuilder.Options);
  }
}