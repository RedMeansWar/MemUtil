/******************************************************************************
Copyright (C) 2025 by RedMeansWar - ProcessUtils.cs

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

namespace MemUtil.Processes;

public static class ProcessUtils
{
    public static Process Process { get; private set; }
    public static IntPtr BaseAddress { get; private set; }

    /// <summary>
    /// Attaches to a process by its name.
    /// </summary>
    /// <param name="processName">The name of the process to attach to.</param>
    /// <returns>
    /// Returns true if the process was successfully attached; otherwise, false.
    /// </returns>
    public static bool AttachToProcess(string processName)
    {
        var processes = Process.GetProcessesByName(processName);
        if (processes.Length == 0)
            return false;

        Process = processes[0];
        BaseAddress = Process.MainModule?.BaseAddress ?? IntPtr.Zero;
        return true;
    }

    /// <summary>
    /// Retrieves the process ID of a process by its name.
    /// </summary>
    /// <param name="processName">The name of the process to search for.</param>
    /// <returns>
    /// Returns the process ID of the first matching process if found; otherwise, null.
    /// </returns>
    public static int? GetProcessIdByName(string processName)
    {
        var processes = Process.GetProcessesByName(processName);
        if (processes.Length == 0)
            return null;
        
        return processes[0].Id;
    }
}
