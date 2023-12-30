// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;

    public interface ICurrentUserValidatorService : IService
    {
        public Task ValidateAsync(string email, string sessionId, CancellationToken cancellationToken);
    }
}
