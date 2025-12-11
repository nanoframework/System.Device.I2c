// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Device.I2c
{
    /// <summary>
    /// Provides information about whether the data transfers that the <see cref="I2cDevice.Read"/>, <see cref="I2cDevice.Write"/>, or <see cref="I2cDevice.WriteRead"/> method performed succeeded, and the actual number
    /// of bytes the method transferred.
    /// </summary>
	public struct I2cTransferResult
    {
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly uint _bytesTransferred;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly I2cTransferStatus _status;

        /// <summary>
        /// The actual number of bytes that the operation actually transferred.
        /// </summary>
        public uint BytesTransferred { get => _bytesTransferred; }

        /// <summary>
        /// An enumeration value that indicates if the read or write operation transferred the full number of bytes that the method requested, or the reason
        /// that the full transfer did not succeed. For <see cref="I2cTransferStatus.PartialTransfer"/>, the value indicates whether the data for both the write and the read operations was entirely transferred.
        /// </summary>
        public I2cTransferStatus Status { get => _status; }
    }
}
