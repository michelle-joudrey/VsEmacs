using System.Runtime.InteropServices;

namespace VsEmacs
{
    internal class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
    }
}