/******************************************************************************
Copyright (C) 2025 by RedMeansWar - MemUtils.cs

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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

namespace MemUtil;

public class MemUtil
{
    /// <summary>
    /// Attempts to parse the specified hexadecimal string representation of a memory address into an <see cref="IntPtr"/>.
    /// </summary>
    /// <param name="hex">The hexadecimal string representation of the address. It may optionally start with "0x".</param>
    /// <param name="address">
    /// When this method returns, contains the parsed memory address as an <see cref="IntPtr"/>,
    /// if the conversion succeeded, or <see cref="IntPtr.Zero"/> if the conversion failed.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if the string was successfully parsed as a valid hexadecimal memory address;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool TryParseHexAddress(string hex, out IntPtr address)
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