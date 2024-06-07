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

    internal static class CustomResponsesCodeWriter
    {
        public static void Generate(CodeGeneratorSettings codeGeneratorSettings, List<CodeGenerator> codeGenerators, SourceProductionContext sourceProductionContext)
        {
            foreach (var codeGenerator in codeGenerators)
            {
                foreach (var customResponse in codeGenerator.CustomResponse)
                {
                    if (codeGeneratorSettings.BackendCustomResponsesSettings != null
                        && (customResponse.Location == Structure.Location.Both || customResponse.Location == Structure.Location.Backend))
                    {
                        WriteResponse(codeGeneratorSettings, sourceProductionContext, customResponse, true);
                    }

                    if (codeGeneratorSettings.FrontendCustomResponsesSettings != null
                        && (customResponse.Location == Structure.Location.Both || customResponse.Location == Structure.Location.Frontend))
                    {
                        WriteResponse(codeGeneratorSettings, sourceProductionContext, customResponse, false);
                    }
                }
            }
        }

        private static void WriteResponse(CodeGeneratorSettings codeGeneratorSettings, SourceProductionContext sourceProductionContext, CustomResponse customResponse, bool isBackend)
        {
            var className = customResponse.Name;
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var keyField = customResponse.ResponseFields?.KeyField;
            var inheritance = "IEndpointResponse";
            var classNamespace = customResponse.Namespace;
            var validatorNamespace = customResponse.Namespace;
            var shouldGenerateNotifyPropertyChanges = isBackend ? false : codeGeneratorSettings.FrontendCustomResponsesSettings?.GenerateNotifyPropertyChanges ?? false;
            var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendCustomResponsesSettings?.UseObservableCollection ?? false;

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

            customResponse.ResponseFields?.HandleFields(EntityFieldCodeWriter.WriteField(fileWriter, className, shouldGenerateNotifyPropertyChanges, useObservableCollection, currentLocation));

            sourceProductionContext.WriteNewCSFile(className, fileWriter);
        }
    }
}
