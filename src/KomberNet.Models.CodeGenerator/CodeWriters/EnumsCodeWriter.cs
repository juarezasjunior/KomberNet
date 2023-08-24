// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.CodeGenerator.CodeWriters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using KomberNet.Models.CodeGenerator.Extensions;
    using KomberNet.Models.CodeGenerator.Structure;
    using KomberNet.Models.CodeGenerator.Writers;
    using Microsoft.CodeAnalysis;

    internal static class EnumsCodeWriter
    {
        public static void Generate(CodeGeneratorSettings codeGeneratorSettings, List<CodeGenerator> codeGenerators, SourceProductionContext sourceProductionContext)
        {
            foreach (var codeGenerator in codeGenerators)
            {
                foreach (var enumEntity in codeGenerator.Enum)
                {
                    if (codeGeneratorSettings.BackendEnumsSettings != null
                        && (enumEntity.Location == Structure.Location.Both || enumEntity.Location == Structure.Location.Backend))
                    {
                        WriteEnum(codeGeneratorSettings, sourceProductionContext, enumEntity, true);
                    }

                    if (codeGeneratorSettings.FrontendEnumsSettings != null
                        && (enumEntity.Location == Structure.Location.Both || enumEntity.Location == Structure.Location.Frontend))
                    {
                        WriteEnum(codeGeneratorSettings, sourceProductionContext, enumEntity, false);
                    }
                }
            }
        }

        private static void WriteEnum(CodeGeneratorSettings codeGeneratorSettings, SourceProductionContext sourceProductionContext, EnumEntity enumEntity, bool isBackend)
        {
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var enumEntityNamespace = isBackend ? codeGeneratorSettings.BackendEnumsSettings?.EnumsNamespace : codeGeneratorSettings.FrontendEnumsSettings?.EnumsNamespace;

            var enumEntityFileWriter = new CSFileWriter(
                    CSFileWriterType.Enum,
                    enumEntityNamespace,
                    enumEntity.Name);

            enumEntityFileWriter.WriteUsing("System");

            var sequence = 0;

            foreach (var enumValue in enumEntity.EnumValue)
            {
                if (enumValue.Location != Structure.Location.Both && enumValue.Location != currentLocation)
                {
                    return;
                }

                var defaultValue = enumValue.DefaultValue;

                if (enumEntity.AutoGenSequenceNumber)
                {
                    enumEntityFileWriter.WriteEnumField(enumValue.Name, sequence.ToString());
                    sequence++;
                }
                else
                {
                    enumEntityFileWriter.WriteEnumField(enumValue.Name, enumValue.DefaultValue);
                }
            }

            sourceProductionContext.WriteNewCSFile(enumEntity.Name, enumEntityFileWriter);
        }
    }
}
