using AutoActions2.Services;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Automation;
using System.Windows.Input;

namespace AutoActions2.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    public enum Mode
    {
        [Description("Disabled")] None,
        [Description("Row/Boat Mode")] Row,
        [Description("Mining Mode")] Mine
    }

    [ObservableProperty] private string _message = string.Empty;
    [ObservableProperty] private Brush _backgroundColor = Brushes.GreenYellow;
    [ObservableProperty] private bool _isStarted = false;
    [ObservableProperty] private bool _canStart = false;
    [ObservableProperty] private Mode _selectedMode = Mode.None;
    private IKeyboardService _keyboardService;
    private IInputService _inputService;

    public MainViewModel() 
    {
        _keyboardService = new KeyboardService(Key.F6);
        _keyboardService.FunctionKeyPressed += OnFunctionKeyPressed;

        _inputService = new InputService();
    }
    
    private void OnFunctionKeyPressed(object? sender, EventArgs e)
    {
        ToggleStartStop();
    }

    private void ToggleStartStop()
    {
        if (SelectedMode==Mode.None) return;

        if (IsStarted)
        {
            Stop();
            return;
        }            

        Start();
    }

    [RelayCommand]
    private void Stop()
    {
        IsStarted = false;
        _inputService.ReleaseAll();
    }

    [RelayCommand]
    private void Start()
    {
        IsStarted = true;

        if (SelectedMode == Mode.Row)
        {
            _inputService.PressKey(Key.W);
        }
        else if (SelectedMode == Mode.Mine)
        {
            _inputService.PressMouse(MouseButton.Left);
            _inputService.PressKey(Key.W);
            _inputService.PressKey(Key.LeftShift);
        }
    }

    partial void OnIsStartedChanged(bool value)
    {
        CanStart =  SelectedMode != Mode.None && !IsStarted;
        BackgroundColor = IsStarted ? Brushes.Red : Brushes.GreenYellow;

    }



    partial void OnSelectedModeChanged(Mode value)
    {
        switch (value)
        {
            case Mode.Row:
                Message = "Row mode selected. W key will be enabled.";
                CanStart = true;
                break;

            case Mode.Mine:
                Message = "Mine mode selected. LShift + W keys will be enabled together with left mouse button.";
                CanStart = true;
                break;

            default:
                Message = "Please select a mode";
                CanStart = false;
                break;
        }
    }
    public void Dispose()
    {
        _inputService.ReleaseAll();
        _keyboardService.FunctionKeyPressed -= OnFunctionKeyPressed;
        _keyboardService.Dispose();
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