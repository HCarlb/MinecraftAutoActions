namespace AutoActions2.Services.StateMachine.States;

public class DisabledState(MainViewModel viewModel, IInputService inputService) : BaseState(viewModel, inputService)
{
    public override void Enter()
    {
        ViewModel.Message = "Please select a mode";
        ViewModel.BackgroundColor = ViewModel.DefaultBackgroundColor;
        ViewModel.StartButtonEnabled = false;
        ViewModel.StopButtonEnabled = false;
        ViewModel.ModeSelectionEnabled = true;
        //ViewModel.SelectedMode = MainViewModel.Mode.None;
    }

    public override void StartExecution()
    {
        // This state does not support execution
    }

    public override void StopExecution()
    {
        // This state does not support execution
    }

    public override void Exit()
    {
        // No specific exit logic for the disabled state
    }
}