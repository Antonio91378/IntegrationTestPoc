using Microsoft.Extensions.Configuration;

namespace IntegrationTestPoc.Util;
public interface IAppConfiguration
{
    string GetSqlServerConnectionString();
    int GetDatabasePort();
    
}
public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _config;
    public AppConfiguration(IConfiguration configuration)
    {
        _config = configuration;
    }
    
    public string GetSqlServerConnectionString()
    {
        var cn = _config.GetSection("ConnectionStrings:SqlServer").Value ?? String.Empty;
        return cn;
    }

    public int GetDatabasePort()
    {
        var connectionString = GetSqlServerConnectionString();
        var startIndex = connectionString.IndexOf(',') + 1;
        var endIndex = connectionString.IndexOf(';', startIndex);
        if (startIndex > 0 && endIndex > startIndex)
        {
            var portString = connectionString[startIndex..endIndex];
            return int.TryParse(portString, out var port) ? port : 1433;
        }
        return 1433; 
    }

}