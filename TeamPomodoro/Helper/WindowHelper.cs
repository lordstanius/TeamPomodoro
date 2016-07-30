using System;
using System.Reflection;
using ViewModel.Globalization;

namespace TeamPomodoro.Helper
{
    public static class WindowHelper
    {
        public static string ApplicationVersion
        {
            get { return Strings.TxtTeamPomodoro + " " + Assembly.GetExecutingAssembly().GetName().Version; }
        }

        /// <summary>
        /// Moves window as a response to non-client area mouse click and drag. 
        /// Use this method as a response to MouseMove event for a control to produce the result as if the window Title Bar is moved.
        /// This method does not send information about current window position.
        /// </summary>
        /// <param name="handle">Handle to target window</param>
        public static void Move(IntPtr handle)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(handle, 0xA1, new IntPtr(0x2), IntPtr.Zero);
        }
    }
}