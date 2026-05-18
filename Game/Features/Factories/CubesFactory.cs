using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public sealed class CubesFactory : IFactory<GameObject>
{
    private readonly Camera _camera;
    private readonly IContentProvider _contentProvider;

    public string Name => nameof(CubesFactory);

    public CubesFactory(Camera camera, IContentProvider contentProvider)
    {
        _camera = camera;
        _contentProvider = contentProvider;
    }
    public GameObject Create()
    {
        GameObject cubes = new("Cubes");
        CreateWiredCube(cubes.Transform);
        CreateColoredCube(cubes.Transform);
        CreateTexturedCube(cubes.Transform);
        cubes.AddComponent(new CubeRotation());
        return cubes;
    }

    private GameObject CreateWiredCube(Transform parent)
    {
        GameObject wiredCube = new("WiredCube", new Vector3(-3, .5f, 0), Vector3.Zero, Vector3.One, parent);
        wiredCube.AddComponent(new WiredCube(_camera.GraphicsDevice, Color.Red));

        return wiredCube;
    }

    private GameObject CreateColoredCube(Transform parent)
    {
        GameObject coloredCube = new("ColoredCube", new Vector3(0, .5f, 0), Vector3.Zero, Vector3.One, parent);
        coloredCube.AddComponent(new ColoredCube(_camera.GraphicsDevice));

        return coloredCube;
    }

    private GameObject CreateTexturedCube(Transform parent)
    {
        Texture2D texture = _contentProvider.GetTextureByType(TextureTypes.ZeroGround);
        GameObject texturedCube = new("TexturedCube", new Vector3(3, .5f, 0), Vector3.Zero, Vector3.One, parent);
        texturedCube.AddComponent(new TexturedCube(_camera.GraphicsDevice, texture));

        return texturedCube;
    }
}

public class CubeRotation : Component
{
    private float _modelAngle;
    public override void Update(GameTime gameTime)
    {
        _modelAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * 25.71f;
        gameObject.Transform.Rotation = new Vector3(0f, MathHelper.ToRadians(_modelAngle), 0f);
    }
}

