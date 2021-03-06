﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.SpecialRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.SpecialRules;
    using TestHelper;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.SpecialRules.SA0001XmlCommentAnalysisDisabled>;

    /// <summary>
    /// Unit tests for <see cref="SA0001XmlCommentAnalysisDisabled"/>.
    /// </summary>
    public class SA0001UnitTests
    {
        [Theory]
        [InlineData(DocumentationMode.Parse)]
        [InlineData(DocumentationMode.Diagnose)]
        public async Task TestEnabledDocumentationModesAsync(DocumentationMode documentationMode)
        {
            var testCode = @"public class Foo
{
}
";

            await new CSharpTest
            {
                TestCode = testCode,
                SolutionTransforms =
                {
                    (solution, projectId) =>
                    {
                        var project = solution.GetProject(projectId);
                        return solution.WithProjectParseOptions(projectId, project.ParseOptions.WithDocumentationMode(documentationMode));
                    },
                },
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData(DocumentationMode.None)]
        public async Task TestDisabledDocumentationModesAsync(DocumentationMode documentationMode)
        {
            var testCode = @"public class Foo
{
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics = { expected },
                SolutionTransforms =
                {
                    (solution, projectId) =>
                    {
                        var project = solution.GetProject(projectId);
                        return solution.WithProjectParseOptions(projectId, project.ParseOptions.WithDocumentationMode(documentationMode));
                    },
                },
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
