using System;
using System.Reflection;
using ViewModel.Globalization;

namespace TeamPomodoro.Helper
{
    public static class WindowHelper
    {
        /// <summary>
        /// Moves window as a response to non-client area mouse click and drag. 
        /// Use this method as a response to MouseMove event for a control to produce the result as if the window Title Bar is moved.
        /// This method does not send information about current window position.
        /// </summary>
        /// <param name="hwnd">Handle to target window</param>
        public static void Move(IntPtr hwnd)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(hwnd, 0xA1, new IntPtr(0x2), IntPtr.Zero);
        }

        public static string ApplicationVersion
        {
            get { return Strings.TxtTeamPomodoro + " " + Assembly.GetExecutingAssembly().GetName().Version; }
        }
    }
}