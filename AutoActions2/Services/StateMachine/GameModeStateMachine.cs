namespace AutoActions2.Services.StateMachine;

public class GameModeStateMachine : IGameModeStateMachine
{
    private IState? _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void HandleFunctionKeyPress()
    {
        _currentState?.HandleFunctionKeyPress();
    }

    public void StartExecution()
    {
        _currentState?.StartExecution();
    }

    public void StopExecution()
    {
        _currentState?.StopExecution();
    }

}
