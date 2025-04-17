using System.Windows.Input;

namespace AutoActions2;

public class Bootstrapper
{
    public IServiceProvider? ServiceProvider { get; private set; }
    public Bootstrapper()
    {
        Initialize();
    }
    public void Initialize()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<IKeyboardService, KeyboardService>(sp => new KeyboardService(Key.F6));
        services.AddSingleton<IGameModeStateMachine, GameModeStateMachine>();
        services.AddSingleton<IInputService, InputService>();
        services.AddSingleton<IStateFactory, StateFactory>();

        // Register states
        services.AddTransient<DisabledState>();
        services.AddTransient<RowState>();
        services.AddTransient<MiningState>();
        services.AddTransient<StaticMiningState>();

        // Register ViewModel
        services.AddSingleton<MainViewModel>();

        ServiceProvider = services.BuildServiceProvider();

        // Set the main window's DataContext
        var mainWindow = new MainView
        {
            DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
        };
        mainWindow.Show();
    }
}
