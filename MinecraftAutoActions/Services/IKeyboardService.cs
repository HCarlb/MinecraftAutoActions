
namespace AutoActions2.Services;

public interface IKeyboardService : IDisposable
{
    event EventHandler? FunctionKeyPressed;
}