namespace AutoActions2.Services.StateMachine.Abstractions;

public interface IGameModeStateMachine
{
    void ChangeState(IState newState);
    void HandleFunctionKeyPress();
    void StartExecution();
    void StopExecution();
}