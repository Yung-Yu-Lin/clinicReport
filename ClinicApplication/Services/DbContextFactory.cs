namespace ClinicApplication.Services
{
    public class DbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext(string connectionName)
        {
            var connectionString = _configuration.GetConnectionString(connectionName);
            return new ApplicationDbContext(connectionString);
        }
    }
}
