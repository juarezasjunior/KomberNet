// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Contracts;
    using Microsoft.AspNetCore.Identity;

    [Table("Users")]
    public class TbUser : IdentityUser<Guid>
    {
        [MaxLength(500)]
        public string FullName { get; set; }
    }
}
