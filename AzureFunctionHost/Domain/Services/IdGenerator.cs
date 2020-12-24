using System;
using System.Security.Cryptography;

namespace AzureFunctionHost.Domain
{
    /// <summary>
    /// Generates a cryptographically secure identifier.
    /// </summary>
    public sealed class IdGenerator
    {
        /// <summary>
        /// Gets the next identifier.
        /// </summary>
        /// <returns>Returns the next GUID.</returns>
        public Guid NextId()
        {
            Span<byte> bytes = stackalloc byte[16];
            RandomNumberGenerator.Fill(bytes);
            return new Guid(bytes);
        }
    }
}
