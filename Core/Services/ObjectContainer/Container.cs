using System;
using System.Collections.Generic;
using System.Linq;

public class Container<T> where T : Component
{
    public int Count => _repository.Count;
    private readonly Dictionary<Type, T> _repository = [];

    public T this[int index]
    {
        get
        {
            return _repository.Values.ElementAt(index);
        }
    }

    public K GetComponent<K>() where K : class
    {
        if (false == TryGetComponent(out K value))
            throw new Exception($"Component with type {typeof(K)} not found");

        return value;
    }

    public bool TryGetComponent<K>(out K component) where K : class
    {
        if (false == _repository.TryGetValue(typeof(K), out T value))
        {
            Type type = typeof(K);
            
            foreach (T item in _repository.Values)
            {
                IEnumerable<Type> types = GetParentTypes(item.GetType());
                
                if (types.Contains(type))
                {
                    component = item as K;
                    return true;
                }
            }

            component = default;
            return false;
        }

        component = value as K;

        return true;
    }

    private static IEnumerable<Type> GetParentTypes(Type type)
    {
        if (type == null)
            yield break;

        foreach (Type i in type.GetInterfaces())
            yield return i;

        Type currentBaseType = type.BaseType;

        while (currentBaseType != null)
        {
            yield return currentBaseType;
            currentBaseType = currentBaseType.BaseType;
        }
    }

    public K Register<K>(K component) where K : T
    {
        Type type = component.GetType();
        
        if (_repository.ContainsKey(type))
            throw new Exception("Type is exist");

        _repository[type] = component;
        
        return component;
    }

    public void Unregister<K>(K component) where K : T =>
        _repository.Remove(component.GetType());

    public bool HasAnyIUIComponent() =>
        _repository.Values.Any(x => x is ICanvasComponent);

    public IEnumerable<ICanvasDrawableComponent> GetDraws()
    {
        foreach (T item in _repository.Values)
            if (item is ICanvasDrawableComponent component)
                yield return component;
    }

    public void AwakeComponent()
    {
        foreach (KeyValuePair<Type, T> component in _repository)
            component.Value.Awake();
    }

    public void AwakeComponents()
    {
        foreach (KeyValuePair<Type, T> component in _repository)
            component.Value.Awake();
    }

    public void StartComponents()
    {
        IEnumerable<KeyValuePair<Type, T>> notStarted = _repository.Where(x => x.Value.Started == false);

        foreach (KeyValuePair<Type, T> component in notStarted)
            component.Value.Start();
    }

    public void SetActive(bool value)
    {
        foreach (KeyValuePair<Type, T> component in _repository)
            component.Value.SetActive(value);
    }

    public void DestroyComponents()
    {
        foreach (T behaviour in _repository.Values)
            behaviour.Destroy();

        CleanUp();
    }

    public bool ContainsComponent<C>() => 
        _repository.ContainsKey(typeof(C));

    public void OnCollisionStay(BoxCollider2D collider)
    {
        foreach (T behaviour in _repository.Values)
            behaviour.OnCollisionStay(collider);
    }

    public void OnCollisionExit(BoxCollider2D collider)
    {
        foreach (T behaviour in _repository.Values)
            behaviour.OnCollisionExit(collider);
    }

    public void OnCollisionEnter(BoxCollider2D collider)
    {
        foreach (T behaviour in _repository.Values)
            behaviour.OnCollisionEnter(collider);
    }

    public IInteractable GetInteractable() =>
        _repository.GetValueOrDefault(typeof(IInteractable)) as IInteractable;

    public void CleanUp() =>
        _repository.Clear();
}