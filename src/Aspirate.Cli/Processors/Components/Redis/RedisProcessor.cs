using AspireRedis = Aspirate.Contracts.Models.AspireManifests.Components.V0.Redis;

namespace Aspirate.Cli.Processors.Components.Redis;

/// <summary>
/// Handles producing the Redis component as Kustomize manifest.
/// </summary>
public class RedisProcessor(IFileSystem fileSystem, IAnsiConsole console) : BaseProcessor<RedisTemplateData>(fileSystem, console)
{
    private readonly IReadOnlyCollection<string> _manifests =
    [
        $"{TemplateLiterals.RedisType}.yml",
    ];

    /// <inheritdoc />
    public override string ResourceType => AspireLiterals.Redis;

    /// <inheritdoc />
    public override Resource? Deserialize(ref Utf8JsonReader reader) =>
        JsonSerializer.Deserialize<AspireRedis>(ref reader);

    public override Task<bool> CreateManifests(KeyValuePair<string, Resource> resource, string outputPath, AspirateSettings? aspirateSettings = null)
    {
        var resourceOutputPath = Path.Combine(outputPath, resource.Key);

        EnsureOutputDirectoryExistsAndIsClean(resourceOutputPath);

        var data = new RedisTemplateData(_manifests);

        CreateCustomManifest(resourceOutputPath, $"{TemplateLiterals.RedisType}.yml", TemplateLiterals.RedisType, data, aspirateSettings);
        CreateComponentKustomizeManifest(resourceOutputPath, data, aspirateSettings);

        LogCompletion(resourceOutputPath);

        return Task.FromResult(true);
    }
}
