public sealed class PoolOptions
{
    public IFactory<GameObject> Factory { get; }
    public int InitialCapacity { get; }
    public bool Additional { get; }
    public PoolOptions(IFactory<GameObject> factory, int initialCapacity, bool additional = true)
    {
        Factory = factory;
        InitialCapacity = initialCapacity;
        Additional = additional;
    }
}
