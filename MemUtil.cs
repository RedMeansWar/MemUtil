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
using MemUtil.Memory;
using MemUtil.Processes;

namespace MemUtil;

public class MemUtil
{
    #region Variables
    internal static MemoryReader _memReader = new();
    internal static MemoryWriter _memWriter = new();
    #endregion

    #region Reader
    /// <summary>
    /// Reads a sequence of bytes from the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address from which to start reading the bytes.</param>
    /// <param name="size">The number of bytes to read from the specified memory address.</param>
    /// <returns>A byte array containing the data read from the specified memory address.</returns>
    public static byte[] ReadMemory(nint address, int size) => _memReader.ReadBytes(address, size);
    
    /// <summary>
    /// Reads a 32-bit integer (Int32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int32 value should be read.</param>
    /// <returns>Returns the Int32 value read from the specified memory address.</returns>
    public static int ReadInt32(nint address) => _memReader.ReadInt32(address);

    /// <summary>
    /// Reads a 64-bit integer (Int64) from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int64 value will be read.</param>
    /// <returns>The 64-bit integer (Int64) value read from the specified memory address.</returns>
    public static long ReadInt64(nint address) => _memReader.ReadInt64(address);

    /// <summary>
    /// Reads a 16-bit unsigned integer (UInt16) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt16 value should be read.</param>
    /// <returns>The UInt16 value read from the specified memory address.</returns>
    public static ushort ReadUInt16(nint address) => _memReader.ReadUInt16(address);

    /// <summary>
    /// Reads a 32-bit unsigned integer (UInt32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt32 value should be read.</param>
    /// <returns>Returns the UInt32 value read from the specified memory address.</returns>
    public static uint ReadUInt32(nint address) => _memReader.ReadUInt32(address);

    /// <summary>
    /// Reads an unsigned 64-bit integer (UInt64) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt64 value should be read.</param>
    /// <returns>Returns the UInt64 value read from the specified memory address.</returns>
    public static ulong ReadUInt64(nint address) => _memReader.ReadUInt64(address);

    /// <summary>
    /// Reads a 128-bit unsigned integer (UInt128) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt128 value should be read.</param>
    /// <returns>Returns the UInt128 value read from the specified memory address.</returns>
    public static uint ReadUInt128(nint address) => _memReader.ReadUInt128(address);

    /// <summary>
    /// Reads a structure of type <typeparamref name="T"/> from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the structure of type <typeparamref name="T"/> should be read.</param>
    /// <typeparam name="T">The type of structure to read. Must be a value type (struct).</typeparam>
    /// <returns>Returns the structure of type <typeparamref name="T"/> read from the specified memory address.</returns>
    public static T ReadStruct<T>(nint address) where T : struct => _memReader.ReadStruct<T>(address);

    /// <summary>
    /// Resolves a multi-level pointer chain by starting from a base address
    /// and iteratively applying the specified offsets to compute the final address.
    /// </summary>
    /// <param name="baseAddress">The initial base memory address from which the pointer chain traversal begins.</param>
    /// <param name="offsets">An array of integer offsets representing the levels of the pointer chain.</param>
    /// <returns>The resolved memory address after applying all the offsets to the base address.</returns>
    public static nint ReadPointerChain(nint baseAddress, int[] offsets) => _memReader.ReadPointerChain(baseAddress, offsets);

    /// <summary>
    /// Releases the resources used by the <see cref="MemoryReader"/> instance, including closing any associated handles to the target process.
    /// </summary>
    public static void DisposeMemoryReader() => _memReader.Dispose();

    /// <summary>
    /// Attaches the memory reader to a target process specified by its process ID.
    /// </summary>
    /// <param name="processId">The identifier of the process to which the memory reader should be attached.</param>
    /// <returns>
    /// <c>true</c> if the memory reader was successfully attached to the process;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool AttachMemoryReader(int processId) => _memReader.Attach(processId);
    #endregion

    #region Writer
    /// <summary>
    /// Writes an array of bytes to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the bytes will be written.</param>
    /// <param name="bytes">The array of bytes to write to the specified memory address.</param>
    /// <returns>
    /// <c>true</c> if the operation successfully writes the bytes to the target memory address;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool WriteBytes(nint address, byte[] bytes) => _memWriter.WriteBytes(address, bytes);

    /// <summary>
    /// Writes a 32-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 32-bit integer will be written.</param>
    /// <param name="value">The 32-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the 32-bit integer was successfully written; otherwise, false.</returns>
    public static bool WriteInt32(nint address, int value) => _memWriter.WriteInt32(address, value);

    /// <summary>
    /// Writes a 64-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 64-bit integer will be written.</param>
    /// <param name="value">The 64-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the 64-bit integer is written successfully; otherwise, false.</returns>
    public static bool WriteInt64(nint address, long value) => _memWriter.WriteInt64(address, value);

    /// <summary>
    /// Writes a single-precision floating-point value to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the floating-point value will be written.</param>
    /// <param name="value">The single-precision floating-point value to write to the specified memory address.</param>
    /// <returns>True if the value was successfully written; otherwise, false.</returns>
    public static bool WriteFloat(nint address, float value) => _memWriter.WriteFloat(address, value);

    /// <summary>
    /// Writes an unsigned 32-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the unsigned 32-bit integer will be written.</param>
    /// <param name="value">The unsigned 32-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the unsigned 32-bit integer is written successfully; otherwise, false.</returns>
    public static bool WriteUInt32(nint address, uint value) => _memWriter.WriteUInt32(address, value);

    /// <summary>
    /// Writes a 64-bit unsigned integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 64-bit unsigned integer will be written.</param>
    /// <param name="value">The 64-bit unsigned integer value to write to the specified memory address.</param>
    /// <returns>True if the 64-bit unsigned integer is written successfully; otherwise, false.</returns>
    public static bool WriteUInt64(nint address, UInt128 value) => _memWriter.WriteUInt128(address, value);

    /// <summary>
    /// Writes a 64-bit unsigned integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 64-bit unsigned integer will be written.</param>
    /// <param name="value">The 64-bit unsigned integer value to write to the specified memory address.</param>
    /// <returns>True if the 64-bit unsigned integer is written successfully; otherwise, false.</returns>
    public static bool WriteUInt64(nint address, ulong value) => _memWriter.WriteUInt64(address, value);

    /// <summary>
    /// Writes a structure of a specified type to the specified memory address in the target process.
    /// </summary>
    /// <typeparam name="T">The type of the structure to be written. Must be a value type (struct).</typeparam>
    /// <param name="address">The memory address where the structure will be written.</param>
    /// <param name="structure">The structure value to write to the specified memory address.</param>
    /// <returns>True if the structure is written successfully; otherwise, false.</returns>
    public static bool WriteStruct<T>(nint address, T structure) where T : struct 
        => _memWriter.WriteStruct(address, structure);

    /// <summary>
    /// Releases resources held by the static instance of <see cref="MemoryWriter"/>,
    /// including closing any active handles to the target process.
    /// </summary>
    public static void DisposeMemoryWriter() => _memWriter.Dispose();
    #endregion

    /// <summary>
    /// Retrieves the process ID of a process specified by its name.
    /// </summary>
    /// <param name="processName">The name of the process whose ID is to be retrieved.</param>
    /// <returns>The process ID if found; otherwise, null.</returns>
    public static int? GetProcessId(string processName)
        => ProcessUtils.GetProcessIdByName(processName);
}