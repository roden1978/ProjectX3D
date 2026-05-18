using System;

public class FromStateTransition : IFromStateTransition
{
    public IState From {get;}

    public IState To {get;}

    public Func<bool> Condition {get;}

    public FromStateTransition(IState from, IState to, Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }
}