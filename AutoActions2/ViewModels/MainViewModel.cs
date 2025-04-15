using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace AutoActions2.ViewModels;

public partial class MainViewModel : ObservableObject
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

    public MainViewModel() {}

    [RelayCommand]
    private void Stop()
    {
        IsStarted = false;
    }

    [RelayCommand]
    private void Start()
    {
        IsStarted = true;
    }

    partial void OnIsStartedChanged(bool value)
    {
        CanStart =  SelectedMode != Mode.None && !IsStarted;
        BackgroundColor = IsStarted ? Brushes.Red : Brushes.GreenYellow;
        //Message = IsStarted ? "Started" : "Stopped";
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