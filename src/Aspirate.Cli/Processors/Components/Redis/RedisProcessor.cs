using AspireRedis = Aspirate.Contracts.Models.AspireManifests.Components.V0.Redis;

namespace Aspirate.Cli.Processors.Components.Redis;

/// <summary>
/// Handles producing the Redis component as Kustomize manifest.
/// </summary>
public class RedisProcessor(IFileSystem fileSystem) : BaseProcessor<RedisTemplateData>(fileSystem)
{
    private readonly IReadOnlyCollection<string> _manifests =
    [
        $"{TemplateLiterals.RedisType}.yml",
    ];

    /// <inheritdoc />
    public override string ResourceType => AspireResourceLiterals.Redis;

    /// <inheritdoc />
    public override Resource? Deserialize(ref Utf8JsonReader reader) =>
        JsonSerializer.Deserialize<AspireRedis>(ref reader);

    public override Task<bool> CreateManifests(KeyValuePair<string, Resource> resource, string outputPath)
    {
        var resourceOutputPath = Path.Combine(outputPath, resource.Key);

        EnsureOutputDirectoryExistsAndIsClean(resourceOutputPath);

        var data = new RedisTemplateData(_manifests);

        CreateCustomManifest(resourceOutputPath, $"{TemplateLiterals.RedisType}.yml", TemplateLiterals.RedisType, data);
        CreateComponentKustomizeManifest(resourceOutputPath, data);

        LogCompletion(resourceOutputPath);

        return Task.FromResult(true);
    }
}