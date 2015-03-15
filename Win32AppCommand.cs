//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpLibHid.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Runtime.InteropServices;

namespace Win32
{
    static public partial class Const
    {
        public const int WM_APPCOMMAND = 0x0319;
    }

    static public partial class Macro
    {
        public static int GET_APPCOMMAND_LPARAM(IntPtr lParam)
        {
            return ((short)HIWORD(lParam.ToInt32()) & ~Const.FAPPCOMMAND_MASK);
        }
    }
}