using System.Data.Common;
using System.Diagnostics;
using DotNet.Testcontainers.Networks;
using IntegrationTestPoc.Tests.Utils;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Dapper;
using IntegrationTestPoc.Util;
using Microsoft.Extensions.Configuration;

namespace IntegrationTestPoc.Tests.Infra;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly Database _database;
    private readonly INetwork _network;
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);

    public DatabaseFixture()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.Test.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
        var appConfiguration = new AppConfiguration(configuration);

        _network = ContainerNetworkUtils.Build();
        _database = new Database(_network, appConfiguration);
    }

    public async Task InitializeAsync()
    {
        using var cancellationTokenSource = new CancellationTokenSource(_timeout);
        try
        {
            await Database.InitializeAsync(cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Database initialization timed out.");
            throw; 
        }
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _database.DisposeAsync();

            await _network.DeleteAsync();
            await _network.DisposeAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}

public class Database : IAsyncDisposable
{
    private static readonly string ResetScript = $"""
                                                  USE [{Constants.DatabaseName}]
                                                  EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
                                                  EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasForeignRef'') = 0 BEGIN TRUNCATE TABLE ? END ELSE DELETE FROM ?'
                                                  EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'
                                                  """;

    private static MsSqlContainer? _container;

    public Database(INetwork network,  IAppConfiguration appConfiguration)
    {
        var port = appConfiguration.GetDatabasePort();

        _container = new MsSqlBuilder()
            .WithName(Constants.DatabaseName)
            .WithPassword(Constants.DatabasePassword)
            .WithPortBinding(port, MsSqlBuilder.MsSqlPort)
            .WithNetwork(network)
            .Build();
    }

    public static async ValueTask InitializeAsync(CancellationToken token)
    {
        if (_container != null)
        {
            await _container.StartAsync(token);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await CleanUp();
        if (_container != null)
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
        }
    }

    private static async Task CleanUp()
    {
        await using var connection = await GetConnection();
        await connection.ExecuteScalarAsync(ResetScript);
        await connection.CloseAsync();
    }

    private static async Task<DbConnection> GetConnection()
    {
        var connection = new SqlConnection(GetConnectionString());
        await connection.OpenAsync();
        return connection;
    }

    private static string GetConnectionString()
    {
        return _container != null ? _container.GetConnectionString() : string.Empty;
    }

}