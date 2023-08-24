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

	[XmlRoot]
	public class CodeGeneratorSettings
	{
		[XmlElement]
		public BackendEnumsSettings BackendEnumsSettings { get; set; }

		[XmlElement]
		public FrontendEnumsSettings FrontendEnumsSettings { get; set; }

		[XmlElement]
		public BackendEntititesSettings BackendEntititesSettings { get; set; }
		
		[XmlElement]
		public FrontendEntititesSettings FrontendEntititesSettings { get; set; }

		[XmlElement]
		public BackendCustomRequestsSettings BackendCustomRequestsSettings { get; set; }
		
		[XmlElement]
		public FrontendCustomRequestsSettings FrontendCustomRequestsSettings { get; set; }

		[XmlElement]
		public BackendCustomResponsesSettings BackendCustomResponsesSettings { get; set; }
		
		[XmlElement]
		public FrontendCustomResponsesSettings FrontendCustomResponsesSettings { get; set; }
	}

	[XmlRoot]
	public class BackendEnumsSettings
	{
		[XmlAttribute]
		public string EnumsNamespace { get; set; }
	}

	[XmlRoot]
	public class FrontendEnumsSettings
	{
		[XmlAttribute]
		public string EnumsNamespace { get; set; }
	}

	[XmlRoot]
	public class BackendEntititesSettings
	{
		[XmlAttribute]
		public string EntitiesNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }
	}

	[XmlRoot]
	public class FrontendEntititesSettings
	{
		[XmlAttribute]
		public string EntitiesNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }

		[XmlAttribute]
		[DefaultValue(true)]
		public bool GenerateNotifyPropertyChanges { get; set; } = true;

		[XmlAttribute]
		[DefaultValue(true)]
		public bool UseObservableCollection { get; set; } = true;
	}

	[XmlRoot]
	public class BackendCustomRequestsSettings
	{
		[XmlAttribute]
		public string CustomRequestsNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }
	}

	[XmlRoot]
	public class FrontendCustomRequestsSettings
	{
		[XmlAttribute]
		public string CustomRequestsNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }

		[XmlAttribute]
		[DefaultValue(true)]
		public bool GenerateNotifyPropertyChanges { get; set; } = true;

		[XmlAttribute]
		[DefaultValue(true)]
		public bool UseObservableCollection { get; set; } = true;
	}

	[XmlRoot]
	public class BackendCustomResponsesSettings
	{
		[XmlAttribute]
		public string CustomResponsesNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }
	}

	[XmlRoot]
	public class FrontendCustomResponsesSettings
	{
		[XmlAttribute]
		public string CustomResponsesNamespace { get; set; }

		[XmlAttribute]
		public string ValidatorsNamespace { get; set; }

		[XmlAttribute]
		[DefaultValue(true)]
		public bool GenerateNotifyPropertyChanges { get; set; } = true;

		[XmlAttribute]
		[DefaultValue(true)]
		public bool UseObservableCollection { get; set; } = true;
	}
}