// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.UI.WEB.APIClient;
    using KomberNet.UI.WEB.APIClient.Auth;
    using NUnit.Framework;

    [TestFixture]
    public class ClientInterfaceScanTests
    {
        [Test]
        public void ShouldScanClientInterfacesTest()
        {
            var allInterfaces = typeof(IAuthClient).Assembly.GetTypes().Where(x =>
                x.IsAssignableTo(typeof(IAPIClient))
                    && x.IsInterface);

            Assert.That(allInterfaces, Is.Not.Null);
        }
    }
}
