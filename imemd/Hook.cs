using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace imemd
{
    class Hook
    {
        private int clickWaitMs;
        private int sameWindowSec;
        private bool iBeamCheck = false;

        private IntPtr hHook;
        private IntPtr lastActiveWindow;
        private DateTime lastWindowChange;
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

            lastWindowChange = new DateTime();
            lastWindowChange = DateTime.Now;
            lastActiveWindow = Win32API.GetForegroundWindow();

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
                // フルスクリーンなら実行しない
                if (IsFullscreen() == true)
                {
                    return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
                }

                // ウインドウ変更時は経過時間に関係なく実行
                if (IsChangeWindow() == true)
                {
                    InputZenkaku();
                    return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
                }

                // 時間経過してないなら実行しない
                if (IsElapsedSameWindowSec() == false)
                {
                    return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
                }


                // Iビーム判定
                if (IBeamCursorCheck() == false)
                {
                    return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
                }

                InputZenkaku();
            }

            return Win32API.CallNextHookEx(this.hHook, nCode, wParam, lParam);
        }

        private bool IsFullscreen()
        {
            Win32API.GetWindowRect(new HandleRef(this, lastActiveWindow), out Win32API.RECT fullRect);
            Win32API.GetClientRect(lastActiveWindow, out Win32API.RECT cliRect);
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

        private void InputZenkaku()
        {
            System.Threading.Thread.Sleep(this.clickWaitMs); // ウインドウがアクティブになるのを待つ

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

        private bool IsElapsedSameWindowSec()
        {
            bool ret = false;
            if ((lastWindowChange + TimeSpan.FromSeconds(sameWindowSec)) <= DateTime.Now)
            {
                ret = true;
            }
            lastWindowChange = DateTime.Now;

            return ret;
        }

        private bool IsChangeWindow()
        {
            if (lastActiveWindow != Win32API.GetForegroundWindow())
            {
                lastActiveWindow = Win32API.GetForegroundWindow();
                return true;
            }
            return false;
        }
    }
}
