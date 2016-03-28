namespace PtpChat.Main.Ribbon.Classes
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class GlobalHook : IDisposable
    {
        #region Subclasses

        /// <summary>
        /// Types of available hooks
        /// </summary>
        public enum HookTypes
        {
            /// <summary>
            /// Installs a mouse hook
            /// </summary>
            Mouse,

            /// <summary>
            /// Installs a keyboard hook
            /// </summary>
            Keyboard
        }

        #endregion

        #region Fields

        private HookProcCallBack _HookProc;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.Handle != 0)
            {
                this.Unhook();
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate used to recieve HookProc
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        internal delegate int HookProcCallBack(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Events

        /// <summary>
        /// Occours when the hook captures a mouse click
        /// </summary>
        public event MouseEventHandler MouseClick;

        /// <summary>
        /// Occours when the hook captures a mouse double click
        /// </summary>
        public event MouseEventHandler MouseDoubleClick;

        /// <summary>
        /// Occours when the hook captures the mouse wheel
        /// </summary>
        public event MouseEventHandler MouseWheel;

        /// <summary>
        /// Occours when the hook captures the press of a mouse button
        /// </summary>
        public event MouseEventHandler MouseDown;

        /// <summary>
        /// Occours when the hook captures the release of a mouse button
        /// </summary>
        public event MouseEventHandler MouseUp;

        /// <summary>
        /// Occours when the hook captures the mouse moving over the screen
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// Occours when a key is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Occours when a key is released
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// Occours when a key is pressed
        /// </summary>
        public event KeyPressEventHandler KeyPress;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Hook of the specified type
        /// </summary>
        /// <param name="hookType"></param>
        public GlobalHook(HookTypes hookType)
        {
            this.HookType = hookType;
            this.InstallHook();
        }

        ~GlobalHook()
        {
            if (this.Handle != 0)
            {
                this.Unhook();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of this hook
        /// </summary>
        public HookTypes HookType { get; }

        /// <summary>
        /// Gets the handle of the hook
        /// </summary>
        public int Handle { get; private set; }

        #endregion

        #region Event Triggers

        /// <summary>
        /// Raises the <see cref="MouseClick"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (this.MouseClick != null)
            {
                this.MouseClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDoubleClick"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (this.MouseDoubleClick != null)
            {
                this.MouseDoubleClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseWheel"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            if (this.MouseWheel != null)
            {
                this.MouseWheel(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (this.KeyDown != null)
            {
                this.KeyDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyUp"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            if (this.KeyUp != null)
            {
                this.KeyUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyPress"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            if (this.KeyPress != null)
            {
                this.KeyPress(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recieves the actual unsafe mouse hook procedure
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                return WinApi.CallNextHookEx(this.Handle, code, wParam, lParam);
            }
            switch (this.HookType)
            {
                case HookTypes.Mouse:
                    return this.MouseProc(code, wParam, lParam);
                case HookTypes.Keyboard:
                    return this.KeyboardProc(code, wParam, lParam);
                default:
                    throw new Exception("HookType not supported");
            }
        }

        /// <summary>
        /// Recieves the actual unsafe keyboard hook procedure
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyboardProc(int code, IntPtr wParam, IntPtr lParam)
        {
            var hookStruct = (WinApi.KeyboardLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinApi.KeyboardLLHookStruct));

            var msg = wParam.ToInt32();
            var handled = false;

            if (msg == WinApi.WM_KEYDOWN || msg == WinApi.WM_SYSKEYDOWN)
            {
                var e = new KeyEventArgs((Keys)hookStruct.vkCode);
                this.OnKeyDown(e);
                handled = e.Handled;
            }
            else if (msg == WinApi.WM_KEYUP || msg == WinApi.WM_SYSKEYUP)
            {
                var e = new KeyEventArgs((Keys)hookStruct.vkCode);
                this.OnKeyUp(e);
                handled = e.Handled;
            }

            if (msg == WinApi.WM_KEYDOWN && this.KeyPress != null)
            {
                var keyState = new byte[256];
                var buffer = new byte[2];
                WinApi.GetKeyboardState(keyState);
                var conversion = WinApi.ToAscii(hookStruct.vkCode, hookStruct.scanCode, keyState, buffer, hookStruct.flags);

                if (conversion == 1 || conversion == 2)
                {
                    var shift = (WinApi.GetKeyState(WinApi.VK_SHIFT) & 0x80) == 0x80;
                    var capital = WinApi.GetKeyState(WinApi.VK_CAPITAL) != 0;
                    var c = (char)buffer[0];
                    if ((shift ^ capital) && char.IsLetter(c))
                    {
                        c = char.ToUpper(c);
                    }
                    var e = new KeyPressEventArgs(c);
                    this.OnKeyPress(e);
                    handled |= e.Handled;
                }
            }

            return handled ? 1 : WinApi.CallNextHookEx(this.Handle, code, wParam, lParam);
        }

        /// <summary>
        /// Processes Mouse Procedures
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseProc(int code, IntPtr wParam, IntPtr lParam)
        {
            var hookStruct = (WinApi.MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinApi.MouseLLHookStruct));

            var msg = wParam.ToInt32();
            var x = hookStruct.pt.x;
            var y = hookStruct.pt.y;
            int delta = (short)((hookStruct.mouseData >> 16) & 0xffff);

            if (msg == WinApi.WM_MOUSEWHEEL)
            {
                this.OnMouseWheel(new MouseEventArgs(MouseButtons.None, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MOUSEMOVE)
            {
                this.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONDBLCLK)
            {
                this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONDOWN)
            {
                this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONUP)
            {
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
                this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONDBLCLK)
            {
                this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONDOWN)
            {
                this.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONUP)
            {
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONDBLCLK)
            {
                this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONDOWN)
            {
                this.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONUP)
            {
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONDBLCLK)
            {
                this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONDOWN)
            {
                this.OnMouseDown(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONUP)
            {
                this.OnMouseUp(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }

            return WinApi.CallNextHookEx(this.Handle, code, wParam, lParam);
        }

        /// <summary>
        /// Installs the actual unsafe hook
        /// </summary>
        private void InstallHook()
        {
            /// Error check
            if (this.Handle != 0)
            {
                throw new Exception("Hook is already installed");
            }

            #region htype

            var htype = 0;

            switch (this.HookType)
            {
                case HookTypes.Mouse:
                    htype = WinApi.WH_MOUSE_LL;
                    break;
                case HookTypes.Keyboard:
                    htype = WinApi.WH_KEYBOARD_LL;
                    break;
                default:
                    throw new Exception("HookType is not supported");
            }

            #endregion

            /// Delegate to recieve message
            this._HookProc = this.HookProc;

            /// Hook
            /// Ed Obeda suggestion for .net 4.0
            //_hHook = WinApi.SetWindowsHookEx(htype, _HookProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            this.Handle = WinApi.SetWindowsHookEx(htype, this._HookProc, Process.GetCurrentProcess().MainModule.BaseAddress, 0);

            /// Error check
            if (this.Handle == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Unhooks the hook
        /// </summary>
        private void Unhook()
        {
            if (this.Handle != 0)
            {
                //bool ret = WinApi.UnhookWindowsHookEx(Handle);

                //if (ret == false)
                //    throw new Win32Exception(Marshal.GetLastWin32Error());

                //_hHook = 0; 
                try
                {
                    //Fix submitted by Simon Dallmair to handle win32 error when closing the form in vista
                    if (!WinApi.UnhookWindowsHookEx(this.Handle))
                    {
                        var ex = new Win32Exception(Marshal.GetLastWin32Error());
                        if (ex.NativeErrorCode != 0)
                        {
                            throw ex;
                        }
                    }

                    this.Handle = 0;
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion
    }
}