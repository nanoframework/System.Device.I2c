// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Device.I2c
{
    /// <summary>
    /// The connection settings of a device on an I2C bus.
    /// </summary>
    public sealed class I2cConnectionSettings
    {

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _deviceAddress;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private I2cBusSpeed _busSpeed;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _busId;

        private I2cConnectionSettings()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="I2cConnectionSettings"/> class.
        /// </summary>
        /// <param name="busId">The bus ID the I2C device is connected to.</param>
        /// <param name="deviceAddress">The bus address of the I2C device.</param>
        public I2cConnectionSettings(int busId, int deviceAddress) : this(busId, deviceAddress, I2cBusSpeed.FastMode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="I2cConnectionSettings"/> class.
        /// </summary>
        /// <param name="busId">The bus ID the I2C device is connected to.</param>
        /// <param name="deviceAddress">The bus address of the I2C device.</param>
        /// <param name="busSpeed">The bus speed of the I2C device.</param>
        public I2cConnectionSettings(int busId, int deviceAddress, I2cBusSpeed busSpeed)
        {
            _busId = busId;
            _deviceAddress = deviceAddress;
            _busSpeed = busSpeed;
        }

        internal I2cConnectionSettings(I2cConnectionSettings other)
        {
            _busId = other.BusId;
            _deviceAddress = other.DeviceAddress;
            _busSpeed = other.BusSpeed;
        }

        /// <summary>
        /// The bus ID the I2C device is connected to.
        /// </summary>
        public int BusId { get => _busId; }

        /// <summary>
        /// The bus address of the I2C device.
        /// </summary>
        public int DeviceAddress { get => _deviceAddress; }

        /// <summary>
        /// The bus speed of the I2C device
        /// </summary>
        public I2cBusSpeed BusSpeed { get => _busSpeed; }
    }
}
