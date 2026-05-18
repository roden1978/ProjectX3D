public sealed class CameraMoverFactory : IFactory<GameObject>
{
    private readonly Camera _camera;
    private readonly IInputService _input;

    public CameraMoverFactory(Camera camera, IInputService input)
    {
        _camera = camera;
        _input = input;
    }

    public string Name => nameof(CameraMoverFactory);

    public GameObject Create()
    {
        GameObject cameraMoverGameObject = new("CameraMover");

        cameraMoverGameObject.AddComponent(new CameraMover(_camera, _input, free: false));
        
        return cameraMoverGameObject;
    }
}
