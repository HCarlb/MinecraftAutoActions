public class DisabledState : IState
{
    private readonly MainViewModel _viewModel;

    public DisabledState(MainViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public bool IsRunning => _isRunning;
    private bool _isRunning = false;

    public void Enter()
    {
        _isRunning = false;
        _viewModel.Message = "Please select a mode";
        _viewModel.BackgroundColor = _viewModel.DefaultBackgroundColor;
        _viewModel.StartButtonEnabled = false;
        _viewModel.StopButtonEnabled = false;
        _viewModel.ModeSelectionEnabled = true;
        _viewModel.SelectedMode = MainViewModel.Mode.None;
    }

    public void Exit()
    { }

    public void HandleFunctionKeyPress()
    { }

    public void StartExecution()
    { }

    public void StopExecution()
    { }
}