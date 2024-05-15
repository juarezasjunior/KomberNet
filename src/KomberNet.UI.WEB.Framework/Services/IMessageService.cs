// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMessageService : IScopedService
    {
        public Task NotifySuccessAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null);

        public Task NotifyWarningAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null);

        public Task NotifyErrorAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null);

        public Task NotifyInfoAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null);

        public Task ShowErrorMessageAsync(string message, string title = null);

        public Task ShowInfoMessageAsync(string message, string title = null);

        public Task<bool> ConfirmAsync(string message, string title = null);
    }
}
