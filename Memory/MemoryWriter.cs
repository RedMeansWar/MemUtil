/******************************************************************************
Copyright (C) 2025 by RedMeansWar - MemoryWriter.cs

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
using MemUtil.Enums;
using MemUtil.Interfaces;
using MemUtil.Utils;

namespace MemUtil.Memory;

/// <summary>
/// Provides functionality to write data into the memory of an external process.
/// </summary>
public class MemoryWriter : IMemoryWriter
{
    internal nint _processHandle;

    public MemoryWriter() { }
    
    /// <summary>
    /// Provides functionality to write data into the memory of an external process.
    /// </summary>
    public MemoryWriter(int processId)
    {
        // open the process with the specified process identifier.
        _processHandle = NativeMethods.OpenProcess
        (
            ProcessAccessFlags.VirtualMemoryWrite | // VirtualMemoryWrite is required for WriteProcessMemory.
            ProcessAccessFlags.VirtualMemoryOperation | // VirtualMemoryOperation is required for WriteProcessMemory.
            ProcessAccessFlags.QueryInformation, // QueryInformation is required for GetProcessId.
            false, // inherit the parent process's handle.
            processId // the process identifier.
        );

        if (_processHandle == nint.Zero) // OpenProcess failed.
        {
            // throw an exception saying the process failed to open.
            throw new Exception("Failed to open process.");
        }
    }

    /// <summary>
    /// Writes an array of bytes to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the bytes will be written.</param>
    /// <param name="bytes">The array of bytes to write to the specified memory address.</param>
    /// <returns>True if the bytes are written successfully; otherwise, false.</returns>
    public bool WriteBytes(nint address, byte[] bytes)
    {
        return NativeMethods.WriteProcessMemory(_processHandle, address, bytes, bytes.Length, out _);
    }

    /// <summary>
    /// Writes a 32-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 32-bit integer will be written.</param>
    /// <param name="value">The 32-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the 32-bit integer is written successfully; otherwise, false.</returns>
    public bool WriteInt32(nint address, int value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // convert the int value to a byte array.
        return WriteBytes(address, bytes); // write the byte array to the specified memory address.
    }

    /// <summary>
    /// Writes a single-precision floating-point value to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the floating-point value will be written.</param>
    /// <param name="value">The single-precision floating-point value to write to the specified memory address.</param>
    /// <returns>True if the floating-point value is written successfully; otherwise, false.</returns>
    public bool WriteFloat(nint address, float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return WriteBytes(address, bytes);
    }

    /// <summary>
    /// Writes a 64-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 64-bit integer will be written.</param>
    /// <param name="value">The 64-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the 64-bit integer is written successfully; otherwise, false.</returns>
    public bool WriteInt64(nint address, long value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // convert the long value to a byte array.
        return WriteBytes(address, bytes); // write the byte array to the specified memory address.
    }

    /// <summary>
    /// Writes an unsigned 32-bit integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the unsigned 32-bit integer will be written.</param>
    /// <param name="value">The unsigned 32-bit integer value to write to the specified memory address.</param>
    /// <returns>True if the unsigned 32-bit integer is written successfully; otherwise, false.</returns>
    public bool WriteUInt32(nint address, uint value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // convert the uint value to a byte array.
        return WriteBytes(address, bytes); // write the byte array to the specified memory address.
    }

    /// <summary>
    /// Writes a 64-bit unsigned integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 64-bit unsigned integer will be written.</param>
    /// <param name="value">The 64-bit unsigned integer value to write to the specified memory address.</param>
    /// <returns>True if the 64-bit unsigned integer is written successfully; otherwise, false.</returns>
    public bool WriteUInt64(nint address, ulong value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // convert the ulong value to a byte array.
        return WriteBytes(address, bytes); // write the byte array to the specified memory address.
    }

    /// <summary>
    /// Writes a 128-bit unsigned integer to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the 128-bit unsigned integer will be written.</param>
    /// <param name="value">The 128-bit unsigned integer value to write to the specified memory address.</param>
    /// <returns>True if the 128-bit unsigned integer is written successfully; otherwise, false.</returns>
    public bool WriteUInt128(nint address, UInt128 value)
    {
        byte[] bytes = BitConverter.GetBytes(value); // convert the UInt128 value to a byte array.
        return WriteBytes(address, bytes); // write the byte array to the specified memory address.
    }

    /// <summary>
    /// Writes a structure of a specified type to the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address where the structure will be written.</param>
    /// <param name="value">The structure value to write to the specified memory address.</param>
    /// <typeparam name="T">The type of the structure to be written. Must be a value type (struct).</typeparam>
    /// <returns>True if the structure is written successfully; otherwise, false.</returns>
    public bool WriteStruct<T>(nint address, T value) where T : struct
    {
        // get the size of the structure in bytes.
        int size = Marshal.SizeOf(typeof(T));
        byte[] bytes = new byte[size]; // allocate a byte array to hold the structure.

        // copy the structure to the byte array.
        nint ptr = Marshal.AllocHGlobal(size);
        
        try
        {
            // copy the structure to the memory allocated for the structure.
            Marshal.StructureToPtr(value, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size); // copy the structure to the byte array.
        }
        finally
        {
            // free the memory allocated for the structure.
            Marshal.FreeHGlobal(ptr);
        }
        
        // write the byte array to the specified memory address.
        return WriteBytes(address, bytes);
    }

    /// <summary>
    /// Releases the resources used by the MemoryWriter instance, including closing the
    /// handle to the target process.
    /// </summary>
    public void Dispose()
    {
        // close the handle to the target process if the processes handle is not zero.
        if (_processHandle == nint.Zero)
            NativeMethods.CloseHandle(_processHandle);
    }
}