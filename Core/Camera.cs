using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera : GameComponent
{
    private const float FieldOfView = MathHelper.Pi / 3; //60 deegre over MathHelper.PiOver2 - 90 degree
    private const float PerspectiveFarPlaneDistance = 1000f;
    private const float PerspectiveNearPlaneDistance = .5f;
    private const float OrthographicFarDistance = -1; 
    public Matrix View { get; private set; }
    public Matrix Projection { get; private set; }
    public GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public CameraTypes CameraType { get; private set; }
    public BasicEffect SpriteBatchEffect { get; private set; }
    public Vector3 Position {get => _position; set => _position = value;}
    public Vector3 Direction {get => _direction; set => _direction = value;}
    public Vector3 Up {get => _up; set => _up = value;}
    private readonly GameWindow _window;
    private Vector3 _position;
    private Vector3 _target;
    private Vector3 _up;
    private Vector3 _direction;
    

    public Camera(
            GraphicsDevice graphicsDevice,
            Game game,
            CameraTypes cameraType = CameraTypes.Perspective,
            Vector3 position = new Vector3(),
            Vector3 target = new Vector3(),
            Vector3 up = new Vector3()
            ) : base(game)
    {
        GraphicsDevice = graphicsDevice;
        CameraType = cameraType;
        _window = game.Window;
        _position = position;
        _target = target;
        _up = up == Vector3.Zero ? Vector3.Up : up;
        _direction = Vector3.Normalize(_target - position);
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteBatchEffect = new BasicEffect(GraphicsDevice);
    }

    public override void Initialize()
    {
        if (_up == Vector3.Zero)
            _up = Vector3.Up;

        SetView();
        SetPojectionMatrix();

        if (CameraType == CameraTypes.Orthographic)
        {
            _position = Vector3.Zero;
            SpriteBatchEffect.TextureEnabled = true;
            SpriteBatchEffect.View = View;
            SpriteBatchEffect.Projection = Projection;
        }
    }

    public override void Update(GameTime gameTime) => SetView();

    public void Display(BasicEffect effect)
    {
        effect.View = View;
        effect.Projection = Projection;
        effect.TextureEnabled = true;
    }

    private void SetView()
    {
        View = CameraType == CameraTypes.Perspective
            ? Matrix.CreateLookAt(_position, _position + _direction, _up)
            : Matrix.CreateLookAt(_position, _position + Vector3.Forward, _up);
    }

    private void SetPojectionMatrix()
    {
        float screenAspect = _window.ClientBounds.Width / (float)_window.ClientBounds.Height;
        Projection = CameraType == CameraTypes.Perspective
        ? Matrix.CreatePerspectiveFieldOfView(FieldOfView, screenAspect, PerspectiveNearPlaneDistance, PerspectiveFarPlaneDistance) 
        : Matrix.CreateOrthographic(_window.ClientBounds.Width, _window.ClientBounds.Height, 0f, OrthographicFarDistance);
    }
}

public enum CameraTypes
{
    Perspective = 0,
    Orthographic = 1
}