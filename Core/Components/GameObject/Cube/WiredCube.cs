using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class WiredCube : Cube
{
    public WiredCube(GraphicsDevice graphicsDevice, Color color = new Color(), float size = 1)
    {
        GraphicsDevice = graphicsDevice;
        Size = size;
        Color = color == new Color() ? new Color(0u) : color;
        Verts = new VertexPositionColor[8];
        IndexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), LineCubeIndices.Length, BufferUsage.WriteOnly);
        VertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), Verts.Length, BufferUsage.None);
        Effect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = true,
        };
    }

    protected override void Initialaize()
    {
        float halfSize = Size / 2;
        
        if (Color == new Color(0u))
        {
            Verts[0] = new VertexPositionColor(new Vector3(-halfSize, halfSize, halfSize), Color.Red);
            Verts[1] = new VertexPositionColor(new Vector3(halfSize, halfSize, halfSize), Color.Green);
            Verts[2] = new VertexPositionColor(new Vector3(halfSize, -halfSize, halfSize), Color.Yellow);
            Verts[3] = new VertexPositionColor(new Vector3(-halfSize, -halfSize, halfSize), Color.Blue);

            Verts[4] = new VertexPositionColor(new Vector3(-halfSize, halfSize, -halfSize), Color.Red);
            Verts[5] = new VertexPositionColor(new Vector3(halfSize, halfSize, -halfSize), Color.Green);
            Verts[6] = new VertexPositionColor(new Vector3(halfSize, -halfSize, -halfSize), Color.Yellow);
            Verts[7] = new VertexPositionColor(new Vector3(-halfSize, -halfSize, -halfSize), Color.Blue);
        }
        else
        {
            Verts[0] = new VertexPositionColor(new Vector3(-halfSize, halfSize, halfSize), Color);
            Verts[1] = new VertexPositionColor(new Vector3(halfSize, halfSize, halfSize), Color);
            Verts[2] = new VertexPositionColor(new Vector3(halfSize, -halfSize, halfSize), Color);
            Verts[3] = new VertexPositionColor(new Vector3(-halfSize, -halfSize, halfSize), Color);

            Verts[4] = new VertexPositionColor(new Vector3(-halfSize, halfSize, -halfSize), Color);
            Verts[5] = new VertexPositionColor(new Vector3(halfSize, halfSize, -halfSize), Color);
            Verts[6] = new VertexPositionColor(new Vector3(halfSize, -halfSize, -halfSize), Color);
            Verts[7] = new VertexPositionColor(new Vector3(-halfSize, -halfSize, -halfSize), Color);
        }

        VertexBuffer.SetData(Verts);

        IndexBuffer.SetData(LineCubeIndices);
    }

    protected override void DrawCube(Camera camera)
    {
        foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 12);
        }
    }
}
