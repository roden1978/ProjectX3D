using System;

public interface ITransition
{
    Func<bool> Condition { get; }
}