using System.Windows.Input;

namespace AutoActions2.StateMachine.States;

public class MiningState(MainViewModel viewModel) : BaseState(viewModel)
{
    public override void Enter()
    {
        ViewModel.Message = "Crouch-Walk-Mine-Mode selected. LShift + W keys will be enabled together with left mouse button.";
        ViewModel.StartButtonEnabled = true;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
        //ViewModel.SelectedMode = MainViewModel.Mode.Mine;
    }

    public override async void StartExecution()
    {
        base.StartExecution();
        await Task.Delay(200); // Add a small delay before each keys are pressed so UI can update

        ViewModel.BackgroundColor = Brushes.Orchid;
        ViewModel.Message = "Crouch Walk Mining Started";

        InputService.PressKey(Key.LeftShift);
        InputService.PressKey(Key.W);
        InputService.PressMouse(MouseButton.Left);
    }

    public override void StopExecution()
    {
        base.StopExecution();
        ViewModel.BackgroundColor = ViewModel.DefaultBackgroundColor;
        ViewModel.Message = "Crouch Walk Mining Ended";
    }
}