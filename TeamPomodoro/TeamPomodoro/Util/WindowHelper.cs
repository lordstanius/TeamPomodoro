using System;
using System.Runtime.InteropServices;

namespace TeamPomodoro.Util
{
	public static class WindowHelper
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HTCAPTION = 0x2;
	
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		/// <summary>
		/// Moves window as a response to non-client area mouse click and drag. 
		/// Use this method as a response to MouseMove event for a control to produce the result as if the window Title Bar is moved.
		/// </summary>
		/// <param name="hwnd">Handle to window being dragged.</param>
		/// <param name="x">Current X coordinate of the window</param>
		/// <param name="y">Current Y coordinate of the window</param>
		public static void Move(IntPtr hwnd, int x, int y)
		{
			ReleaseCapture();
			SendMessage(hwnd, WM_NCLBUTTONDOWN, HTCAPTION, x | (y << 16));
		}

		/// <summary>
		/// Moves window as a response to non-client area mouse click and drag. 
		/// Use this method as a response to MouseMove event for a control to produce the result as if the window Title Bar is moved.
		/// This method does not send information about current window position.
		/// </summary>
		/// <param name="hwnd"></param>
		public static void Move(IntPtr hwnd)
		{
			ReleaseCapture();
			SendMessage(hwnd, WM_NCLBUTTONDOWN, HTCAPTION, 0);
		}
	}
}
