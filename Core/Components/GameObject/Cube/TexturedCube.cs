using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TexturedCube : Cube
{
    private readonly VertexPositionTexture[] _verts;
    public TexturedCube(GraphicsDevice graphicsDevice, Texture2D texture, float size = 1)
    {
        GraphicsDevice = graphicsDevice;
        _verts = new VertexPositionTexture[8];
        Size = size;
        IndexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), TriangleCubeIndices.Length, BufferUsage.WriteOnly);
        VertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), _verts.Length, BufferUsage.None);
        Effect = new BasicEffect(GraphicsDevice)
        {
            TextureEnabled = true,
            Texture = texture
        };
    }
   
    protected override void Initialaize()
    {
        float halfSize = Size / 2;

        _verts[0] = new VertexPositionTexture(new Vector3(-halfSize, halfSize, halfSize), new Vector2(0, 0));
        _verts[1] = new VertexPositionTexture(new Vector3(halfSize, halfSize, halfSize), new Vector2(1, 0));
        _verts[2] = new VertexPositionTexture(new Vector3(halfSize, -halfSize, halfSize), new Vector2(1, 1));
        _verts[3] = new VertexPositionTexture(new Vector3(-halfSize, -halfSize, halfSize), new Vector2(0, 1));
        
        _verts[4] = new VertexPositionTexture(new Vector3(-halfSize, halfSize, -halfSize), new Vector2(0, 0));
        _verts[5] = new VertexPositionTexture(new Vector3(halfSize, halfSize, -halfSize), new Vector2(1, 0));
        _verts[6] = new VertexPositionTexture(new Vector3(halfSize, -halfSize, -halfSize), new Vector2(1, 1));
        _verts[7] = new VertexPositionTexture(new Vector3(-halfSize, -halfSize, -halfSize), new Vector2(0, 1));

        VertexBuffer.SetData(_verts);

        IndexBuffer.SetData(TriangleCubeIndices);
    }

    protected override void DrawCube(Camera camera)
    {
        foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _verts, 0, _verts.Length, TriangleCubeIndices, 0, 12);
        }
    }
}