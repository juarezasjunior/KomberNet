// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.CodeGenerator.CodeWriters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using KomberNet.Models.CodeGenerator.Extensions;
    using KomberNet.Models.CodeGenerator.Structure;
    using KomberNet.Models.CodeGenerator.Writers;
    using Microsoft.CodeAnalysis;

    internal static class CustomRequestsCodeWriter
    {
        public static void Generate(CodeGeneratorSettings codeGeneratorSettings, List<CodeGenerator> codeGenerators, SourceProductionContext sourceProductionContext)
        {
            foreach (var codeGenerator in codeGenerators)
            {
                foreach (var customRequest in codeGenerator.CustomRequest)
                {
                    if (codeGeneratorSettings.BackendCustomRequestsSettings != null
                        && (customRequest.Location == Structure.Location.Both || customRequest.Location == Structure.Location.Backend))
                    {
                        WriteRequest(codeGeneratorSettings, sourceProductionContext, customRequest, true);
                    }

                    if (codeGeneratorSettings.FrontendCustomRequestsSettings != null
                        && (customRequest.Location == Structure.Location.Both || customRequest.Location == Structure.Location.Frontend))
                    {
                        WriteRequest(codeGeneratorSettings, sourceProductionContext, customRequest, false);
                    }
                }
            }
        }

        private static void WriteRequest(CodeGeneratorSettings codeGeneratorSettings, SourceProductionContext sourceProductionContext, CustomRequest customRequest, bool isBackend)
        {
            var className = customRequest.Name;
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var keyField = customRequest.RequestFields?.KeyField;
            var inheritance = "IEndpointRequest";
            var classNamespace = customRequest.Namespace;
            var validatorNamespace = customRequest.Namespace;
            var shouldGenerateNotifyPropertyChanges = isBackend ? false : codeGeneratorSettings.FrontendCustomRequestsSettings?.GenerateNotifyPropertyChanges ?? false;
            var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendCustomRequestsSettings?.UseObservableCollection ?? false;

            if (keyField != null)
            {
                inheritance += ", IHasKey";
            }

            var fileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    classNamespace,
                    className,
                    isPartial: true,
                    inheritance: inheritance);

            fileWriter.WriteUsing("System");
            fileWriter.WriteUsing("KomberNet.Models");
            fileWriter.WriteUsing("KomberNet.Models.Contracts");

            var validatorClassName = $"{className}Validator";
            var validatorFileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    validatorNamespace,
                    validatorClassName,
                    isPartial: true,
                    inheritance: $"AbstractValidator<{className}>");

            validatorFileWriter.WriteUsing("System");
            validatorFileWriter.WriteUsing("FluentValidation");
            validatorFileWriter.WriteUsing("KomberNet.Models");
            validatorFileWriter.WriteUsing(classNamespace);

            validatorFileWriter.WriteMethod("SetCustomRules", isPartial: true);

            customRequest.RequestFields?.HandleFields(EntityFieldCodeWriter.WriteField(fileWriter, shouldGenerateNotifyPropertyChanges, useObservableCollection, currentLocation, validatorFileWriter: validatorFileWriter));

            validatorFileWriter.WriteConstructorAdditionalBodyLine($"this.SetCustomRules();");

            sourceProductionContext.WriteNewCSFile(className, fileWriter);
            sourceProductionContext.WriteNewCSFile(validatorClassName, validatorFileWriter);
        }
    }
}
