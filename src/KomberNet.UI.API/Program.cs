// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

using KomberNet.UI.API.Bootstraps;

var builder = WebApplication.CreateBuilder(args);

await APIBootstrap.ConfigureAPIAsync(builder);