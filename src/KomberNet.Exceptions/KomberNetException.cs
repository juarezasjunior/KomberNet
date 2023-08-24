// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Exceptions
{
    using System;

    public class KomberNetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KomberNetException"/> class.
        /// </summary>
        /// <param name="exceptionCode">Error code used by KomberNet to register some specific internal exceptions.</param>
        /// <param name="additionalInfo">Add any other information that you want to.</param>
        public KomberNetException(ExceptionCode exceptionCode = ExceptionCode.Others, string additionalInfo = null)
        {
            this.ExceptionCode = exceptionCode;
            this.AdditionalInfo = additionalInfo;
        }

        public ExceptionCode ExceptionCode { get; }

        public string AdditionalInfo { get; }
    }
}
