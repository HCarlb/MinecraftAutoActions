using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace AutoActions2.Services;

public class KeyboardService : IDisposable
{
    private const int WH_KEYBOARD_LL = 13; // Low-level keyboard hook for global keyboard events
    private const int WM_KEYDOWN = 0x0100; // Key down message for global keyboard events that are not specific to a window
    private const int WM_KEYUP = 0x0101;   // Key up message for global keyboard events that are not specific to a window

    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID = IntPtr.Zero;
    private readonly Key _keyToListenFor;

    public event EventHandler? FunctionKeyPressed;

    public KeyboardService(Key keyToListenFor)
    {
        _keyToListenFor = keyToListenFor;
        _proc = HookCallback;
        _hookID = SetHook(_proc);
    }

    /// <summary>
    ///  Sets a low-level keyboard hook for the current process. It retrieves the main module of the process and installs
    /// the hook.
    /// </summary>
    /// <param name="proc">The function to be called whenever a keyboard event occurs.</param>
    /// <returns>Returns a handle to the hook if successful, otherwise an exception is thrown.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the main module of the current process is null.</exception>
    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        return curModule == null ?
            throw new InvalidOperationException("Current module is null") :
            SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Handles keyboard input events and triggers an action when a specific key is pressed.
    /// </summary>
    /// <param name="nCode">Indicates the status of the hook and whether the event should be processed.</param>
    /// <param name="wParam">Specifies the type of keyboard event, such as key down or key up.</param>
    /// <param name="lParam">Contains information about the key event, including the virtual key code.</param>
    /// <returns>Returns the result of the next hook in the chain.</returns>
    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        //if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))       // Execute on key down or key up
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN)) // Executes on key down
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (vkCode == KeyInterop.VirtualKeyFromKey(_keyToListenFor))
            {
                FunctionKeyPressed?.Invoke(this, EventArgs.Empty);
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        UnhookWindowsHookEx(_hookID);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}
