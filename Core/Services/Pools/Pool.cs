using System.Collections.Generic;

public sealed class Pool
{
    public IReadOnlyList<GameObject> PoolItems => _repository;
    public int Count => _repository.Count;
    public int Capacity { get; }
    public bool Additional { get; }
    public int Index;
    private readonly List<GameObject> _repository = [];

    public Pool(int capacity, bool additional = true)
    {
        Capacity = capacity;
        Additional = additional;
    }

    public bool TryGetPooledObject(out GameObject gameObject)
    {
        for (int i = 0; i < _repository.Count; i++)
        {
            if (_repository[i].Active == false)
            {
                gameObject = _repository[i];
                return true;
            }
        }

        gameObject = default;

        return false;
    }

    public void AddAdditional(GameObject additional) => 
        _repository.Add(additional);
}