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