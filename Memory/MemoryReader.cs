/******************************************************************************
Copyright (C) 2025 by RedMeansWar - MemoryReader.cs

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
/// Provides functionality for reading memory from an external process.
/// </summary>
public class MemoryReader : IMemoryReader
{
    internal readonly IntPtr _processHandle;

    /// <summary>
    /// Provides functionality for reading memory from an external process.
    /// </summary>
    public MemoryReader(int processId)
    {
        _processHandle = NativeMethods.OpenProcess
        (
            ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation,
            false,
            processId
        );

        if (_processHandle == IntPtr.Zero)
        {
            throw new Exception("Failed to open process.");
        }
    }

    /// <summary>
    /// Reads a sequence of bytes from the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address from which to start reading the bytes.</param>
    /// <param name="size">The number of bytes to read from the specified memory address.</param>
    /// <returns>A byte array containing the data read from the specified memory address.</returns>
    public byte[] ReadBytes(IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        NativeMethods.ReadProcessMemory(_processHandle, address, buffer, size, out _);
        return buffer;
    }

    /// <summary>
    /// Reads a 32-bit integer (Int32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int32 value should be read.</param>
    /// <returns>Returns the Int32 value read from the specified memory address.</returns>
    public int ReadInt32(IntPtr address) => BitConverter.ToInt32(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads a single-precision floating-point value (float) from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the float value should be read.</param>
    /// <returns>Returns the float value read from the specified memory address.</returns>
    public float ReadFloat(IntPtr address) => BitConverter.ToSingle(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads a 64-bit integer (Int64) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int64 value should be read.</param>
    /// <returns>Returns the Int64 value read from the specified memory address.</returns>
    public long ReadInt64(IntPtr address) => BitConverter.ToInt64(ReadBytes(address, 8), 0);

    /// <summary>
    /// Reads a 16-bit unsigned integer (UInt16) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt16 value should be read.</param>
    /// <returns>Returns the UInt16 value read from the specified memory address.</returns>
    public uint ReadUInt16(IntPtr address) => BitConverter.ToUInt16(ReadBytes(address, 2), 0);

    /// <summary>
    /// Reads a 32-bit unsigned integer (UInt32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt32 value should be read.</param>
    /// <returns>Returns the UInt32 value read from the specified memory address.</returns>
    public uint ReadUInt32(IntPtr address) => BitConverter.ToUInt32(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads an unsigned 64-bit integer (UInt64) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt64 value should be read.</param>
    /// <returns>Returns the UInt64 value read from the specified memory address.</returns>
    public uint ReadUInt64(IntPtr address) => (uint)BitConverter.ToUInt64(ReadBytes(address, 8), 0);

    /// <summary>
    /// Reads a 128-bit unsigned integer (UInt128) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt128 value should be read.</param>
    /// <returns>Returns the UInt128 value read from the specified memory address.</returns>
    public uint ReadUInt128(IntPtr address) => (uint)BitConverter.ToUInt128(ReadBytes(address, 16), 0);

    /// <summary>
    /// Reads a structure of type <typeparamref name="T"/> from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the structure of type <typeparamref name="T"/> should be read.</param>
    /// <typeparam name="T">The type of the structure to read. Must be a value type (struct).</typeparam>
    /// <returns>Returns the structure of type <typeparamref name="T"/> read from the specified memory address.</returns>
    public T ReadStruct<T>(IntPtr address) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] buffer = ReadBytes(address, size);
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        T structure = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
        handle.Free();
        return structure;
    }

    /// <summary>
    /// Resolves a multi-level pointer chain, starting from a base address and traversing
    /// through the specified offsets to compute the final address.
    /// </summary>
    /// <param name="baseAddress">The initial base address where the pointer chain resolution begins.</param>
    /// <param name="offsets">An array of offsets to traverse through the pointer chain at each level.</param>
    /// <returns>Returns the resolved memory address after traversing the pointer chain.</returns>
    public IntPtr ReadPointerChain(IntPtr baseAddress, int[] offsets)
    {
        IntPtr currentAddress = baseAddress;
        for (int i = 0; i < offsets.Length; i++)
        {
            currentAddress = (IntPtr)ReadInt64(currentAddress + i);
            currentAddress += i;
        }
        
        return currentAddress;
    }

    /// <summary>
    /// Releases the resources used by the MemoryReader instance, including closing the handle to the target process.
    /// </summary>
    public void Dispose()
    {
        if (_processHandle != IntPtr.Zero)
        {
            NativeMethods.CloseHandle(_processHandle);
        }
    }
}