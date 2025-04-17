using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace AutoActions2.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    public enum Mode
    {
        [Description("Disabled")] None,
        [Description("Walk/BoatRow-Mode")] Row,
        [Description("Crouch-Walk-Mining Mode")] Mine,
        [Description("Mouse Mining Mode")] StaticMine,
    }
    public Brush DefaultBackgroundColor { get;  } = Brushes.LightGray;

    [ObservableProperty] private string _message = string.Empty;
    [ObservableProperty] private Brush _backgroundColor = Brushes.Gray;
    [ObservableProperty] private bool _startButtonEnabled;
    [ObservableProperty] private bool _stopButtonEnabled;
    [ObservableProperty] private bool _modeSelectionEnabled = true;
    [ObservableProperty] private Mode _selectedMode;

    private IKeyboardService _keyboardService;
    private readonly IGameModeStateMachine _stateMachine;
    private readonly IState _disabledState;
    private readonly IState _rowState;
    private readonly IState _miningState;
    private readonly IState _staticMiningState;

    public MainViewModel() 
    {
        _keyboardService = new KeyboardService(Key.F6);
        _keyboardService.FunctionKeyPressed += OnFunctionKeyPressed;


        // Initialize the state machine and states
        _stateMachine = new GameModeStateMachine();
        _disabledState = new DisabledState(this);
        _rowState = new RowState(this);
        _miningState = new MiningState(this);
        _staticMiningState = new StaticMiningState(this);
        _stateMachine.ChangeState(_disabledState);
    }
    
    private void OnFunctionKeyPressed(object? sender, EventArgs e)
    {
        StartStop();
    }

    [RelayCommand]
    private void StartStop()
    {
        _stateMachine.HandleFunctionKeyPress();
    }

    public void Dispose()
    {
        _keyboardService.FunctionKeyPressed -= OnFunctionKeyPressed;
        _keyboardService.Dispose();
    }

    partial void OnSelectedModeChanged(Mode value)
    {
        switch (value)
        {
            case Mode.Row:
                _stateMachine.ChangeState(_rowState);
                break;
            case Mode.Mine:
                _stateMachine.ChangeState(_miningState);
                break;
            case Mode.StaticMine:
                _stateMachine.ChangeState(_staticMiningState);
                break;
            default:
                _stateMachine.ChangeState(_disabledState);
                break;
        }
    }

    /// <summary>
    /// Gets the list of mode values with their descriptions to be used by the ComboBox in ViewModel.
    /// </summary>
    public static object ModeValues
    {
        get
        {
            var list = new List<object>();
            foreach (var mode in Enum.GetValues(typeof(Mode)))
            {
                if (mode != null)
                {
                    var s = mode.ToString();
                    if (s == null) continue;

                    var memberInfo = mode.GetType().GetMember(s).FirstOrDefault();
                    var descAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
                    var desc = descAttribute?.Description ?? "No Description";
                    list.Add(new { Value = mode, Description = desc });
                }
            }
            return list;
        }
    }
}