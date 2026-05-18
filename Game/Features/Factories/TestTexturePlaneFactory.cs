using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public sealed class TestTexturePlaneFactory : IFactory<GameObject>
{
    private readonly Camera _camera;
    private readonly IContentProvider _contentProvider;

    public string Name => nameof(TestTexturePlaneFactory);

    public TestTexturePlaneFactory(Camera camera, IContentProvider contentProvider)
    {
        _camera = camera;
        _contentProvider = contentProvider;
    }
    public GameObject Create()
    {
        GameObject texturePlaneTest = new("TexturePlaneTest", new(0f, 0, -10), new(0, 0, 0), new(1, 1, 1));
        Texture2D texture = _contentProvider.CreateDefaultSprite(1000, 1000, Color.Blue).Image;
        texturePlaneTest.AddComponent(new TexturePlane(_camera.GraphicsDevice, texture));

        return texturePlaneTest;
    }
}

