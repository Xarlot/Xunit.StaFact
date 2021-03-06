// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the Ms-PL license. See LICENSE.txt file in the project root for full license information.

#if NETFRAMEWORK || NETCOREAPP

namespace Xunit
{
    using System;
    using System.Windows.Threading;
    using Xunit.Sdk;

    /// <summary>
    /// Identifies an xunit test that starts on an STA thread
    /// with a WinUI <see cref="Microsoft.UI.Threading.DispatcherQueueSyncContext" />.
    /// Tests will be Skipped on non-Windows operating systems.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Xunit.Sdk.WinUIFactDiscoverer", ThisAssembly.AssemblyName)]
    public class WinUIFactAttribute : FactAttribute
    {
    }
}

#endif
