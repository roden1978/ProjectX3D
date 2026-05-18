public interface IStateTransition : ITransition
{
    IState To { get; }
}
