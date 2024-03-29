﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Resources;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Localization;

    public partial class ActionButtonsControl : ComponentBase
    {
        [Parameter]
        public List<ActionButton> ActionButtons { get; set; } = new List<ActionButton>();

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }
    }
}
