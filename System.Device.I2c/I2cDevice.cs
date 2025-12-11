// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Device.I2c
{
    /// <summary>
    /// The communications channel to a device on an I2C bus.
    /// </summary>
    public class I2cDevice : IDisposable
    {
        // this is used as the lock object 
        // a lock is required because multiple threads can access the device
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly I2cConnectionSettings _connectionSettings;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private bool _disposed;

        // speeds up the execution of ReadByte and WriteByte operations
        private readonly byte[] _buffer;

        /// <summary>
        /// The connection settings of a device on an I2C bus. The connection settings are immutable after the device is created
        /// so the object returned will be a clone of the settings object.
        /// </summary>
        public I2cConnectionSettings ConnectionSettings { get => _connectionSettings; } 

        /// <summary>
        /// Reads a byte from the I2C device.
        /// </summary>
        /// <returns>A byte read from the I2C device.</returns>
        public byte ReadByte()
        {
            lock (_syncLock)
            {
                NativeTransmit(default, new Span<byte>(_buffer));

                return _buffer[0];
            }
        }

        /// <summary>
        /// Reads data from the I2C device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the I2C device.
        /// The length of the buffer determines how much data to read from the I2C device.
        /// </param>
        public I2cTransferResult Read(Span<byte> buffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(default, buffer);
            }
        }

        /// <summary>
        /// Writes a byte to the I2C device.
        /// </summary>
        /// <param name="value">The byte to be written to the I2C device.</param>
        public I2cTransferResult WriteByte(byte value)
        {
            lock (_syncLock)
            {
                _buffer[0] = value;

                return NativeTransmit(
                    new ReadOnlySpan<byte>(_buffer),
                    default);
            }
        }

        /// <summary>
        /// Writes data to the I2C device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that contains the data to be written to the I2C device.
        /// The data should not include the I2C device address.
        /// </param>
        public I2cTransferResult Write(ReadOnlySpan<byte> buffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(buffer, default);
            }
        }

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the I2C bus on which the device is connected,
        /// and sends a restart condition between the write and read operations.
        /// </summary>
        /// <param name="writeBuffer">
        /// The buffer that contains the data to be written to the I2C device.
        /// The data should not include the I2C device address.</param>
        /// <param name="readBuffer">
        /// The buffer to read the data from the I2C device.
        /// The length of the buffer determines how much data to read from the I2C device.
        /// </param>
        public I2cTransferResult WriteRead(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(writeBuffer, readBuffer);
            }
        }

        /// <summary>
        /// Creates a communications channel to a device on an I2C bus running on the current platform
        /// </summary>
        /// <param name="settings">The connection settings of a device on an I2C bus.</param>
        /// <returns>A communications channel to a device on an I2C bus</returns>
        public static I2cDevice Create(I2cConnectionSettings settings)
        {
            return new I2cDevice(settings);
        }

        /// <summary>
        /// Create an I2C Device
        /// </summary>
        /// <param name="settings">Connection settings</param>
        public I2cDevice(I2cConnectionSettings settings)
        {
            _connectionSettings = settings;

            // create the buffer
            _buffer = new byte[1];

            // create the lock object
            _syncLock = new object();

            // call native init to allow HAL/PAL inits related with I2C hardware
            NativeInit();
        }

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                NativeDispose();

                _disposed = true;
            }
        }

#pragma warning disable 1591
        ~I2cDevice()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                if (!_disposed)
                {
                    Dispose(true);

                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeInit();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDispose();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern I2cTransferResult NativeTransmit(
            ReadOnlySpan<byte> writeBuffer,
            Span<byte> readBuffer);

        #endregion
    }
}