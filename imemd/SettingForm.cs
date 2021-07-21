using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace imemd
{
    public partial class SettingForm : Form
    {
        private IntPtr hHook;

        public SettingForm()
        {
            InitializeComponent();
            SetMouseHook();
        }

        private int SetMouseHook()
        {
            IntPtr hModule = Win32API.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            hHook = Win32API.SetWindowsHookEx((int)Win32API.HOOK_TYPE.WH_MOUSE_LL, MouseHookCallback, hModule, IntPtr.Zero);
            if (hHook == null)
            {
                MessageBox.Show("Error: SetWindowsHookEx", "Error");
                return -1;
            }
            return 0;
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Win32API.CURSORINFO cInfo = new Win32API.CURSORINFO
            {
                cbSize = Marshal.SizeOf(typeof(Win32API.CURSORINFO))
            };
            Win32API.GetCursorInfo(ref cInfo);

            if ((nCode == Win32API.HC_ACTION) &&
                (Win32API.MOUSE_MESSAGE.WM_LBUTTONUP == (Win32API.MOUSE_MESSAGE)wParam))
            {
                if (cInfo.hCursor == Win32API.LoadCursor(IntPtr.Zero, (int)Win32API.IDC_STANDARD_CURSORS.IDC_IBEAM))
                {
                    InputZenkaku();
                }
            }

            return Win32API.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        private void InputZenkaku()
        {
            System.Threading.Thread.Sleep(50); // ウインドウがアクティブになるのを待つ

            Win32API.INPUT input = new Win32API.INPUT
            {
                type = Win32API.INPUT_KEYBOARD
            };
            input.ki.wScan = 0;
            input.ki.time = 0;
            input.ki.dwExtraInfo = Win32API.GetMessageExtraInfo();
            input.ki.wVk = Win32API.VK_KANJI;

            //Key Down
            input.ki.dwFlags = Win32API.KEYEVENTF_KEYDOWN;
            Win32API.SendInput(1, ref input, Marshal.SizeOf(typeof(Win32API.INPUT)));

            //Key Up
            input.ki.dwFlags = Win32API.KEYEVENTF_KEYUP;
            Win32API.SendInput(1, ref input, Marshal.SizeOf(typeof(Win32API.INPUT)));

            //Key Down
            input.ki.dwFlags = Win32API.KEYEVENTF_KEYDOWN;
            Win32API.SendInput(1, ref input, Marshal.SizeOf(typeof(Win32API.INPUT)));

            //Key Up
            input.ki.dwFlags = Win32API.KEYEVENTF_KEYUP;
            Win32API.SendInput(1, ref input, Marshal.SizeOf(typeof(Win32API.INPUT)));
        }
    }
}
