// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.CodeGenerator.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using KomberNet.Models.CodeGenerator.Writers;
    using Microsoft.CodeAnalysis;

    internal static class SourceProductionContextExtensions
    {
        public static void WriteNewCSFile(this SourceProductionContext sourceProductionContext, string fileNameWithoutExtension, CSFileWriter fileWriter)
        {
            var fileName = $"{fileNameWithoutExtension}.g.cs";
            var fileContent = fileWriter.GetFileContent();

            sourceProductionContext.AddSource(fileName, fileContent);
            Debug.WriteLine($"The {fileName} was auto generated with this content: " + fileContent);
            ReportGeneratedCode(sourceProductionContext, fileContent);
        }

        public static void WriteNewCSFile(this SourceProductionContext sourceProductionContext, string fileNameWithoutExtension, string fileContent)
        {
            var fileName = $"{fileNameWithoutExtension}.g.cs";

            sourceProductionContext.AddSource(fileName, fileContent);
            Debug.WriteLine($"The {fileName} was auto generated with this content: " + fileContent);
            ReportGeneratedCode(sourceProductionContext, fileContent);
        }

        private static void ReportGeneratedCode(SourceProductionContext context, string sourceCode)
    {
        var descriptor = new DiagnosticDescriptor(
            id: "MYGEN001",
            title: "Generated Source Code",
            messageFormat: "Generated Source Code: {0}",
            category: "SourceGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        var diagnostic = Diagnostic.Create(descriptor, Location.None, sourceCode.Replace("\n", " "));
        context.ReportDiagnostic(diagnostic);
    }
    }
}
