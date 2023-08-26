﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.
// <autogenerated>

namespace KomberNet.Models.CodeGenerator.Structure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Xml.Serialization;

    #region Root
    [XmlRoot]
    public class CodeGenerator
    {
        [XmlElement]
        public List<Entity> Entity { get; set; } = new List<Entity>();

        [XmlElement]
        public List<Summary> Summary { get; set; } = new List<Summary>();

        [XmlElement]
        public List<EnumEntity> Enum { get; set; } = new List<EnumEntity>();

        [XmlElement]
        public List<CustomRequest> CustomRequest { get; set; } = new List<CustomRequest>();

        [XmlElement]
        public List<CustomResponse> CustomResponse { get; set; } = new List<CustomResponse>();
    }
    #endregion Root

    #region Entities
    public enum Location
    {
        Both = 0,
        Backend = 1,
        Frontend = 2,
    }

    [XmlRoot]
    public class Entity
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string PluralName { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlElement]
        public Fields EntityFields { get; set; }

        [XmlAttribute]
        [DefaultValue(true)]
        public bool IncludeAuditLog { get; set; } = true;

        [XmlAttribute]
        [DefaultValue(true)]
        public bool IncludeRowVersionControl { get; set; } = true;

        [XmlAttribute]
        [DefaultValue(true)]
        public bool IncludeDataState { get; set; } = true;

        [XmlElement]
        public GenerateEntityHandlerRequest GenerateEntityHandlerRequest { get; set; }

        [XmlElement]
        public GenerateEntityQueryRequest GenerateEntityQueryRequest { get; set; }

        [XmlElement]
        public GenerateEntitiesQueryRequest GenerateEntitiesQueryRequest { get; set; }
    }

    [XmlRoot]
    public class Summary
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string PluralName { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlElement]
        public Fields SummaryFields { get; set; }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IncludeAuditLog { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IncludeRowVersionControl { get; set; } = false;

        [XmlElement]
        public GenerateSummaryQueryRequest GenerateSummaryQueryRequest { get; set; }

        [XmlElement]
        public GenerateSummariesQueryRequest GenerateSummariesQueryRequest { get; set; }
    }

    [XmlRoot]
    public class CustomRequest
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlElement]
        public Fields RequestFields { get; set; }
    }

    [XmlRoot]
    public class CustomResponse
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlElement]
        public Fields ResponseFields { get; set; }
    }
    
    [XmlRoot]
    public class GenerateEntityHandlerRequest
    {
        [XmlElement]
        public Fields AdditionalFields { get; set; }
    }

    [XmlRoot]
    public class GenerateEntityQueryRequest
    {
        [XmlElement]
        public Fields AdditionalFields { get; set; }
    }

    [XmlRoot]
    public class GenerateEntitiesQueryRequest
    {
        [XmlElement]
        public Fields RequestFields { get; set; }
    }

    [XmlRoot]
    public class GenerateSummaryQueryRequest
    {
        [XmlElement]
        public Fields AdditionalFields { get; set; }
    }

    [XmlRoot]
    public class GenerateSummariesQueryRequest
    {
        [XmlElement]
        public Fields RequestFields { get; set; }

        [XmlAttribute]
        [DefaultValue(true)]
        public bool GenerateSearchableDefaultCriteria { get; set; } = true;
    }
    #endregion Entities

    #region Fields
    public interface IField
    {
        string Name { get; set; }

        Location Location { get; set; }
    }

    public interface ICanBeRequired
    {
        bool IsRequired { get; set; }
    }

    public interface INativeField : IField, ICanBeRequired
    {
    }

    public interface ITypedField : IField, ICanBeRequired
    {
        string Type { get; set; }
    }

    [XmlRoot]
    public class Fields
    {
        [XmlElement]
        public KeyField KeyField { get; set; }

        [XmlElement]
        public List<GuidField> GuidField { get; set; } = new List<GuidField>();

        [XmlElement]
        public List<StringField> StringField { get; set; } = new List<StringField>();

        [XmlElement]
        public List<BoolField> BoolField { get; set; } = new List<BoolField>();

        [XmlElement]
        public List<DateTimeField> DateTimeField { get; set; } = new List<DateTimeField>();

        [XmlElement]
        public List<DateTimeOffsetField> DateTimeOffsetField { get; set; } = new List<DateTimeOffsetField>();

        [XmlElement]
        public List<DecimalField> DecimalField { get; set; } = new List<DecimalField>();

        [XmlElement]
        public List<IntField> IntField { get; set; } = new List<IntField>();

        [XmlElement]
        public List<CollectionField> CollectionField { get; set; } = new List<CollectionField>();

        [XmlElement]
        public List<EntityCollectionField> EntityCollectionField { get; set; } = new List<EntityCollectionField>();

        [XmlElement]
        public List<SummaryField> SummaryField { get; set; } = new List<SummaryField>();

        [XmlElement]
        public List<EnumField> EnumField { get; set; } = new List<EnumField>();

        [XmlElement]
        public List<EntityField> EntityField { get; set; } = new List<EntityField>();
    }

    [XmlRoot]
    public class KeyField : IField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;
    }

    [XmlRoot]
    public class GuidField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;
    }

    [XmlRoot]
    public class StringField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        [DefaultValue("")]
        public string DefaultValue { get; set; } = string.Empty;

        [XmlAttribute]
        [DefaultValue(0)]
        public int MaxLength { get; set; }
    }

    [XmlRoot]
    public class BoolField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool DefaultValue { get; set; } = false;
    }

    [XmlRoot]
    public class DateTimeField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;
    }

    [XmlRoot]
    public class DateTimeOffsetField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;
    }

    [XmlRoot]
    public class DecimalField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(typeof(decimal), "0")]
        public decimal DefaultValue { get; set; } = 0;

        [XmlAttribute]
        public int Precision { get; set; }

        [XmlAttribute]
        public int Scale { get; set; }
    }

    [XmlRoot]
    public class IntField : INativeField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(0)]
        public int DefaultValue { get; set; } = 0;
    }

    [XmlRoot]
    public class CollectionField : ITypedField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        public string Type { get; set; }
    }

    [XmlRoot]
    public class EntityCollectionField : ITypedField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        public string Type { get; set; }
    }

    [XmlRoot]
    public class SummaryField : ITypedField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        [DefaultValue("")]
        public string IsSummaryFor { get; set; } = string.Empty;
    }

    [XmlRoot]
    public class EnumField : ITypedField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        [DefaultValue("")]
        public string IsEnumFor { get; set; } = string.Empty;
    }

    [XmlRoot]
    public class EntityField : ITypedField
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsRequired { get; set; } = false;

        [XmlAttribute]
        public string Type { get; set; }
    }
    #endregion Fields

    #region Enums
    [XmlRoot]
    public class EnumEntity
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlElement]
        public List<EnumValue> EnumValue { get; set; } = new List<EnumValue>();

        [XmlAttribute]
        [DefaultValue(false)]
        public bool AutoGenSequenceNumber { get; set; } = false;
    }

    [XmlRoot]
    public class EnumValue
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(Location.Both)]
        public Location Location { get; set; } = Location.Both;

        [XmlAttribute]
        [DefaultValue("")]
        public string DefaultValue { get; set; } = string.Empty;
    }
    #endregion Enums
}
