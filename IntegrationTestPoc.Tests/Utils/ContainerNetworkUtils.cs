using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

namespace IntegrationTestPoc.Tests.Utils;

public static class ContainerNetworkUtils
{
    public static INetwork Build()
    {
        return new NetworkBuilder()
            .WithCleanUp(true)
            .WithName($"integration-tests-network-{Guid.NewGuid()}")
            .Build();
    }
}