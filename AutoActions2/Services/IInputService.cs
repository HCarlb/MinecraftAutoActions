using System.Windows.Input;

namespace AutoActions2.Services;
public interface IInputService
{
    void PressKey(Key key);
    void PressMouse(MouseButton button);
    void ReleaseKey(Key key);
    void ReleaseMouse(MouseButton button);

    void ReleaseAll();
}