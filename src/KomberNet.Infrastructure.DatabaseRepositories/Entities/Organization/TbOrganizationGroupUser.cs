// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Organization;
    using KomberNet.Models.Contracts;

    [Table("OrganizationGroupUsers")]
    public class TbOrganizationGroupUser : IDatabaseEntity, IHasKey, IHasAuditLog, IHasRowVersionControl
    {
        [Key]
        [Required]
        public Guid OrgazationGroupUserId { get; set; }

        [Required]
        [ForeignKey("OrganizationGroup")]
        public Guid OrgazationGroupId { get; set; }

        public virtual TbOrganizationGroup OrganizationGroup { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual TbUser User { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; }

        [MaxLength(255)]
        [Required]
        public string CreatedByUserName { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        public Guid UpdatedByUserId { get; set; }

        [MaxLength(255)]
        public string UpdatedByUserName { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        [Timestamp]
        [Required]
        public byte[] RowVersion { get; set; }
    }
}
