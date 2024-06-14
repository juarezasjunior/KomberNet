// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.APIClient.Organization
{
    using System.Threading.Tasks;
    using KomberNet.Models.Organization;
    using Refit;

    public partial interface IOrganizationGroupClient : IAuthenticatedAPIClient
    {
        [Get("/api/Organization/OrganizationGroup/Get")]
        public Task<OrganizationGroupGetResponse> GetAsync([Query] OrganizationGroupGetRequest request);

        [Post("/api/Organization/OrganizationGroup/Post")]
        public Task<OrganizationGroupHandlerResponse> PostAsync([Body] OrganizationGroupHandlerRequest request);
    }
}
