public interface IFromStateTransition : IStateTransition
{
    IState From { get; }
}