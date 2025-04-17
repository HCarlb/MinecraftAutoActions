using AutoActions2.Services.StateMachine.Abstractions;
using System.Windows.Input;

namespace AutoActions2.Services.StateMachine.States;

public class RowState(MainViewModel viewModel, IInputService inputService) : BaseState(viewModel, inputService)
{
    public override void Enter()
    {
        ViewModel.Message = "Walk/Row Mode selected. W key will be enabled.";
        ViewModel.StartButtonEnabled = true;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
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