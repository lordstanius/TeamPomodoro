using System;

namespace TeamPomodoro.Util
{
    public static class WindowHelper
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        /// <summary>
        /// Moves window as a response to non-client area mouse click and drag. 
        /// Use this method as a response to MouseMove event for a control to produce the result as if the window Title Bar is moved.
        /// This method does not send information about current window position.
        /// </summary>
        /// <param name="hwnd"></param>
        public static void Move(IntPtr hwnd)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(hwnd, WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
        }
    }
}
