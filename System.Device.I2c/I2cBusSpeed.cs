﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.I2c
{
    /// <summary>
    /// Describes the bus speeds that are available for connecting to an inter-integrated circuit (I2C) device. 
    /// The bus speed is the frequency at which to clock the I2C bus when accessing the device.
    /// </summary>
	public enum I2cBusSpeed
    {
        /// <summary>
        /// The standard speed of 100 kilohertz (kHz). This speed is the default.
        /// </summary>
        StandardMode,

        /// <summary>
        /// A fast speed of 400 kHz.
        /// </summary>
        FastMode,

        /// <summary>
        /// A super fast speed of 1 MHz.
        /// </summary>
        FastModePlus
    }
}
