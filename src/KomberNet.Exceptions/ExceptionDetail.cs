// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ExceptionDetail
    {
        public ExceptionCode ExceptionCode { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
