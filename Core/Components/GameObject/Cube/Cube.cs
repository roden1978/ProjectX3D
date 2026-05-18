using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Cube : Component
{
    protected GraphicsDevice GraphicsDevice;
    protected float Size;
    protected Color Color;
    protected VertexPositionColor[] Verts;
    protected VertexBuffer VertexBuffer;
    protected IndexBuffer IndexBuffer;
    protected BasicEffect Effect;
    protected float ModelAngle;
    protected readonly short[] TriangleCubeIndices =
            [
                0,1,2, // передняя сторона
                2,3,0,

                6,5,4, // задняя сторона
                4,7,6,

                4,0,3, // левый бок
                3,7,4,

                1,5,6, // правый бок
                6,2,1,

                4,5,1, // вверх
                1,0,4,

                3,2,6, // низ
                6,7,3,
            ];
    protected readonly short[] LineCubeIndices =
            [
                0,1,// передние линии
                1,2,
                2,3,
                3,0,

                4,5, // задние линии
                5,6,
                6,7,
                7,4,

                0,4,// боковые линии
                3,7,
                1,5,
                2,6
            ];

    public override void Start()
    {
        Initialaize();
    }

    public override void Draw(Camera camera)
    {
        InitDraw(camera);
        DrawCube(camera);
    }

    public override void Update(GameTime gameTime)
    {
        UpdateCube(gameTime);
    }

    protected abstract void Initialaize();
    protected abstract void DrawCube(Camera camera);
    protected virtual void InitDraw(Camera camera)
    {
        GraphicsDevice.SetVertexBuffer(VertexBuffer);
        GraphicsDevice.Indices = IndexBuffer;

        Effect.View = camera.View;
        Effect.Projection = camera.Projection;
        Effect.World = gameObject.Transform.Absolute;
    }
    protected virtual void UpdateCube(GameTime gameTime)
    {
        ModelAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * 25.71f;
        gameObject.Transform.Rotation = new Vector3(0f, MathHelper.ToRadians(ModelAngle), 0f);
    }
}
