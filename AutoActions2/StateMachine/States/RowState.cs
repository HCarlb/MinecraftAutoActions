using System.Windows.Input;

namespace AutoActions2.StateMachine.States;

public class RowState(MainViewModel viewModel) :  BaseState(viewModel)
{
    public override void Enter()
    {
        ViewModel.Message = "Walk/Row Mode selected. W key will be enabled.";
        ViewModel.StartButtonEnabled = true;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
        //ViewModel.SelectedMode = MainViewModel.Mode.Row;
    }

    public override void StartExecution()
    {
        base.StartExecution();
        ViewModel.BackgroundColor = Brushes.Aqua;
        ViewModel.Message = "Walking / Rowing Started";
        InputService.PressKey(Key.W);
    }

    public override void StopExecution()
    {
        base.StopExecution();
        ViewModel.BackgroundColor = ViewModel.DefaultBackgroundColor;
        ViewModel.Message = "Walking / Rowing Ended";
    }
}