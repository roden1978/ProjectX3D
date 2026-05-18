using Autofac;

public class FactoryIstaller : Module
{
    protected override void Load(ContainerBuilder container)
    {
        RegisterGameFactories(container);
        RegisterUIFactories(container);

        container.RegisterType<GameFactory>().AsSelf().As<IStartable>().SingleInstance();
    }

    private void RegisterGameFactories(ContainerBuilder container)
    {
        container.RegisterType<TestTexturePlaneFactory>().AsSelf().SingleInstance();
        container.RegisterType<CameraMoverFactory>().AsSelf().SingleInstance();
        container.RegisterType<CubesFactory>().AsSelf().SingleInstance();
        container.RegisterType<TitleFactory>().AsSelf().SingleInstance();
        /*

        container.RegisterType<ShipFactory>().AsSelf().SingleInstance();

        container.RegisterType<GreenLaserFactory>().AsSelf().SingleInstance();
        container.RegisterType<RedLaserFactory>().AsSelf().SingleInstance();
        container.RegisterType<BlueLaserFactory>().AsSelf().SingleInstance();
        container.RegisterType<EnemyLaserFactory>().AsSelf().SingleInstance();

        container.RegisterType<HealthPillFactory>().AsSelf().SingleInstance();
        container.RegisterType<EnergyPillFactory>().AsSelf().SingleInstance();
        container.RegisterType<ShieldFactory>().AsSelf().SingleInstance();
        container.RegisterType<RedGunUpFactory>().AsSelf().SingleInstance();
        container.RegisterType<BlueGunUpFactory>().AsSelf().SingleInstance();
        container.RegisterType<AlienFactory>().AsSelf().SingleInstance();
        container.RegisterType<ShieldUpFactory>().AsSelf().SingleInstance();

        container.RegisterType<GreenGunFactory>().AsSelf().SingleInstance();
        container.RegisterType<RedGunFactory>().AsSelf().SingleInstance();
        container.RegisterType<BlueGunFactory>().AsSelf().SingleInstance();

        container.RegisterType<AsteroidsFactory>().AsSelf().SingleInstance();
        container.RegisterType<UfoFactory>().AsSelf().SingleInstance();

        container.RegisterType<BackgroundStarsFactory>().AsSelf().SingleInstance();

        container.RegisterType<ExplosionFactory>().AsSelf().SingleInstance();
        container.RegisterType<ShipExplosionFactory>().AsSelf().SingleInstance();
        container.RegisterType<BloodEffectFactory>().AsSelf().SingleInstance();
        container.RegisterType<GreenLaserFlashFactory>().AsSelf().SingleInstance();
        container.RegisterType<RedLaserFlashFactory>().AsSelf().SingleInstance();
        container.RegisterType<BlueLaserFlashFactory>().AsSelf().SingleInstance();
        container.RegisterType<HealthBoosterEffectFactory>().AsSelf().SingleInstance();
        container.RegisterType<EnergyBoosterEffectFactory>().AsSelf().SingleInstance();
        container.RegisterType<BurstFactory>().AsSelf().SingleInstance();
        container.RegisterType<DispatchersObserverFactory>().AsSelf().SingleInstance();
        container.RegisterType<GameControllerFactory>().AsSelf().SingleInstance();
        */
    }

    private void RegisterUIFactories(ContainerBuilder container)
    {
        /*
        container.RegisterType<ScoreLabelFactory>().AsSelf().SingleInstance();
        container.RegisterType<ScoreTextFactory>().AsSelf().SingleInstance();
        container.RegisterType<HealthBarFactory>().AsSelf().SingleInstance();
        container.RegisterType<EnergyBarFactory>().AsSelf().SingleInstance();
        container.RegisterType<HUDFactory>().AsSelf().SingleInstance();

        container.RegisterType<LeaderboardFactory>().AsSelf().SingleInstance();
        container.RegisterType<StartButtonFactory>().AsSelf().SingleInstance();
        container.RegisterType<GameOverLabelFactory>().AsSelf().SingleInstance();
        container.RegisterType<TitleFactory>().AsSelf().SingleInstance();
        container.RegisterType<InstructionLabelFactory>().AsSelf().SingleInstance();
        container.RegisterType<StartScreenFactory>().AsSelf().SingleInstance();
        */
    }
}
