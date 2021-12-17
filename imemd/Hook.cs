using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace imemd
{
    class Hook
    {
        private int clickWaitMs;
        private int sameWindowSec;
        private bool iBeamCheck;

        private IntPtr hHook;
        private IntPtr lastClickWindow;
        private DateTime lastWindowChange;
        private DateTime lastClickTime;
        private Win32API.HOOKPROC hHookProc;
        private GCHandle allocHandle;

        public void FreeAllocHandle()
        {
            this.allocHandle.Free();
        }

        public bool SetMouseHook()
        {
            this.hHookProc = MouseHookCallback;
            this.allocHandle = GCHandle.Alloc(this.hHookProc);

            IntPtr hModule = Win32API.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            this.hHook = Win32API.SetWindowsHookEx((int)Win32API.HOOK_TYPE.WH_MOUSE_LL, this.hHookProc, hModule, IntPtr.Zero);
            if (this.hHook == null)
            {
                return false;
            }

            lastWindowChange = DateTime.Now;
            lastClickWindow = Win32API.GetForegroundWindow();

            return true;
        }

        public void UpdateHookSettings(int clickWait, int windowCheck, bool iBeam)
        {
            this.clickWaitMs = clickWait;
            this.sameWindowSec = windowCheck;
            this.iBeamCheck = iBeam;
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode == Win32API.HC_ACTION) &&
                ((Win32API.MOUSE_MESSAGE.WM_LBUTTONUP == (Win32API.MOUSE_MESSAGE)wParam)))
            {
                if (IsExecAction() == true)
                {
                    InputZenkaku();
                }

                lastClickWindow = Win32API.GetForegroundWindow();
            }
            return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
        }

        private bool IsExecAction()
        {
            // フルスクリーンなら実行しない
            if (IsFullscreen() == true)
            {
                Debug.WriteLine("Don't Exec, IsFullscreen() == true");
                return false;
            }

            // Chormeなら実行
            if (IsChorme() == true)
            {
                Debug.WriteLine("Exec, IsChorme() == true");
                return true;
            }

            // Iビーム判定
            if (IBeamCursorCheck() == false)
            {
                Debug.WriteLine("Don't Exec, IBeamCursorCheck() == false");
                return false;
            }

            // ウインドウ変更時は経過時間に関係なく実行
            if (IsChangeWindow() == true)
            {
                Debug.WriteLine("Exec, IsChangeWindow() == true");
                return true;
            }

            // 時間経過していたら実行
            if (IsElapsedSameWindowSec() == true)
            {
                Debug.WriteLine("Exec, IsElapsedSameWindowSec() == true");
                return true;
            }

            Debug.WriteLine("Don't Exec");
            return false;
        }

        private bool IsChorme()
        {
            IntPtr hWnd = Win32API.GetForegroundWindow();
            int length = Win32API.GetWindowTextLength(hWnd);
            StringBuilder tsb = new StringBuilder(length + 1);
            Win32API.GetWindowText(hWnd, tsb, tsb.Capacity);
            Debug.WriteLine(tsb.ToString());

            if (tsb.ToString().Contains(" - Google Chrome") == true)
            {
                return true;
            }

            return false;
        }

        private bool IsFullscreen()
        {
            IntPtr window = Win32API.GetForegroundWindow();
            Win32API.GetWindowRect(new HandleRef(this, window), out Win32API.RECT fullRect);
            Win32API.GetClientRect(window, out Win32API.RECT cliRect);

            if (fullRect.left == cliRect.left &&
                fullRect.top == cliRect.top &&
                fullRect.right == cliRect.right &&
                fullRect.bottom == cliRect.bottom)
            {
                return true;
            }
            return false;
        }

        private bool IBeamCursorCheck()
        {
            Win32API.CURSORINFO cInfo = new Win32API.CURSORINFO
            {
                cbSize = Marshal.SizeOf(typeof(Win32API.CURSORINFO))
            };
            Win32API.GetCursorInfo(ref cInfo);

            if ((cInfo.hCursor == Win32API.LoadCursor(IntPtr.Zero, (int)Win32API.IDC_STANDARD_CURSORS.IDC_IBEAM)) ||
               (this.iBeamCheck == false))
            {
                return true;
            }
            return false;
        }

        private async void InputZenkaku()
        {
            Debug.WriteLine("InputZenkaku()");

            lastClickTime = DateTime.Now;
            await Task.Delay(this.clickWaitMs); // ウインドウがアクティブになるのを待つ
            if (lastClickTime.AddMilliseconds(this.clickWaitMs) > DateTime.Now)
            {
                // Delay中に二回目が来ていたらlaxtClickTimeが書き換わっている
                Debug.WriteLine("Cancel(Double Click)");
                lastClickTime = DateTime.Now;
                return;
            }

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

            lastWindowChange = DateTime.Now;
        }

        private bool IsElapsedSameWindowSec()
        {
            if ((lastWindowChange + TimeSpan.FromSeconds(sameWindowSec)) <= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        private bool IsChangeWindow()
        {
            if (lastClickWindow != Win32API.GetForegroundWindow())
            {
                return true;
            }
            return false;
        }
    }
}
