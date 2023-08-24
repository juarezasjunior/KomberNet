﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;

    public interface IApplicationUserHandlerService : IService
    {
        public Task<ApplicationUserInsertResponse> InsertApplicationUserAsync(ApplicationUserInsertRequest request, CancellationToken cancellationToken);
    }
}
