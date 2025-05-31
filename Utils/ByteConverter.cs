/******************************************************************************
Copyright (C) 2025 by RedMeansWar - ByteConverter.cs

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

DISCLAIMER:
This tool is intended for legal, educational, and debugging purposes only.
The author is not responsible for any misuse of this library.
******************************************************************************/

using System;
using System.Globalization;

namespace MemUtil.Utils;

/// <summary>
/// Provides utility methods for converting byte arrays to various data types.
/// </summary>
public static class ByteConverter
{
    /// <summary>
    /// Converts a byte array to a 32-bit signed integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 4 bytes to be converted to an Int32 value.</param>
    /// <returns>The 32-bit signed integer (Int32) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 4 bytes.</exception>
    public static int ToInt32(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 4)
        {
            throw new ArgumentException("Buffer is too small for Int32.");
        }
        
        return BitConverter.ToInt32(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a 32-bit unsigned integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 4 bytes to be converted to a UInt32 value.</param>
    /// <returns>The 32-bit unsigned integer (UInt32) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 4 bytes.</exception>
    public static uint ToUInt32(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 4)
        {
            throw new ArgumentException("Buffer is too small for UInt32.");
        }
        
        return BitConverter.ToUInt32(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a 64-bit unsigned integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 8 bytes to be converted to a UInt64 value.</param>
    /// <returns>The 64-bit unsigned integer (UInt64) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 8 bytes.</exception>
    public static uint ToUInt64(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 8)
        {
            throw new ArgumentException("Buffer is too small for UInt64.");
        }
        
        return (uint)BitConverter.ToUInt64(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a 128-bit unsigned integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 16 bytes to be converted to a UInt128 value.</param>
    /// <returns>The 128-bit unsigned integer (UInt128) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 16 bytes.</exception>
    public static uint ToUInt128(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 16)
        {
            throw new ArgumentException("Buffer is too small for UInt128.");
        }
        
        return (uint)(BitConverter.ToUInt64(buffer, 0) << 64 | BitConverter.ToUInt64(buffer, 8));
    }

    /// <summary>
    /// Converts a byte array to a 64-bit signed integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 8 bytes to be converted to an Int64 value.</param>
    /// <returns>The 64-bit signed integer (Int64) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 8 bytes.</exception>
    public static long ToInt64(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 8)
        {
            throw new ArgumentException("Buffer is too small for Int64.");
        }
        
        return BitConverter.ToInt64(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a single-precision floating-point number.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 4 bytes to be converted to a float value.</param>
    /// <returns>The single-precision floating-point number (float) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 4 bytes.</exception>
    public static float ToFloat(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 4)
        {
            throw new ArgumentException("Buffer is too small for Single (float).");
        }
        
        return BitConverter.ToSingle(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a 64-bit double-precision floating-point number.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 8 bytes to be converted to a Double value.</param>
    /// <returns>The 64-bit double-precision floating-point number (Double) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 8 bytes.</exception>
    public static double ToDouble(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 8)
        {
            throw new ArgumentException("Buffer is too small for Double.");
        }
        
        return BitConverter.ToDouble(buffer, 0);
    }

    /// <summary>
    /// Converts a byte array to a 16-bit signed integer.
    /// </summary>
    /// <param name="buffer">The byte array containing at least 2 bytes to be converted to an Int16 value.</param>
    /// <returns>The 16-bit signed integer (Int16) represented by the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown if the byte array is null or contains fewer than 2 bytes.</exception>
    public static short ToInt16(byte[] buffer)
    {
        if (buffer is null || buffer.Length < 2)
        {
            throw new ArgumentException("Buffer is too small for Int16.");
        }
        
        return BitConverter.ToInt16(buffer, 0);
    }

    /// <summary>
    /// Attempts to parse a hexadecimal string into an <see cref="IntPtr"/> address.
    /// </summary>
    /// <param name="hex">The hexadecimal string to be parsed, which may optionally start with "0x".</param>
    /// <param name="address">The parsed <see cref="IntPtr"/> value if the conversion succeeds, or <see cref="IntPtr.Zero"/> if it fails.</param>
    /// <returns>True if the string was successfully parsed into an <see cref="IntPtr"/> value; otherwise, false.</returns>
    public static bool TryParseHexAddress(string hex, out IntPtr address)
    {
        address = IntPtr.Zero;
        if (string.IsNullOrWhiteSpace(hex))
            return false;

        hex = hex.Trim();

        if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            hex = hex.Substring(2);

        if (!ulong.TryParse(hex, NumberStyles.HexNumber, null, out ulong result)) return false;
        
        address = new(unchecked((long)result));
        return true;

    }
}