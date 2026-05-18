using Autofac;

public class ModelsInstaller : Module
{
    protected override void Load(ContainerBuilder container) => 
        RegisterModels(container);
    private void RegisterModels(ContainerBuilder container)
    {
        /*
        container.RegisterType<ShipModel>().AsSelf().As<IShipHealthProvider>().As<IShipEnergyProvider>().As<IShooteable>().SingleInstance();
        container.RegisterType<CurrentResultProvider>().AsSelf().As<IScoreStringProvider>().As<IResultProvider>().SingleInstance();
        container.RegisterType<LaserDispatcher>().As<ILaserDispatcher>().SingleInstance();
        container.RegisterType<BonusesDispatcher>().As<IBonusesDispatcher>().SingleInstance();
        container.RegisterType<EnemyDispatcher>().As<IEnemyDispatcher>().SingleInstance();
        container.RegisterType<EffectDispatcher>().As<IEffectDispatcher>().SingleInstance();
        container.RegisterType<ShieldDispatcher>().As<IShieldDispatcher>().SingleInstance();
        container.RegisterType<BackgroundStarsDispatcher>().As<IBackgroundStarsDispatcher>().SingleInstance();
        container.RegisterType<GameController>().AsSelf().SingleInstance();
        */
    }
}