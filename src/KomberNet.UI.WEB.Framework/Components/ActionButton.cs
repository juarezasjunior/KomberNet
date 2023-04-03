// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using KomberNet.Resources;
    using Microsoft.Extensions.Localization;

    public class ActionButton
    {
        public ActionButton()
        {
        }

        public ActionButton(string text, int sequence, Func<bool> canExecute, Action onExecute)
        {
            this.Text = text;
            this.Sequence = sequence;
            this.CanExecute = canExecute;
            this.OnExecute = onExecute;
        }

        public string Text { get; set; }

        public int Sequence { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled { get; set; } = true;

        public Func<bool> CanExecute { get; set; }

        public Action OnExecute { get; set; }

        public static ActionButton NewActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_New"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton AddActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Add"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton CloseActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Close"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton OpenActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Open"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton EditActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Edit"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton SaveActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Save"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton RunActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Run"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };

        public static ActionButton DeleteActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecute) => new ActionButton()
        {
            Text = localizer["Button_Delete"],
            Sequence = 3,
            CanExecute = canExecute,
            OnExecute = onExecute,
        };
    }
}
