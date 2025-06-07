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
    internal nint _processHandle;

    public MemoryReader() { }
    
    /// <summary>
    /// Provides functionality for reading memory from an external process.
    /// </summary>
    public MemoryReader(int processId)
    {
        // open the process with the required access rights.
        _processHandle = NativeMethods.OpenProcess
        (
            ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation, // required access rights
            false, // do not inherit the handle
            processId // process identifier
        );

        // check if the process was successfully opened.
        if (_processHandle == nint.Zero)
            throw new Exception("Failed to open process."); //  throw an exception if the process was not opened.
    }

    /// <summary>
    /// Reads a sequence of bytes from the specified memory address in the target process.
    /// </summary>
    /// <param name="address">The memory address from which to start reading the bytes.</param>
    /// <param name="size">The number of bytes to read from the specified memory address.</param>
    /// <returns>A byte array containing the data read from the specified memory address.</returns>
    public byte[] ReadBytes(nint address, int size)
    {
        // check if the process handle is valid.
        byte[] buffer = new byte[size];
        
        // read the memory from the process.
        NativeMethods.ReadProcessMemory(_processHandle, address, buffer, size, out _);
        
        // check if the read operation was successful.
        return buffer;
    }

    /// <summary>
    /// Reads a 32-bit integer (Int32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int32 value should be read.</param>
    /// <returns>Returns the Int32 value read from the specified memory address.</returns>
    public int ReadInt32(nint address) => BitConverter.ToInt32(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads a single-precision floating-point value (float) from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the float value should be read.</param>
    /// <returns>Returns the float value read from the specified memory address.</returns>
    public float ReadFloat(nint address) => BitConverter.ToSingle(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads a 64-bit integer (Int64) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the Int64 value should be read.</param>
    /// <returns>Returns the Int64 value read from the specified memory address.</returns>
    public long ReadInt64(nint address) => BitConverter.ToInt64(ReadBytes(address, 8), 0);

    /// <summary>
    /// Reads a 16-bit unsigned integer (UInt16) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt16 value should be read.</param>
    /// <returns>Returns the UInt16 value read from the specified memory address.</returns>
    public ushort ReadUInt16(nint address) => BitConverter.ToUInt16(ReadBytes(address, 2), 0);

    /// <summary>
    /// Reads a 32-bit unsigned integer (UInt32) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt32 value should be read.</param>
    /// <returns>Returns the UInt32 value read from the specified memory address.</returns>
    public uint ReadUInt32(nint address) => BitConverter.ToUInt32(ReadBytes(address, 4), 0);

    /// <summary>
    /// Reads an unsigned 64-bit integer (UInt64) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt64 value should be read.</param>
    /// <returns>Returns the UInt64 value read from the specified memory address.</returns>
    public uint ReadUInt64(nint address) => (uint)BitConverter.ToUInt64(ReadBytes(address, 8), 0);

    /// <summary>
    /// Reads a 128-bit unsigned integer (UInt128) value from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the UInt128 value should be read.</param>
    /// <returns>Returns the UInt128 value read from the specified memory address.</returns>
    public uint ReadUInt128(nint address) => (uint)BitConverter.ToUInt128(ReadBytes(address, 16), 0);

    /// <summary>
    /// Reads a structure of type <typeparamref name="T"/> from the specified memory address.
    /// </summary>
    /// <param name="address">The memory address from which the structure of type <typeparamref name="T"/> should be read.</param>
    /// <typeparam name="T">The type of the structure to read. Must be a value type (struct).</typeparam>
    /// <returns>Returns the structure of type <typeparamref name="T"/> read from the specified memory address.</returns>
    public T ReadStruct<T>(nint address) where T : struct
    {
        // get the size of the structure.
        int size = Marshal.SizeOf<T>();
        
        // read the structure from the memory.
        byte[] buffer = ReadBytes(address, size);
        
        // create a GCHandle to pin the buffer.
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        T structure = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
        
        // free the handle.
        handle.Free();
        
        // return the structure.
        return structure;
    }

    /// <summary>
    /// Resolves a multi-level pointer chain, starting from a base address and traversing
    /// through the specified offsets to compute the final address.
    /// </summary>
    /// <param name="baseAddress">The initial base address where the pointer chain resolution begins.</param>
    /// <param name="offsets">An array of offsets to traverse through the pointer chain at each level.</param>
    /// <returns>Returns the resolved memory address after traversing the pointer chain.</returns>
    public nint ReadPointerChain(nint baseAddress, int[] offsets)
    {
        // check if the process handle is valid.
        nint currentAddress = baseAddress;
        
        // iterate through the offsets and resolve the pointer chain.
        for (int i = 0; i < offsets.Length; i++)
        {
            // read the current offset.
            currentAddress = (nint)ReadInt64(currentAddress + i);
            currentAddress += i; // add the offset to the current address.
        }
        
        // return the resolved address.
        return currentAddress;
    }

    /// <summary>
    /// Releases the resources used by the MemoryReader instance, including closing the handle to the target process.
    /// </summary>
    public void Dispose()
    {
        // check if the process handle is valid.
        if (_processHandle != nint.Zero)
            NativeMethods.CloseHandle(_processHandle); // close the process handle.
    }

    /// <summary>
    /// Attaches the memory reader to a target process specified by its process ID.
    /// </summary>
    /// <param name="processId">The identifier of the process to attach to.</param>
    /// <returns>
    /// <c>true</c> if the process was successfully attached; otherwise, <c>false</c>.
    /// </returns>
    public bool Attach(int processId)
    {
        _processHandle = NativeMethods.OpenProcess(
            ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation, // required access rights
            false, // do not inherit the handle
            processId // process identifier
        );

        // check if the process was successfully opened.
        return _processHandle != nint.Zero;
    }
}