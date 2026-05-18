using Autofac;

public class BehaviourInstaller : Module
{
    protected override void Load(ContainerBuilder container) => 
        RegisterBehaviour(container);

    private void RegisterBehaviour(ContainerBuilder container)
    {
        /*
        container.RegisterType<Ship>().AsSelf().As<IGunProvider>().As<IPositionAdapter>().As<IShipActivator>().SingleInstance();
        container.RegisterType<DispatchersObserver>().AsSelf().SingleInstance();
        container.RegisterType<HUD>().AsSelf().SingleInstance();
        container.RegisterType<StartScreenBehaviour>().AsSelf().SingleInstance();
        container.RegisterType<GameController>().AsSelf().SingleInstance();
        */
    }
}
