// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;

    public interface ITokenService : IService
    {
        public Task<(string Token, string RefreshToken)> GenerateTokenAsync(TbUser user, CancellationToken cancellationToken);
    }
}
