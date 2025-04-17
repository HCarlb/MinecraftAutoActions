public interface IState
{
    void Enter();
    void Exit();
    void HandleFunctionKeyPress();
    void StartExecution();
    void StopExecution();
    bool IsRunning { get; }
}
