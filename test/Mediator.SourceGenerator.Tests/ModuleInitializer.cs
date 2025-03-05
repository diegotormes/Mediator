using System.IO;
using DiffEngine;
using Microsoft.Build.Locator;
using VerifyTests;
using VerifyXunit;

namespace Mediator.SourceGenerator.Tests;

public static class ModuleInitializer
{
    private static VisualStudioInstance? _instance;

    // ModuleInitializer should only be used in apps
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void Init()
    {
        _instance ??= MSBuildLocator.RegisterDefaults();

        DiffRunner.Disabled = false; //Original value: true

        Verifier.DerivePathInfo(
            (file, _, type, method) => new(Path.Join(Path.GetDirectoryName(file), "_snapshots"), type.Name, method.Name)
        );

        VerifySourceGenerators.Initialize();
        VerifyDiffPlex.Initialize(VerifyTests.DiffPlex.OutputType.Compact);
        //
        VerifierSettings.InitializePlugins();
        VerifierSettings.ScrubLinesContaining("DiffEngineTray");
        VerifierSettings.IgnoreStackTrace();
        DiffTools.UseOrder(DiffTool.VisualStudioCode, DiffTool.VisualStudio);
    }
}
