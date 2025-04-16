using System.Windows.Input;

public class MiningState : IState
{
    private readonly MainViewModel _viewModel;
    private bool _isRunning = false;
    public bool IsRunning => _isRunning;
    private InputService _inputService;
    public MiningState(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        _inputService = new InputService();
    }

    public void Enter()
    {
        _viewModel.Message = "Mine mode selected. LShift + W keys will be enabled together with left mouse button.";
        _viewModel.StartButtonEnabled = true;
        _viewModel.StopButtonEnabled = false;
        _viewModel.ModeSelectionEnabled = true;
        _viewModel.SelectedMode = MainViewModel.Mode.Mine;
    }

    public void Exit()
    {
        if(IsRunning) StopExecution();
    }

    public void HandleFunctionKeyPress()
    {
        if(IsRunning)
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

        _viewModel.BackgroundColor = Brushes.Orchid;
        _viewModel.Message = "Mining.";

        // Add button presses
        _inputService.PressKey(Key.LeftShift);
        _inputService.PressKey(Key.W);
        _inputService.PressMouse(MouseButton.Left);

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