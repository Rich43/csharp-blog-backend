using blog.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace blog.Database {
    public class BlogDBContext : DbContext {
        public DbSet<BlogEntry> BlogEntries { get; set; }
        private readonly IConfiguration Configuration;
        private string SqlServerConnString { get; }
        private string MysqlConnString { get; }
        private string SqliteConnString { get; }
        private string PostgreSQLConnString { get; }

        public BlogDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
            SqlServerConnString = Configuration["ConnectionStrings:Data:SqlServer"];
            PostgreSQLConnString = Configuration["ConnectionStrings:Data:PostgreSQL"];
            MysqlConnString = Configuration["ConnectionStrings:Data:Mysql"];
            SqliteConnString = Configuration["ConnectionStrings:Data:Sqlite"];
        }

        override protected void OnConfiguring(DbContextOptionsBuilder options) {
            if (PostgreSQLConnString != null) {
                options.UseNpgsql(PostgreSQLConnString);
            } else if (MysqlConnString != null) {
                options.UseMySql(MysqlConnString, ServerVersion.AutoDetect(MysqlConnString));
            } else if (SqlServerConnString != null) {
                options.UseSqlServer(SqlServerConnString);
            } else if (SqliteConnString != null) {
                options.UseSqlite(SqliteConnString);
            }
        }
    }
}