using System.Collections.Generic;
using Microsoft.Xna.Framework;
public sealed class PoolService
{
    private readonly Dictionary<string, IFactory<GameObject>> _factories = [];
    private readonly Dictionary<string, Pool> _pools = [];
    private readonly List<PoolOptions> _poolsOptions = [];
    private readonly Scene _scene;

    public PoolService(Scene scene)
    {
        _scene = scene;
    }
    public void Initialize()
    {
        foreach (PoolOptions poolOptions in _poolsOptions)
        {
            Pool pool = new(poolOptions.InitialCapacity);

            for (int i = 0; i < poolOptions.InitialCapacity; i++)
            {
                GameObject pooledObject = poolOptions.Factory.Create();
                pooledObject.Name = $"{poolOptions.Factory.Name}({i})";
                pooledObject.Transform.Position = ResetPosition();
                pooledObject.SetActive(false);
                pool.AddAdditional(pooledObject);
                pool.Index = i;
                _scene.Register(pooledObject);
            }
            _pools.Add(poolOptions.Factory.Name, pool);
            _factories.Add(poolOptions.Factory.Name, poolOptions.Factory);
        }
    }

    public GameObject GetPooledObject(string name)
    {
        if (_pools.TryGetValue(name, out Pool pool))
        {
            if (false == pool.TryGetPooledObject(out GameObject pooledObject) & pool.Additional)
            {
                int index = pool.Count - 1;
                GameObject additional = _factories[name].Create();
                additional.Name = $"{additional.Name}({++index})";
                additional.Transform.Position = ResetPosition();
                pool.AddAdditional(additional);
                _scene.Register(additional);
                return additional;
            }
            else
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }
        else
        {
            throw new KeyNotFoundException($"Pool with name '{name}' not found");
        }
    }

    public IReadOnlyList<GameObject> GetAllPoolItemsByPoolName(string name)
    {
        if(_pools.TryGetValue(name, out Pool pool))
        return pool.PoolItems;

        throw new KeyNotFoundException($"Pool items with pool name '{name}' not found");
    }

    public static void ReturnToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.Transform.Position = ResetPosition();
        gameObject.Transform.Parent = null;
    }

    public PoolService Add(PoolOptions poolOptions)
    {
        _poolsOptions.Add(poolOptions);
        return this;
    }

    private static Vector3 ResetPosition() => 
        Helper.GetHidenPoolPosition();

    public void CleanUp()
    {
        _poolsOptions.Clear();
        _factories.Clear();
        _pools.Clear();
    }
}