namespace IntegrationTestPoc.Tests.Infra;

[CollectionDefinition("WhiteboxAppFixture")]
public class FixtureCollection :
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<CustomWebApplicationFactory>
{
}