using System.Windows.Input;

public class RowState : IState
{
    private readonly MainViewModel _viewModel;
    private bool _isRunning = false;
    public bool IsRunning => _isRunning;
    private InputService _inputService;
    public RowState(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        _inputService = new InputService();
    }

    public void Enter()
    {
        _viewModel.Message = "Row mode selected. W key will be enabled.";
        _viewModel.StartButtonEnabled = true;
        _viewModel.StopButtonEnabled = false;
        _viewModel.ModeSelectionEnabled = true;
        _viewModel.SelectedMode = MainViewModel.Mode.Row;
    }

    public void Exit()
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

    public void StartExecution()
    {
        _viewModel.StartButtonEnabled = false;
        _viewModel.StopButtonEnabled = true;
        _viewModel.ModeSelectionEnabled = false;

        _viewModel.BackgroundColor = Brushes.Aqua;
        _viewModel.Message = "Rowing.";

        // Add button presses
        _inputService.PressKey(Key.W);

        _isRunning = true;
    }

    public void StopExecution()
    {
        _viewModel.StartButtonEnabled = true;
        _viewModel.StopButtonEnabled = false;
        _viewModel.ModeSelectionEnabled = true;

        _viewModel.BackgroundColor = _viewModel.DefaultBackgroundColor;
        _viewModel.Message = "Row mode selected. W key will be enabled.";

        // Clear button presses
        _inputService.ReleaseAll();

        _isRunning = false;
    }
}