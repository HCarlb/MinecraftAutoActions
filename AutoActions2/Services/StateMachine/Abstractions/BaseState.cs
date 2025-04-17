namespace AutoActions2.Services.StateMachine.Abstractions;

public abstract class BaseState : IState
{
    protected readonly MainViewModel ViewModel;
    protected readonly IInputService InputService;
    private bool _isRunning;

    protected BaseState(MainViewModel viewModel, IInputService inputService)
    {
        ViewModel = viewModel;
        InputService = inputService;
    }

    public bool IsRunning => _isRunning;

    public virtual void Enter() { }

    public virtual void Exit()
    {
        if (IsRunning) StopExecution();
    }

    public void HandleFunctionKeyPress()
    {
        if (IsRunning)
        {
            StopExecution();
        }
        else
        {
            StartExecution();
        }
    }

    public virtual void StartExecution()
    {
        ViewModel.ModeSelectionEnabled = false;
        ViewModel.StartButtonEnabled = false;
        ViewModel.StopButtonEnabled = true;
        _isRunning = true;
    }

    public virtual void StopExecution()
    {
        ViewModel.StartButtonEnabled = true;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
        InputService.ReleaseAll();
        _isRunning = false;
    }
}
