// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.CodeGenerator.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using KomberNet.Models.CodeGenerator.CodeWriters;
    using KomberNet.Models.CodeGenerator.Structure;
    using Microsoft.CodeAnalysis;

    internal static class CodeGeneratorHelper
    {
        public static void Generate(CodeGeneratorSettings codeGeneratorSettings, List<CodeGenerator> codeGenerators, SourceProductionContext sourceProductionContext)
        {
            if (codeGeneratorSettings.BackendEnumsSettings != null || codeGeneratorSettings.FrontendEnumsSettings != null)
            {
                EnumsCodeWriter.Generate(codeGeneratorSettings, codeGenerators, sourceProductionContext);
            }

            if (codeGeneratorSettings.BackendEntititesSettings != null || codeGeneratorSettings.FrontendEntititesSettings != null)
            {
                EntitiesCodeWriter.Generate(codeGeneratorSettings, codeGenerators, sourceProductionContext);
            }

            if (codeGeneratorSettings.BackendCustomRequestsSettings != null || codeGeneratorSettings.FrontendCustomRequestsSettings != null)
            {
                CustomRequestsCodeWriter.Generate(codeGeneratorSettings, codeGenerators, sourceProductionContext);
            }

            if (codeGeneratorSettings.BackendCustomResponsesSettings != null || codeGeneratorSettings.FrontendCustomResponsesSettings != null)
            {
                CustomResponsesCodeWriter.Generate(codeGeneratorSettings, codeGenerators, sourceProductionContext);
            }
        }
    }
}
