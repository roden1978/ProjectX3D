using System.Collections.Generic;
using Autofac;

public class GameFactory : IFactory<IReadOnlyList<GameObject>>, IStartable
{
    private readonly PoolService _poolService;
    private readonly TestTexturePlaneFactory _testTexturePlaneFactory;
    private readonly CameraMoverFactory _cameraMoverFactory;
    private readonly CubesFactory _cubesFactory;
    private readonly TitleFactory _titleFactory;

    public string Name => GetType().Name;

    public GameFactory(PoolService poolService,
    TestTexturePlaneFactory testTexturePlaneFactory,
    CameraMoverFactory cameraMoverFactory,
    CubesFactory cubesFactory,
    TitleFactory titleFactory)
    {
        _poolService = poolService;
        _testTexturePlaneFactory = testTexturePlaneFactory;
        _cameraMoverFactory = cameraMoverFactory;
        _cubesFactory = cubesFactory;
        _titleFactory = titleFactory;
    }
    public IReadOnlyList<GameObject> Create()
    {
        GameObject testTexturePlane = _testTexturePlaneFactory.Create();
        GameObject cameraMover = _cameraMoverFactory.Create();
        GameObject cubes = _cubesFactory.Create();
        GameObject title = _titleFactory.Create();
        return [cubes, cameraMover, title];
    }

    public void Start() =>
        Initialize();
    private void Initialize()
    {
        //Example
        //_poolService.Add(new PoolOptions(_asteroidsFactory, 3));

        _poolService.Initialize();
    }

}
