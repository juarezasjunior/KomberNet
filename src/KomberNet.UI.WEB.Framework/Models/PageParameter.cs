// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PageParameter
    {
        private readonly PageParameterType pageParameterType;
        private readonly string parameterValue;
        private readonly string parameterName;
        private readonly PageParameterValueType parameterValueType;

        public PageParameter(string parameterName, DateTime parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.DateTime, pageParameterType)
        {
        }

        public PageParameter(string parameterName, decimal parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Decimal, pageParameterType)
        {
        }

        public PageParameter(string parameterName, double parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Double, pageParameterType)
        {
        }

        public PageParameter(string parameterName, float parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Float, pageParameterType)
        {
        }

        public PageParameter(string parameterName, Guid parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Guid, pageParameterType)
        {
        }

        public PageParameter(string parameterName, int parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Int, pageParameterType)
        {
        }

        public PageParameter(string parameterName, long parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue.ToString(), PageParameterValueType.Long, pageParameterType)
        {
        }

        public PageParameter(string parameterName, string parameterValue, PageParameterType pageParameterType = PageParameterType.Route)
            : this(parameterName, parameterValue, PageParameterValueType.String, pageParameterType)
        {
        }

        private PageParameter(string parameterName, string parameterValue, PageParameterValueType parameterValueType, PageParameterType pageParameterType)
        {
            this.parameterName = parameterName;
            this.pageParameterType = pageParameterType;
            this.parameterValue = parameterValue;
            this.parameterValueType = parameterValueType;
        }

        public PageParameterType PageParameterType => this.pageParameterType;

        public string GetValue()
        {
            return this.pageParameterType switch
            {
                PageParameterType.Route => Uri.EscapeDataString(this.parameterValue),
                PageParameterType.Query => $"{this.parameterName}={Uri.EscapeDataString(this.parameterValue)}",
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
