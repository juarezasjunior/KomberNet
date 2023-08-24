// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISummaryQueryResponse<TSummary> : IEndpointResponse
        where TSummary : class, ISummary
    {
        public TSummary Summary { get; set; }
    }
}
