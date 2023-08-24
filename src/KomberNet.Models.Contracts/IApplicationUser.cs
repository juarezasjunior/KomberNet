// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IApplicationUser
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
