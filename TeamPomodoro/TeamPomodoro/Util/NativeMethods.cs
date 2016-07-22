using System;
using System.Runtime.InteropServices;

namespace TeamPomodoro.Util
{
	internal class NativeMethods
	{
		[DllImport("user32.dll")]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern bool ReleaseCapture();
	}
}
