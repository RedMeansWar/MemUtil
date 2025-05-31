/******************************************************************************
Copyright (C) 2025 by RedMeansWar - ProcessAccessFlags.cs

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

namespace MemUtil.Enums;

[Flags]
public enum ProcessAccessFlags : uint
{
    Terminate = 0x0001,
    CreateThread = 0x0002,
    VirtualMemoryOperation = 0x0008,
    VirtualMemoryRead = 0x0010,
    VirtualMemoryWrite = 0x0020,
    DuplicateHandle = 0x0040,
    QueryInformation = 0x0400,
    QueryLimitedInformation = 0x1000,
    Synchronize = 0x00100000,
    All = 0x001F0FFF
}