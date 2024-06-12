// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories.Entities.Organization
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using KomberNet.Models.Contracts;

    [Table("OrganizationGroups")]
    public class TbOrganizationGroup : IDatabaseEntity, IHasKey, IHasAuditLog, IHasRowVersionControl
    {
        [Key]
        [Required]
        public Guid OrgazationGroupId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

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

        public virtual ICollection<TbOrganizationGroupUser> OrganizationGroupUsers { get; set; }
    }
}