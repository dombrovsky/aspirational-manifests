namespace Aspirate.Tests.ServiceTests;

[UsesVerify]
public class ContainerDetailsServiceTests
{
    [ModuleInitializer]
    internal static void Initialize() =>
        VerifierSettings.NameForParameter<TestContainerProperties>(_ => _.Value);

    [Theory]
    [MemberData(nameof(MockContainerProperties))]
    public async Task GetContainerDetails_WhenCalled_ReturnsCorrectContainerDetails(TestContainerProperties properties)
    {
        // Arrange
        var projectPropertyService = Substitute.For<IProjectPropertyService>();

        var responseJson = JsonSerializer.Serialize(properties.Properties);

        projectPropertyService
            .GetProjectPropertiesAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .ReturnsForAnyArgs(responseJson);


        var containerDetailsService = new ContainerDetailsService(projectPropertyService);

        // Act
        var containerDetails = await containerDetailsService.GetContainerDetails("test-service", new());

        // Assert
        await Verify(containerDetails)
            .UseParameters(properties)
            .UseDirectory("VerifyResults");
    }

    public static IEnumerable<object[]> MockContainerProperties =>
        new List<object[]>
        {
            new object[]
            {
                new TestContainerProperties(
                    "FullResponse", CreateContainerProperties("test-registry", "test-repository", "test-image", "test-tag")),
            },
            new object[]
            {
                new TestContainerProperties(
                    "NoTagShouldBeLatest", CreateContainerProperties("test-registry", "test-repository", "test-image")),
            },
            new object[]
            {
                new TestContainerProperties(
                    "NoImageShouldBeRepository", CreateContainerProperties("test-registry", "test-repository", null, "test-tag")),
            },
            new object[]
            {
                new TestContainerProperties(
                    "NoImageOrTagShouldBeRepositoryLatest", CreateContainerProperties("test-registry", "test-repository")),
            },
            new object[]
            {
                new TestContainerProperties(
                    "NoRepositoryShouldBeImage", CreateContainerProperties("test-registry", null, "test-image", "test-tag")),
            },
            new object[]
            {
                new TestContainerProperties(
                    "NoRepositoryOrTagShouldBeImageLatest", CreateContainerProperties("test-registry", null, "test-image")),
            },
        };

    private static ContainerProperties CreateContainerProperties(string? registry = null,
        string? repo = null,
        string? image = null,
        string? tag = null) =>
        new()
        {
            Properties = new()
            {
                ContainerRegistry = registry, ContainerRepository = repo, ContainerImage = image, ContainerImageTag = tag,
            },
        };

    public record TestContainerProperties(string Value, ContainerProperties Properties);
}