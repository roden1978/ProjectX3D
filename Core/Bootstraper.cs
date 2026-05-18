using System.Runtime.CompilerServices;
using Autofac;
using Microsoft.Xna.Framework;


public class Bootstraper : DrawableGameComponent
{
    private readonly Game _game;
    private readonly ContainerBuilder _container;
    private readonly MouseEventSystem _mouseEventSystem;
    private readonly Camera _camera;
    private readonly Scene _scene;
    private readonly CollisionSystem _collisionSystem;
    private readonly PoolService _poolService;

    public Bootstraper(Game game) : base(game)
    {
        _game = game;
        _container = new ContainerBuilder();
        _mouseEventSystem = new MouseEventSystem();
        Canvas canvas = new(_mouseEventSystem,
            new Camera(game.GraphicsDevice, game, CameraTypes.Orthographic),
            "Canvas", Settings.ScreenWidth, Settings.ScreenHeight);
        _camera = new Camera(game.GraphicsDevice, game, CameraTypes.Perspective, new(1, 2, 10));
        _scene = new("Scene", canvas, _camera, true);
        _collisionSystem = new(_scene, new CollisionMatrix());
        _poolService = new(_scene);
    }
    public override void Draw(GameTime gameTime) => 
        _scene.Draw();

    public override void Update(GameTime gameTime)
    {
        _scene.Update(gameTime);
        _collisionSystem.Update(gameTime);
    }

    public override void Initialize()
    {
        InitializeContainer();
        InitializeCamera();
        InitializeScene();
    }
    private void InitializeCamera()
    {
        _game.Components.Add(_camera);
        _camera.Initialize();
    }
    private void InitializeScene()
    {
        _scene.Initialize();
    }

    private void InitializeContainer()
    {
        _container.RegisterInstance(_mouseEventSystem).SingleInstance();
        _container.RegisterInstance(_game).As<IGraphicsDeviceProvider>().SingleInstance();
        _container.RegisterInstance(_game).As<IContentManagerProvider>().SingleInstance();
        _container.RegisterInstance(_camera).SingleInstance();
        _container.RegisterInstance(_scene).SingleInstance();
        _container.RegisterInstance(_collisionSystem).SingleInstance();
        _container.RegisterInstance(_poolService).SingleInstance();
        _container.RegisterModule(new ServicesInstaller());
        _container.RegisterModule(new ModelsInstaller());
        _container.RegisterModule(new FactoryIstaller());
        _container.RegisterModule(new BehaviourInstaller());
        _container.RegisterType<GameInitializer>().AsSelf().As<IStartable>().SingleInstance();

        _container.Build().BeginLifetimeScope();
    }

    public void CleanUp()
    {
        _poolService.CleanUp();
        _collisionSystem.CleanUp();
        _scene.CleanUp();
    }

}
