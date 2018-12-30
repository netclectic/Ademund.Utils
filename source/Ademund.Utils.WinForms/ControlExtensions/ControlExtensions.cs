using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ademund.Utils.ControlExtensions
{
	public static class ControlExtensions
	{
		public static void UIThread(this Control control, Action code)
		{
			if (control == null)
				return;

			if (control.InvokeRequired)
			{
				control.BeginInvoke(code);
				return;
			}
			code.Invoke();
		}

		public static void UIThreadInvoke(this Control control, Action code)
		{
			if (control == null)
				return;

			if (control.InvokeRequired)
			{
				control.Invoke(code);
				return;
			}
			code.Invoke();
		}
	}

    public static class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control control, Action action)
        {
            SendMessage(control.Handle, WM_SETREDRAW, false, 0);
            action();
            SendMessage(control.Handle, WM_SETREDRAW, true, 0);
            control.Refresh();
        }
    }
}