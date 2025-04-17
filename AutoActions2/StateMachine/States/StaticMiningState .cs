using System.Windows.Input;

namespace AutoActions2.StateMachine.States;

public class StaticMiningState(MainViewModel viewModel) : BaseState(viewModel)
{
    public override void Enter()
    {
        ViewModel.Message = "Mouse-MineMode selected. Left mouse button enabled.";
        ViewModel.StartButtonEnabled = true;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
        //ViewModel.SelectedMode = MainViewModel.Mode.Mine;
    }

    public override void StartExecution()
    {
        base.StartExecution();
        ViewModel.BackgroundColor = Brushes.LightGoldenrodYellow;
        ViewModel.Message = "Mining Started";
        InputService.PressMouse(MouseButton.Left);
    }

    public override void StopExecution()
    {
        base.StopExecution();
        ViewModel.BackgroundColor = ViewModel.DefaultBackgroundColor;
        ViewModel.Message = "Mining Ended.";
    }
}