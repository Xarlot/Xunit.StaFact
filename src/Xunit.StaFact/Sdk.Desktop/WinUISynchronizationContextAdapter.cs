// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the Ms-PL license. See LICENSE.txt file in the project root for full license information.

namespace Xunit.Sdk
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    public static class WinUISynchronizationContextAdapterHelper
    {
        static SynchronizationContext MainThreadContext { get; set; }
        static Action CleanUpHandler { get; set; }

        public static void Initialize(SynchronizationContext context, Action cleanUp)
        {
            MainThreadContext = context;
            CleanUpHandler = cleanUp;
        }
        internal static SynchronizationContext CreateCopy()
        {
            return MainThreadContext.CreateCopy();
        }
        internal static void CleanUp()
        {
            CleanUpHandler?.Invoke();
        }
    }

    internal class WinUISynchronizationContextAdapter : SyncContextAdapter
    {
        internal static readonly SyncContextAdapter Default = new WinUISynchronizationContextAdapter();

        private WinUISynchronizationContextAdapter()
        {
        }

        internal override bool CanCompleteOperations => false;

        internal override SynchronizationContext Create() => WinUISynchronizationContextAdapterHelper.CreateCopy();

        internal override Task WaitForOperationCompletionAsync(SynchronizationContext syncContext)
        {
            throw new NotSupportedException("Async void test methods are not supported by the WinUI dispatcher queue. Use Async Task instead.");
        }

        internal override void PumpTill(SynchronizationContext synchronizationContext, Task task)
        {
            if (!task.IsCompleted)
            {
                EventWaitHandle waitHandle = new ManualResetEvent(false);
                task.ContinueWith(_ => waitHandle.Set(), TaskScheduler.Default);
                waitHandle.WaitOne();
            }
        }

        internal override void Cleanup()
        {
            WinUISynchronizationContextAdapterHelper.CleanUp();
            base.Cleanup();
        }
    }
}
