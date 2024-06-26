﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IHasAuditLog
    {
        public Guid CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public string UpdatedByUserName { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
