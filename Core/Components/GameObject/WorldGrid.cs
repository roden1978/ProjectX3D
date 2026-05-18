using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class WorldGrid
{
    private int _size;
    private const int AxisLength = 1000;
    private const int RayPoints = 6;
    private int _currentIndex;

    private readonly GraphicsDevice _graphicsDevice;
    private VertexPositionColor[] _verts;
    private VertexBuffer _vertexBuffer;
    private BasicEffect _effect;
    private int _raysCount;
    private Color _gridColor;

    public WorldGrid(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }
    public void Initialize(Color gridColor = new Color(), int size = 100)
    {
        _verts = new VertexPositionColor[RayPoints + size * 2];
        _gridColor = gridColor == new Color() ? Color.Gray : gridColor;
        _size = size;

        CalculateLinesX();
        CalculateLinesZ();
        CalculateAxis();
        InitialaizeVertexBuffer();
    }

    private void InitialaizeVertexBuffer()
    {
        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), _verts.Length, BufferUsage.None);

        _vertexBuffer.SetData(_verts);

        _effect = new BasicEffect(_graphicsDevice)
        {
            VertexColorEnabled = true
        };

        _graphicsDevice.SetVertexBuffer(_vertexBuffer);
    }

    private void CalculateAxis()
    {
        _verts[_currentIndex] = new VertexPositionColor(new Vector3(-AxisLength, 0, 0), Color.Blue);
        _verts[++_currentIndex] = new VertexPositionColor(new Vector3(AxisLength, 0, 0), Color.Blue);
        _verts[++_currentIndex] = new VertexPositionColor(new Vector3(0, -AxisLength, 0), Color.Red);
        _verts[++_currentIndex] = new VertexPositionColor(new Vector3(0, AxisLength, 0), Color.Red);
        _verts[++_currentIndex] = new VertexPositionColor(new Vector3(0, 0, -AxisLength), Color.Green);
        _verts[++_currentIndex] = new VertexPositionColor(new Vector3(0, 0, AxisLength), Color.Green);
    }

    private void CalculateLinesZ()
    {
        _raysCount = _size / 4;
        while (_currentIndex < _size * 2)
        {
            if (_raysCount == 0)
            {
                _raysCount--;
                continue;
            }
            _verts[_currentIndex] = new VertexPositionColor(new Vector3(-_size / 4, 0, _raysCount), _gridColor);
            _verts[++_currentIndex] = new VertexPositionColor(new Vector3(_size / 4, 0, _raysCount), _gridColor);
            _currentIndex++;
            _raysCount--;
        }
    }

    private void CalculateLinesX()
    {
        int _raysCount = _size / 4;
        while (_currentIndex < _size)
        {
            if (_raysCount == 0)
            {
                _raysCount--;
                continue;
            }
            _verts[_currentIndex] = new VertexPositionColor(new Vector3(_raysCount, 0, -_size / 4), _gridColor);
            _verts[++_currentIndex] = new VertexPositionColor(new Vector3(_raysCount, 0, _size / 4), _gridColor);
            _currentIndex++;
            _raysCount--;
        }
    }

    public void Draw(Camera camera)
    {
        _effect.View = camera.View;
        _effect.Projection = camera.Projection;
        
        foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _verts, 0, _verts.Length / 2);
        }
    }
}