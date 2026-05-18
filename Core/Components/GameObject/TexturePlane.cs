using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TexturePlane : Component
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly Texture2D _texture;
    private readonly VertexPositionTexture[] _verts;
    private readonly VertexBuffer _vertexBuffer;

    private readonly BasicEffect _effect;
    private readonly Matrix _world;
    private Vector3 _startPosition;

    public TexturePlane(GraphicsDevice graphicsDevice, Texture2D texture, Matrix? world = null)
    {
        _graphicsDevice = graphicsDevice;
        _texture = texture;
        _world = world ?? Matrix.Identity;
        var halfWidth = texture.Width / 2;

        _verts = new VertexPositionTexture[4];
        _verts[0] = new VertexPositionTexture(new Vector3(-1, 1f, 0), new Vector2(0, 0));
        _verts[1] = new VertexPositionTexture(new Vector3(1f, 1f, 0), new Vector2(1, 0));
        _verts[2] = new VertexPositionTexture(new Vector3(-1, -1f, 0), new Vector2(1, 0));
        _verts[3] = new VertexPositionTexture(new Vector3(1, -1f, 0), new Vector2(1, 1));

        _vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), _verts.Length, BufferUsage.None);

        _vertexBuffer.SetData(_verts);

        _effect = new BasicEffect(graphicsDevice)
        {
            Texture = _texture,
            TextureEnabled = true
        };

    }
    public override void Start()
    {
        _startPosition = gameObject.Transform.Position;
    }
    public override void Draw(Camera camera)
    {
        _graphicsDevice.SetVertexBuffer(_vertexBuffer);
        _effect.CurrentTechnique.Passes[0].Apply();
        _effect.View = camera.View;
        _effect.Projection = camera.Projection;
        _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _verts, 0, 2);
    }

    public override void Update(GameTime gameTime)
    {
        /*
        float modelAngle = (float)gameTime.ElapsedGameTime.TotalSeconds * 25;
        gameObject.Transform.Rotation += new Vector3(0f, modelAngle, 0f);
        gameObject.Transform.Position = new Vector3(
            _startPosition.X + MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds * 1) * 3f, 
            gameObject.Transform.Position.Y,
            _startPosition.Z + MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds * 1) * 10f);
        _effect.World = gameObject.Transform.Local;


        Console.WriteLine(gameObject.Transform);
        */
    }
}
