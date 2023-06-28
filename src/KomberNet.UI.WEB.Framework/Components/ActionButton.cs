// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Runtime.CompilerServices;
    using KomberNet.Resources;
    using Microsoft.Extensions.Localization;
    using Radzen;

    public class ActionButton
    {
        private ActionButton()
        {
        }

        public string Text { get; private set; }

        public int Sequence { get; private set; }

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled { get; set; } = true;

        public ButtonType ButtonType { get; private set; } = ButtonType.Button;

        public Func<bool> CanExecute { get; private set; }

        public Action OnExecuting { get; private set; }

        public void TryExecute()
        {
            var canExecute = true;

            if (this.CanExecute != null)
            {
                canExecute = this.CanExecute.Invoke();
            }

            if (canExecute && this.OnExecuting != null)
            {
                this.OnExecuting.Invoke();
            }
        }

        public static ActionButton NewActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => NewActionButton(localizer, null, onExecuting);

        public static ActionButton NewActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_New"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton AddActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => AddActionButton(localizer, null, onExecuting);

        public static ActionButton AddActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_Add"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton CloseActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => CloseActionButton(localizer, null, onExecuting);

        public static ActionButton CloseActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_Close"],
            Sequence = 1,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton OpenActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => OpenActionButton(localizer, null, onExecuting);

        public static ActionButton OpenActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_Open"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton EditActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => EditActionButton(localizer, null, onExecuting);

        public static ActionButton EditActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_Edit"],
            Sequence = 2,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton SaveActionButton(
            IStringLocalizer<Resource> localizer) => SaveActionButton(localizer, null);

        public static ActionButton SaveActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute) => new ActionButton()
        {
            Text = localizer["Button_Save"],
            Sequence = 2,
            ButtonType = ButtonType.Submit,
            CanExecute = canExecute,
        };

        public static ActionButton RunActionButton(
            IStringLocalizer<Resource> localizer) => RunActionButton(localizer, null);

        public static ActionButton RunActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute) => new ActionButton()
        {
            Text = localizer["Button_Run"],
            Sequence = 2,
            ButtonType = ButtonType.Submit,
            CanExecute = canExecute,
        };

        public static ActionButton DeleteActionButton(
            IStringLocalizer<Resource> localizer,
            Action onExecuting) => DeleteActionButton(localizer, null, onExecuting);

        public static ActionButton DeleteActionButton(
            IStringLocalizer<Resource> localizer,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = localizer["Button_Delete"],
            Sequence = 3,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static ActionButton CustomActionButton(
            string text,
            Action onExecuting) => CustomActionButton(text, null, onExecuting);

        public static ActionButton CustomActionButton(
            string text,
            Func<bool> canExecute,
            Action onExecuting) => new ActionButton()
        {
            Text = text,
            Sequence = 4,
            CanExecute = canExecute,
            OnExecuting = onExecuting,
        };

        public static void EnableActionButtons(List<ActionButton> actionButtons)
        {
            foreach (var actionButton in actionButtons)
            {
                if (actionButton.CanExecute != null)
                {
                    actionButton.IsEnabled = actionButton.CanExecute.Invoke();
                }
                else
                {
                    actionButton.IsEnabled = true;
                }
            }
        }
    }
}
