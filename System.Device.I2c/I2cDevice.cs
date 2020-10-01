using System;

namespace System.Device.I2c
{
    /// <summary>
    /// The communications channel to a device on an I2C bus.
    /// </summary>
    public class I2cDevice : IDisposable
    {

        private Windows.Devices.I2c.I2cDevice _device;
        // For the ReadByte and WriteByte operations
        private byte[] bufferSingleOperation = new byte[1];

        /// <summary>
        /// The connection settings of a device on an I2C bus. The connection settings are immutable after the device is created
        /// so the object returned will be a clone of the settings object.
        /// </summary>
        public I2cConnectionSettings ConnectionSettings { get; }

        /// <summary>
        /// Reads a byte from the I2C device.
        /// </summary>
        /// <returns>A byte read from the I2C device.</returns>
        public byte ReadByte()
        {
            _device.Read(bufferSingleOperation);
            return bufferSingleOperation[0];
        }

        /// <summary>
        /// Reads data from the I2C device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the I2C device.
        /// The length of the buffer determines how much data to read from the I2C device.
        /// </param>
        public void Read(SpanByte buffer)
        {
            // This is allocating an intermediate buffer and then copy back the data to 
            // the SpanByte. This is intend to be changed in a native implementation
            byte[] toRead = new byte[buffer.Length];
            _device.Read(toRead);
            for (int i = 0; i < toRead.Length; i++)
            {
                buffer[i] = toRead[i];
            }
        }

        /// <summary>
        /// Writes a byte to the I2C device.
        /// </summary>
        /// <param name="value">The byte to be written to the I2C device.</param>
        public void WriteByte(byte value)
        {
            bufferSingleOperation[0] = value;
            _device.Write(bufferSingleOperation);
        }

        /// <summary>
        /// Writes data to the I2C device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that contains the data to be written to the I2C device.
        /// The data should not include the I2C device address.
        /// </param>
        public void Write(SpanByte buffer)
        {
            // This is allocating an intermediate buffer using the buffer of 
            // the SpanByte. This is intend to be changed in a native implementation
            _device.Write(buffer.ToArray());
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
        public void WriteRead(SpanByte writeBuffer, SpanByte readBuffer)
        {
            // This is allocating an intermediate buffer and then copy back the data to 
            // the SpanByte. This is intend to be changed in a native implementation
            byte[] toRead = new byte[readBuffer.Length];
            _device.WriteRead(writeBuffer.ToArray(), toRead);
            for (int i = 0; i < toRead.Length; i++)
            {
                readBuffer[i] = toRead[i];
            }
        }

        /// <summary>
        /// Creates a communications channel to a device on an I2C bus running on the current platform
        /// </summary>
        /// <param name="settings">The connection settings of a device on an I2C bus.</param>
        /// <returns>A communications channel to a device on an I2C bus running on Windows 10 IoT.</returns>
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
            ConnectionSettings = settings;
            _device = Windows.Devices.I2c.I2cDevice.FromId($"I2C{settings.BusId}", new Windows.Devices.I2c.I2cConnectionSettings(settings.DeviceAddress)
            {
                BusSpeed = (Windows.Devices.I2c.I2cBusSpeed)settings.BusSpeed,
                SharingMode = (Windows.Devices.I2c.I2cSharingMode)settings.SharingMode
            });
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if explicitly disposing, <see langword="false"/> if in finalizer</param>
        void Dispose(bool disposing)
        { }
    }
}