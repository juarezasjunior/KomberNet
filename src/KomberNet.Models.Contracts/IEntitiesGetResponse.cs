// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public interface IEntitiesGetResponse<TEntity, TCollectionType> : IEndpointResponse
        where TEntity : class, IEntity
        where TCollectionType : ICollection<TEntity>
    {
        public TCollectionType Entities { get; set; }
    }
}
