using System;
using System.Threading;

namespace Tests
{
    public static class UniqueValueHelper
    {
        private static int counter;

        public static string Name() => Guid.NewGuid().ToString();

        public static int Number() => Interlocked.Increment(ref counter);
    }
}
