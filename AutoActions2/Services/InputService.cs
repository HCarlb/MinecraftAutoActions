using System.Runtime.InteropServices;
using System.Windows.Input;

namespace AutoActions2.Services;

public partial class InputService : IInputService
{
    private readonly List<Key> _pressedKeys = new();
    private readonly List<MouseButton> _pressedMouseButtons = new();

    [Flags]
    private enum KeyEventFlags : uint
    {
        KeyDown = 0x0000,
        KeyUp = 0x0002 // Flag for key release
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

    #region Dll Imports

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);


    #endregion Dll Imports

    public void PressKey(Key key)
    {
        if (_pressedKeys.Contains(key)) return; // Key is already pressed

        // Convert WPF Key to Virtual-Key code
        int virtualKey = KeyInterop.VirtualKeyFromKey(key);

        // Simulate key press using WinAPI
        //keybd_event((byte)virtualKey, 0, 0, 0);
        TriggerKeyboardEvent(virtualKey, KeyEventFlags.KeyDown);

        _pressedKeys.Add(key);
    }

    public void PressMouse(MouseButton button)
    {
        if (_pressedMouseButtons.Contains(button)) return; // Button is already pressed

        var mouseEvent = GetMouseEventFlag(button, isPressed: true);
        if (mouseEvent == 0) return; // Invalid button

        TriggerMouseEvent(mouseEvent);
        _pressedMouseButtons.Add(button);
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

    public void ReleaseKey(Key key)
    {
        if (!_pressedKeys.Contains(key)) return; // Key is not pressed

        // Convert WPF Key to Virtual-Key code
        int virtualKey = KeyInterop.VirtualKeyFromKey(key);

        // Simulate key release using WinAPI
        TriggerKeyboardEvent(virtualKey, KeyEventFlags.KeyUp);

        _pressedKeys.Remove(key);
    }
    public void ReleaseMouse(MouseButton button)
    {
        if (!_pressedMouseButtons.Contains(button)) return; // Button is not pressed

        var mouseEvent = GetMouseEventFlag(button, isPressed: false);
        if (mouseEvent == 0) return; // Invalid button

        TriggerMouseEvent(mouseEvent);
        _pressedMouseButtons.Remove(button);
    }
    private static uint GetMouseEventFlag(MouseButton button, bool isPressed)
    {
        return button switch
        {
            MouseButton.Left => isPressed ? (uint)MouseEventFlags.LeftDown : (uint)MouseEventFlags.LeftUp,
            MouseButton.Middle => isPressed ? (uint)MouseEventFlags.MiddleDown : (uint)MouseEventFlags.MiddleUp,
            MouseButton.Right => isPressed ? (uint)MouseEventFlags.RightDown : (uint)MouseEventFlags.RightUp,
            _ => 0 // Invalid button
        };
    }

    private static void TriggerKeyboardEvent(int virtualKey, KeyEventFlags keyEventFlag)
    {
        keybd_event((byte)virtualKey, 0, (uint)keyEventFlag, UIntPtr.Zero);
    }

    private static void TriggerMouseEvent(uint mouseEvent)
    {
        mouse_event(mouseEvent, 0, 0, 0, UIntPtr.Zero);
    }
}