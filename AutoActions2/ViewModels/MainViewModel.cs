using System.ComponentModel;
using System.Reflection;

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
    private readonly IStateFactory _stateFactory;

    public MainViewModel(IKeyboardService keyboardService, IGameModeStateMachine stateMachine,IStateFactory stateFactory)
    {
        _keyboardService = keyboardService;
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _keyboardService.FunctionKeyPressed += OnFunctionKeyPressed;
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

    partial void OnSelectedModeChanged(Mode value) // value = New Mode
    {
        var state = GetStateByModeFromFactory(value);
        _stateMachine.ChangeState(state);
    }

    private IState GetStateByModeFromFactory(Mode mode)
    {
        return mode switch
        {
            Mode.Row => _stateFactory.CreateRowState(),
            Mode.Mine => _stateFactory.CreateMiningState(),
            Mode.StaticMine => _stateFactory.CreateStaticMiningState(),
            _ => _stateFactory.CreateDisabledState(),
        };
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