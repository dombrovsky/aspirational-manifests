﻿namespace Aspirate.Commands.Options;

public sealed class OutputFormatOption : BaseOption<string?>
{
    private static readonly string[] _aliases = { "--output-format" };

    private OutputFormatOption() : base(_aliases, "ASPIRATE_OUTPUT_FORMAT", null)
    {
        Name = nameof(IGenerateOptions.OutputFormat);
        Description = "The output format of the generated manifests. Supported values are 'kustomize', 'compose', and 'helm'.";
        Arity = ArgumentArity.ZeroOrOne;
        IsRequired = false;
        this.AddValidator(ValidateFormat);
    }

    public static OutputFormatOption Instance { get; } = new();

    private static void ValidateFormat(OptionResult optionResult)
    {
        var value = optionResult.GetValueOrDefault<string?>();

        if (value is null)
        {
            return;
        }

        if (!OutputFormat.TryFromValue(value, out _))
        {
            var errorBuilder = new StringBuilder();
            errorBuilder.Append("--output-format must be one of: '");
            errorBuilder.AppendJoin("', '", OutputFormat.List.Select(x => x.Value));
            errorBuilder.Append("' and not quoted.");

            throw new ArgumentException(errorBuilder.ToString());
        }
    }
}
