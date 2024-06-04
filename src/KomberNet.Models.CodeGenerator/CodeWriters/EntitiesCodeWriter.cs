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

    internal static class EntitiesCodeWriter
    {
        public static void Generate(CodeGeneratorSettings codeGeneratorSettings, List<CodeGenerator> codeGenerators, SourceProductionContext sourceProductionContext)
        {
            foreach (var codeGenerator in codeGenerators)
            {
                foreach (var entity in codeGenerator.Entity)
                {
                    if (codeGeneratorSettings.BackendEntititesSettings != null
                        && (entity.Location == Structure.Location.Both || entity.Location == Structure.Location.Backend))
                    {
                        GenerateEntities(codeGeneratorSettings, sourceProductionContext, entity, true);
                    }

                    if (codeGeneratorSettings.FrontendEntititesSettings != null
                        && (entity.Location == Structure.Location.Both || entity.Location == Structure.Location.Frontend))
                    {
                        GenerateEntities(codeGeneratorSettings, sourceProductionContext, entity, false);
                    }
                }

                foreach (var summary in codeGenerator.Summary)
                {
                    if (codeGeneratorSettings.BackendEntititesSettings != null
                        && (summary.Location == Structure.Location.Both || summary.Location == Structure.Location.Backend))
                    {
                        GenerateSummaries(codeGeneratorSettings, sourceProductionContext, summary, true);
                    }

                    if (codeGeneratorSettings.FrontendEntititesSettings != null
                        && (summary.Location == Structure.Location.Both || summary.Location == Structure.Location.Frontend))
                    {
                        GenerateSummaries(codeGeneratorSettings, sourceProductionContext, summary, false);
                    }
                }
            }
        }

        private static void GenerateEntities(CodeGeneratorSettings codeGeneratorSettings, SourceProductionContext sourceProductionContext, Entity entity, bool isBackend)
        {
            WriteEntity(
                codeGeneratorSettings,
                sourceProductionContext,
                entityNamespace: entity.Namespace,
                entityName: entity.Name,
                defaultInheritance: "IEntity",
                fields: entity.EntityFields,
                includeDataState: entity.IncludeDataState,
                includeRowVersionControl: entity.IncludeRowVersionControl,
                includeAuditLog: entity.IncludeAuditLog,
                isBackend: isBackend);

            if (entity.GenerateEntityHandlerRequest != null)
            {
                WriteRequest(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.Name}Handler",
                    inheritance: $"IEntityHandlerRequest<{entity.Name}>",
                    fields: entity.GenerateEntityHandlerRequest.AdditionalFields,
                    isBackend: isBackend,
                    entityPropertyType: entity.Name,
                    entityPropertyName: "Entity",
                    entityPropertyHasValidator: true);

                var responseInheritance = $"IEntityHandlerResponse";
                Fields responseFields = null;

                if (entity.EntityFields?.KeyField != null)
                {
                    responseFields = new Fields
                    {
                        KeyField = entity.EntityFields?.KeyField,
                    };

                    responseInheritance += ", IHasKey";
                }

                WriteResponse(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.Name}Handler",
                    inheritance: responseInheritance,
                    fields: responseFields,
                    isBackend: isBackend);
            }

            if (entity.GenerateEntityGetRequest != null)
            {
                var entityGetRequestInheritance = "IEntityGetRequest";
                var entityGetRequestFields = entity.GenerateEntityGetRequest.AdditionalFields;

                if (entity.EntityFields?.KeyField != null)
                {
                    entityGetRequestFields = entityGetRequestFields ?? new Fields();

                    entityGetRequestFields.KeyField = entity.EntityFields?.KeyField;

                    entityGetRequestInheritance += ", IHasKey";
                }

                WriteRequest(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.Name}Get",
                    inheritance: entityGetRequestInheritance,
                    fields: entityGetRequestFields,
                    isBackend: isBackend);

                WriteResponse(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.Name}Get",
                    inheritance: $"IEntityGetResponse<{entity.Name}>",
                    fields: null,
                    isBackend: isBackend,
                    entityPropertyType: entity.Name,
                    entityPropertyName: "Entity");
            }

            if (entity.GenerateEntitiesGetRequest != null)
            {
                WriteRequest(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.PluralName}Get",
                    inheritance: "IEntitiesGetRequest",
                    fields: entity.GenerateEntitiesGetRequest.RequestFields,
                    isBackend: isBackend);

                var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.UseObservableCollection ?? false;
                var collectionType = useObservableCollection ? "ObservableCollection" : "IList";
                var collectionTypeInstantiateCommand = useObservableCollection ? "new ObservableCollection" : "new List";

                WriteResponse(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: entity.Namespace,
                    classPrefix: $"{entity.PluralName}Get",
                    inheritance: $"IEntitiesGetResponse<{entity.Name}, {collectionType}<{entity.Name}>>",
                    fields: null,
                    isBackend: isBackend,
                    entityPropertyType: $"{collectionType}<{entity.Name}>",
                    entityPropertyName: "Entities",
                    entityPropertyValue: $"{collectionTypeInstantiateCommand}<{entity.Name}>()",
                    entityPropertyIsObservableCollection: useObservableCollection);
            }
        }

        private static void GenerateSummaries(CodeGeneratorSettings codeGeneratorSettings, SourceProductionContext sourceProductionContext, Summary summary, bool isBackend)
        {
            WriteEntity(
                codeGeneratorSettings,
                sourceProductionContext,
                entityNamespace: summary.Namespace,
                entityName: summary.Name,
                defaultInheritance: "ISummary",
                fields: summary.SummaryFields,
                includeDataState: false,
                includeRowVersionControl: summary.IncludeRowVersionControl,
                includeAuditLog: summary.IncludeAuditLog,
                isBackend: isBackend);

            if (summary.GenerateSummaryGetRequest != null)
            {
                var summaryGetRequestInheritance = "ISummaryGetRequest";
                var summaryGetRequestFields = summary.GenerateSummaryGetRequest.AdditionalFields;

                if (summary.SummaryFields?.KeyField != null)
                {
                    summaryGetRequestFields = summaryGetRequestFields ?? new Fields();

                    summaryGetRequestFields.KeyField = summary.SummaryFields?.KeyField;

                    summaryGetRequestInheritance += ", IHasKey";
                }

                WriteRequest(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: summary.Namespace,
                    classPrefix: $"{summary.Name}Get",
                    inheritance: summaryGetRequestInheritance,
                    fields: summaryGetRequestFields,
                    isBackend: isBackend);

                WriteResponse(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: summary.Namespace,
                    classPrefix: $"{summary.Name}Get",
                    inheritance: $"ISummaryGetResponse<{summary.Name}>",
                    fields: null,
                    isBackend: isBackend,
                    entityPropertyType: summary.Name,
                    entityPropertyName: "Summary");
            }

            if (summary.GenerateSummariesGetRequest != null)
            {
                var requestInheritance = "ISummariesGetRequest";

                var requestFields = summary.GenerateSummariesGetRequest.RequestFields;

                if (summary.GenerateSummariesGetRequest.GenerateSearchableDefaultCriteria)
                {
                    requestInheritance += ", IHasSearchableDefaultCriteria";

                    if (requestFields == null)
                    {
                        requestFields = new Fields();
                    }

                    requestFields.StringField.Add(new StringField() { Name = "Search" });
                    requestFields.IntField.Add(new IntField() { Name = "Take" });
                }

                WriteRequest(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: summary.Namespace,
                    classPrefix: $"{summary.PluralName}Get",
                    inheritance: requestInheritance,
                    fields: requestFields,
                    isBackend: isBackend);

                var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.UseObservableCollection ?? false;
                var collectionType = useObservableCollection ? "ObservableCollection" : "IList";
                var collectionTypeInstantiateCommand = useObservableCollection ? "new ObservableCollection" : "new List";

                WriteResponse(
                    codeGeneratorSettings,
                    sourceProductionContext,
                    classNamespace: summary.Namespace,
                    classPrefix: $"{summary.PluralName}Get",
                    inheritance: $"ISummariesGetResponse<{summary.Name}, {collectionType}<{summary.Name}>>",
                    fields: null,
                    isBackend: isBackend,
                    entityPropertyType: $"{collectionType}<{summary.Name}>",
                    entityPropertyName: "Summaries",
                    entityPropertyValue: $"{collectionTypeInstantiateCommand}<{summary.Name}>()",
                    entityPropertyIsObservableCollection: useObservableCollection);
            }
        }

        private static void WriteEntity(
            CodeGeneratorSettings codeGeneratorSettings,
            SourceProductionContext sourceProductionContext,
            string entityNamespace,
            string entityName,
            string defaultInheritance,
            Fields fields,
            bool includeDataState,
            bool includeRowVersionControl,
            bool includeAuditLog,
            bool isBackend)
        {
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var keyField = fields?.KeyField;
            var entityValidatorNamespace = entityNamespace;
            var shouldGenerateNotifyPropertyChanges = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.GenerateNotifyPropertyChanges ?? false;
            var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.UseObservableCollection ?? false;

            var inheritance = defaultInheritance;

            if (includeDataState)
            {
                inheritance += ", IHasDataState";
            }

            if (keyField != null)
            {
                inheritance += ", IHasKey";
            }

            if (includeRowVersionControl)
            {
                inheritance += ", IHasRowVersionControl";
            }

            if (includeAuditLog)
            {
                inheritance += ", IHasAuditLog";
            }

            var entityFileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    entityNamespace,
                    entityName,
                    isPartial: true,
                    inheritance: inheritance);

            entityFileWriter.WriteUsing("System");
            entityFileWriter.WriteUsing("KomberNet.Models");
            entityFileWriter.WriteUsing("KomberNet.Models.Contracts");

            var validatorClassName = $"{entityName}Validator";
            var entityValidatorFileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    entityValidatorNamespace,
                    validatorClassName,
                    isPartial: true,
                    inheritance: $"AbstractValidator<{entityName}>");

            entityValidatorFileWriter.WriteUsing("System");
            entityValidatorFileWriter.WriteUsing("FluentValidation");
            entityValidatorFileWriter.WriteUsing("KomberNet.Models.Contracts");
            entityValidatorFileWriter.WriteUsing("KomberNet.Resources");
            entityValidatorFileWriter.WriteUsing(entityNamespace);

            entityValidatorFileWriter.WriteMethod("SetCustomRules", isPartial: true);

            fields?.HandleFields(EntityFieldCodeWriter.WriteField(entityFileWriter, entityName, shouldGenerateNotifyPropertyChanges, useObservableCollection, currentLocation, validatorFileWriter: entityValidatorFileWriter));

            if (includeRowVersionControl)
            {
                entityFileWriter.WriteProperty("byte[]", "RowVersion", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
            }

            if (includeAuditLog)
            {
                entityFileWriter.WriteProperty("Guid", "CreatedByUserId", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
                entityFileWriter.WriteProperty("string", "CreatedByUserName", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
                entityFileWriter.WriteProperty("DateTimeOffset", "CreatedAt", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);

                entityFileWriter.WriteProperty("Guid", "UpdatedByUserId", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
                entityFileWriter.WriteProperty("string", "UpdatedByUserName", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
                entityFileWriter.WriteProperty("DateTimeOffset?", "UpdatedAt", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
            }

            if (includeDataState)
            {
                entityFileWriter.WriteProperty("DataState", "DataState", hasNotifyPropertyChanged: shouldGenerateNotifyPropertyChanges, isVirtual: false);
            }

            entityValidatorFileWriter.WriteConstructorAdditionalBodyLine($"this.SetCustomRules();");

            sourceProductionContext.WriteNewCSFile(entityName, entityFileWriter);
            sourceProductionContext.WriteNewCSFile(validatorClassName, entityValidatorFileWriter);
        }

        private static void WriteRequest(
            CodeGeneratorSettings codeGeneratorSettings,
            SourceProductionContext sourceProductionContext,
            string classNamespace,
            string classPrefix,
            string inheritance,
            Fields fields,
            bool isBackend,
            string entityPropertyType = null,
            string entityPropertyName = null,
            string entityPropertyValue = null,
            bool entityPropertyHasValidator = false,
            bool entityPropertyIsObservableCollection = false)
        {
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var validatorNamespace = classNamespace;
            var shouldGenerateNotifyPropertyChanges = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.GenerateNotifyPropertyChanges ?? false;
            var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.UseObservableCollection ?? false;

            var className = classPrefix + "Request";

            var fileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    classNamespace,
                    className,
                    isPartial: true,
                    inheritance: inheritance);

            fileWriter.WriteUsing("System");
            fileWriter.WriteUsing("KomberNet.Models");
            fileWriter.WriteUsing("KomberNet.Models.Contracts");

            if (entityPropertyIsObservableCollection)
            {
                fileWriter.WriteUsing("System.Collections.ObjectModel");
            }

            var validatorClassName = $"{className}Validator";
            var validatorFileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    validatorNamespace,
                    validatorClassName,
                    isPartial: true,
                    inheritance: $"AbstractValidator<{className}>");

            validatorFileWriter.WriteUsing("System");
            validatorFileWriter.WriteUsing("FluentValidation");
            validatorFileWriter.WriteUsing("KomberNet.Models.Contracts");
            validatorFileWriter.WriteUsing("KomberNet.Resources");
            validatorFileWriter.WriteUsing(classNamespace);

            validatorFileWriter.WriteMethod("SetCustomRules", isPartial: true);

            if (!string.IsNullOrEmpty(entityPropertyName))
            {
                fileWriter.WriteProperty(
                    type: entityPropertyType,
                    name: entityPropertyName,
                    value: entityPropertyValue,
                    hasNotifyPropertyChanged: entityPropertyIsObservableCollection ? false : shouldGenerateNotifyPropertyChanges,
                    isObservableCollection: entityPropertyIsObservableCollection);

                if (entityPropertyHasValidator)
                {
                    validatorFileWriter.WriteConstructorAdditionalBodyLine($"this.RuleFor(x => x.{entityPropertyName}).NotNull().NotEmpty().WithMessage(Resource.ResourceManager.GetString(\"{className}_{entityPropertyName}_Validation_Required\"));");
                    validatorFileWriter.WriteConstructorAdditionalBodyLine($"this.RuleFor(x => x.{entityPropertyName}).SetValidator(x => new {entityPropertyType}Validator());");
                }
            }

            fields?.HandleFields(EntityFieldCodeWriter.WriteField(fileWriter, className, shouldGenerateNotifyPropertyChanges, useObservableCollection, currentLocation, validatorFileWriter: validatorFileWriter));

            validatorFileWriter.WriteConstructorAdditionalBodyLine($"this.SetCustomRules();");

            sourceProductionContext.WriteNewCSFile(className, fileWriter);
            sourceProductionContext.WriteNewCSFile(validatorClassName, validatorFileWriter);
        }

        private static void WriteResponse(
            CodeGeneratorSettings codeGeneratorSettings,
            SourceProductionContext sourceProductionContext,
            string classNamespace,
            string classPrefix,
            string inheritance,
            Fields fields,
            bool isBackend,
            string entityPropertyType = null,
            string entityPropertyName = null,
            string entityPropertyValue = null,
            bool entityPropertyIsObservableCollection = false)
        {
            var currentLocation = isBackend ? Structure.Location.Backend : Structure.Location.Frontend;
            var validatorNamespace = classNamespace;
            var shouldGenerateNotifyPropertyChanges = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.GenerateNotifyPropertyChanges ?? false;
            var useObservableCollection = isBackend ? false : codeGeneratorSettings.FrontendEntititesSettings?.UseObservableCollection ?? false;

            var className = classPrefix + "Response";

            var fileWriter = new CSFileWriter(
                    CSFileWriterType.Class,
                    classNamespace,
                    className,
                    isPartial: true,
                    inheritance: inheritance);

            fileWriter.WriteUsing("System");
            fileWriter.WriteUsing("KomberNet.Models");
            fileWriter.WriteUsing("KomberNet.Models.Contracts");

            if (entityPropertyIsObservableCollection)
            {
                fileWriter.WriteUsing("System.Collections.ObjectModel");
            }

            if (!string.IsNullOrEmpty(entityPropertyName))
            {
                fileWriter.WriteProperty(
                    type: entityPropertyType,
                    name: entityPropertyName,
                    value: entityPropertyValue,
                    hasNotifyPropertyChanged: entityPropertyIsObservableCollection ? false : shouldGenerateNotifyPropertyChanges,
                    isObservableCollection: entityPropertyIsObservableCollection);
            }

            fields?.HandleFields(EntityFieldCodeWriter.WriteField(fileWriter, className, shouldGenerateNotifyPropertyChanges, useObservableCollection, currentLocation));

            sourceProductionContext.WriteNewCSFile(className, fileWriter);
        }
    }
}
