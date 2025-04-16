using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace AutoActions2.Services;

public class InputService : IInputService
{
    [Flags]
    private enum InputType : uint
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags]
    private enum MouseEventFlags : uint
    {
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040
    }

    [Flags]
    private enum KeyEventFlags : uint
    {
        KeyDown = 0x0000,
        KeyUp = 0x0002
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public InputType type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public KeyEventFlags dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

    private readonly List<Key> _pressedKeys = new();
    private readonly List<MouseButton> _pressedMouseButtons = new();

    private void SendInputEvent(INPUT input)
    {
        SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
    }

    public void PressKey(Key key)
    {
        if (!_pressedKeys.Contains(key))
        {
            _pressedKeys.Add(key);
            ushort virtualKey = (ushort)KeyInterop.VirtualKeyFromKey(key);
            Debug.WriteLine($"Virtual key for {key}: {virtualKey}");

            var input = new INPUT
            {
                type = InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = virtualKey,
                        wScan = 0,
                        dwFlags = KeyEventFlags.KeyDown,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            var result = SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
            if (result == 0)
            {
                var errorCode = Marshal.GetLastWin32Error();
                Debug.WriteLine($"SendInput failed with error code: {errorCode}");
            }
            else
            {
                Debug.WriteLine($"Key {key} pressed successfully.");
            }
        }
    }

    public void ReleaseKey(Key key)
    {
        if (_pressedKeys.Contains(key))
        {
            _pressedKeys.Remove(key);
            var input = new INPUT
            {
                type = InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)KeyInterop.VirtualKeyFromKey(key),
                        dwFlags = KeyEventFlags.KeyUp
                    }
                }
            };

            SendInputEvent(input);
        }
    }

    public void PressMouse(MouseButton button)
    {
        if (!_pressedMouseButtons.Contains(button))
        {
            _pressedMouseButtons.Add(button);
            var input = new INPUT
            {
                type = InputType.Mouse,
                u = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dwFlags = button switch
                        {
                            MouseButton.Left => (uint)MouseEventFlags.LeftDown,
                            MouseButton.Right => (uint)MouseEventFlags.RightDown,
                            MouseButton.Middle => (uint)MouseEventFlags.MiddleDown,
                            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
                        }
                    }
                }
            };

            SendInputEvent(input);
        }
    }

    public void ReleaseMouse(MouseButton button)
    {
        if (_pressedMouseButtons.Contains(button))
        {
            _pressedMouseButtons.Remove(button);
            var input = new INPUT
            {
                type = InputType.Mouse,
                u = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dwFlags = button switch
                        {
                            MouseButton.Left => (uint)MouseEventFlags.LeftUp,
                            MouseButton.Right => (uint)MouseEventFlags.RightUp,
                            MouseButton.Middle => (uint)MouseEventFlags.MiddleUp,
                            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
                        }
                    }
                }
            };

            SendInputEvent(input);
        }
    }

    public void ReleaseAll()
    {
        foreach (var key in _pressedKeys.ToList())
        {
            ReleaseKey(key);
        }

        foreach (var button in _pressedMouseButtons.ToList())
        {
            ReleaseMouse(button);
        }
    }
}
