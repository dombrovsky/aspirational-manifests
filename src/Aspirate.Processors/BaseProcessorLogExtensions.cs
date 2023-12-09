namespace Aspirate.Processors;

[ExcludeFromCodeCoverage]
public static class BaseProcessorLogExtensions
{
    public static void LogCreateManifestNotOverridden(this IAnsiConsole console, string processor) =>
        console.MarkupLine($"[bold yellow]Processor {processor} has not been configured. CreateManifest must be overridden.[/]");

    public static void LogCompletion(this IAnsiConsole console, string outputPath) =>
        console.MarkupLine($"[green]({EmojiLiterals.CheckMark}) Done: [/] Generating [blue]{outputPath}[/]");
}