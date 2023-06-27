// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Reactive.Subjects;
    using FluentValidation.Results;
    using KomberNet.Resources;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Localization;

    public partial class FieldControl : ComponentBase
    {
        [Parameter]
        public string FieldName { get; set; }

        [Parameter]
        public string FieldDescription { get; set; }

        [Parameter]
        public bool IsRequired { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private IStringLocalizer<Resource> Localizer { get; set; }

        private string FormattedDescription => this.IsRequired ? "*" + this.FieldDescription : this.FieldDescription;
    }
}
