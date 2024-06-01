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

    internal static class EntityFieldCodeWriter
    {
        public static Action<object> WriteField(CSFileWriter fileWriter, bool hasNotifyPropertyChanged, bool useObservableCollection, Location location, CSFileWriter validatorFileWriter = null)
        {
            return x =>
            {
                if (x is IField field)
                {
                    if (field.Location != Location.Both && field.Location != location)
                    {
                        return;
                    }

                    var fieldType = field.GetFieldType();
                    var isList = field is EntityCollectionField || field is CollectionField;
                    var fieldValue = string.Empty;
                    var isObservableCollection = false;

                    if (isList)
                    {
                        if (useObservableCollection)
                        {
                            fieldType = $"ObservableCollection<{fieldType}>";
                            fieldValue = $"new ObservableCollection<{fieldType}>()";
                            isObservableCollection = true;
                        }
                        else
                        {
                            fieldType = $"IList<{fieldType}>";
                            fieldValue = $"new List<{fieldType}>()";
                        }
                    }

                    var isRequired = false;

                    if (field is ICanBeRequired requiredField)
                    {
                        isRequired = requiredField.IsRequired;
                    }

                    var maxLength = 0;

                    if (field is StringField stringField)
                    {
                        maxLength = stringField.MaxLength;
                    }

                    if (isRequired)
                    {
                        validatorFileWriter?.WriteConstructorAdditionalBodyLine($"this.RuleFor(x => x.{field.Name}).NotNull().NotEmpty();");
                    }

                    if (maxLength > 0)
                    {
                        validatorFileWriter?.WriteConstructorAdditionalBodyLine($"this.RuleFor(x => x.{field.Name}).MaximumLength({maxLength});");
                    }

                    if (field is EntityField entityField)
                    {
                        validatorFileWriter?.WriteConstructorAdditionalBodyLine($"this.RuleFor(x => x.{field.Name}).SetValidator(x => new {entityField.Type}Validator());");
                    }

                    fileWriter.WriteProperty(fieldType, field.Name, value: fieldValue, hasNotifyPropertyChanged: hasNotifyPropertyChanged, isObservableCollection: isObservableCollection);
                }
            };
        }
    }
}
