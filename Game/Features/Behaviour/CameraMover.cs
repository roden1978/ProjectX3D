using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class CameraMover : Component
{
    private const float RotationSpeed = 10f;
    private const int MoveSpeed = 10;
    private const int Forward = 1;
    private const int Backward = -1;
    private const int Left = 1;
    private const int Right = -1;
    private readonly Camera _camera;
    private readonly IInputService _input;
    private readonly bool _free;
    private Vector3 _currentMouse;
    private Vector3 _prevMouseState;
    private float deltaX;
    private float deltaY;
    private float _cameraY;

    public CameraMover(Camera camera, IInputService input, bool free = true)
    {
        _camera = camera;
        _input = input;
        _free = free;
    }

    public override void Start()
    {
        Mouse.SetPosition(Settings.ScreenWidth / 2, Settings.ScreenHeight / 2);
        Mouse.SetCursor(MouseCursor.Crosshair);
        _prevMouseState = _input.MouseMove();
        _cameraY = _camera.Position.Y;
    }

    public override void Update(GameTime gameTime)
    {
        CameraMove(gameTime);
        CameraRotation(gameTime);
    }

    private Vector3 GetKeyboardMove() => _input.KeyboardMove();
    private Vector3 GetMouseMove() => _input.MouseMove();
    private void CameraMove(GameTime gameTime)
    {
        Vector3 direction = GetKeyboardMove();
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

        if (direction.Z == Forward)
            _camera.Position += _camera.Direction * (float)deltaTime * MoveSpeed;
        if (direction.Z == Backward)
            _camera.Position -= _camera.Direction * (float)deltaTime * MoveSpeed;
        if (direction.X == Left)
            _camera.Position -= Vector3.Cross(_camera.Up, _camera.Direction * (float)deltaTime * MoveSpeed);
        if (direction.X == Right)
            _camera.Position += Vector3.Cross(_camera.Up, _camera.Direction * (float)deltaTime * MoveSpeed);
    }

    public void CameraRotation(GameTime gameTime)
    {
        _currentMouse = GetMouseMove();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        deltaX = _currentMouse.X - _prevMouseState.X;
        deltaY = _currentMouse.Y - _prevMouseState.Y;

        _camera.Direction = Vector3.Transform(_camera.Direction,
            Matrix.CreateFromAxisAngle(_camera.Up, -MathHelper.PiOver4 / 150 * deltaX * deltaTime * RotationSpeed));

        _camera.Direction = Vector3.Transform(_camera.Direction, Matrix.CreateFromAxisAngle(Vector3.Cross(_camera.Up, _camera.Direction),
            MathHelper.PiOver4 / 150 * deltaY * deltaTime * RotationSpeed * 2));

        if (false == _free)
            _camera.Position = new Vector3(_camera.Position.X, _cameraY, _camera.Position.Z);
       
        if (MathF.Abs(deltaX) == 0 & MathF.Abs(deltaY) == 0)
        {
            Mouse.SetPosition(Settings.ScreenWidth / 2, Settings.ScreenHeight / 2);
            _prevMouseState = GetMouseMove();
        }
        else
            _prevMouseState = _currentMouse;
    }

}