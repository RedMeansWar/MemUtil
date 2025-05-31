/******************************************************************************
Copyright (C) 2025 by RedMeansWar - IMemoryReader.cs

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

namespace MemUtil.Interfaces;

public interface IMemoryReader : IDisposable
{
    byte[] ReadBytes(IntPtr address, int size);
    int ReadInt32(IntPtr address);
    long ReadInt64(IntPtr address);
    float ReadFloat(IntPtr address);
    T ReadStruct<T>(IntPtr address) where T : struct;
    IntPtr ReadPointerChain(IntPtr baseAddress, int[] offsets);
}