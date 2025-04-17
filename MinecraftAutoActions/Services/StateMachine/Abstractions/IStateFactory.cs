namespace AutoActions2.Services.StateMachine.Abstractions;
public interface IStateFactory
{
    DisabledState CreateDisabledState();
    RowState CreateRowState();
    MiningState CreateMiningState();
    StaticMiningState CreateStaticMiningState();
}
