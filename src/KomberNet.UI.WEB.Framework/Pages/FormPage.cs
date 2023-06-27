// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.UI.WEB.Framework.Components;

    public abstract class FormPage : BasePage
    {
        public List<ActionButton> ActionButtons { get; } = new List<ActionButton>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.CreateActionButtons();
        }

        protected abstract void CreateActionButtons();
    }
}
