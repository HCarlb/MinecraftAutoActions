namespace AutoActions2.Services.StateMachine;
public class StateFactory : IStateFactory
{
    private readonly IServiceProvider _serviceProvider;

    public StateFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DisabledState CreateDisabledState() => _serviceProvider.GetRequiredService<DisabledState>();
    public RowState CreateRowState() => _serviceProvider.GetRequiredService<RowState>();
    public MiningState CreateMiningState() => _serviceProvider.GetRequiredService<MiningState>();
    public StaticMiningState CreateStaticMiningState() => _serviceProvider.GetRequiredService<StaticMiningState>();
}
