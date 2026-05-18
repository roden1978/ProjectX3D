using System;
using System.Collections.Generic;
using System.Linq;

public class AnimationController : IDestroy
{
    public Animation CurrentAnimation { get; private set; }
    public bool Active { get; private set; } = true;
    private Dictionary<string, List<AnimationTransition>> _transitions = [];
    private List<AnimationTransition> _currentTransitions = [];
    private List<AnimationTransition> _anyTransitions = [];
    private static readonly List<AnimationTransition> EmptyTransitions = [];
    private readonly Animation[] _animations;
    public AnimationController(Animation[] animations)
    {
        _animations = animations;
    }
    public void Update()
    {
        AnimationTransition transition = GetTransition();
        if (transition.To != string.Empty)
            SetAnimation(transition.To);
    }
    public AnimationController SetAnimation(string name)
    {
        if (CurrentAnimation?.Name == name) return this;

        IEnumerable<Animation> current = _animations.Where(animation => animation.Name.Contains(name));
        CurrentAnimation = current.ToArray()[0];
        _transitions.TryGetValue(CurrentAnimation.Name, out _currentTransitions);

        _currentTransitions ??= EmptyTransitions;

        return this;
    }
    public AnimationController AddTransition(string from, string to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from, out List<AnimationTransition> value) == false)
        {
            value = [];
            _transitions[from] = value;
        }

        value.Add(new AnimationTransition(to, predicate));
        return this;
    }
    public AnimationController AddAnyTransition(string to, Func<bool> predicate)
    {
        _anyTransitions.Add(new AnimationTransition(to, predicate));
        return this;
    }

    private AnimationTransition GetTransition()
    {
        foreach (AnimationTransition transition in _anyTransitions
            .Where(transition => transition.Condition()))
            return transition;


        foreach (var transition in _currentTransitions
            .Where(transition => transition.Condition()))
            return transition;

        return new AnimationTransition("", () => false);
    }

    public void SetActive(bool value) => 
        Active = value;

    public void Destroy()
    {
        CurrentAnimation = null;

        foreach (KeyValuePair<string, List<AnimationTransition>> transitionList in _transitions)
            for (int i = 0; i < transitionList.Value.Count; i++)
            {
                if (transitionList.Value[i] != null)
                {
                    transitionList.Value[i].Destroy();
                    transitionList.Value[i] = null;
                }
            }
        _transitions.Clear();
        _transitions = null;


        for (int i = 0; i < _currentTransitions.Count; i++)
        {
            if (_currentTransitions[i] != null)
            {
                _currentTransitions[i].Destroy();
                _currentTransitions[i] = null;
            }
        }
        _currentTransitions.Clear();
        _currentTransitions = null;

        for (int i = 0; i < _anyTransitions.Count; i++)
        {
            if (_anyTransitions[i] != null)
            {

                _anyTransitions[i].Destroy();
                _anyTransitions[i] = null;
            }
        }
        _anyTransitions.Clear();
        _anyTransitions = null;


        for (int i = 0; i < _animations.Length; i++)
        {
            if (_animations[i] != null)
            {
                _animations[i].Destroy();
                _animations[i] = null;
            }
        }

        EmptyTransitions.Clear();
    }
}