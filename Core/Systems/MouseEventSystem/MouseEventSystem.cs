using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class MouseEventSystem
{
    public bool Active { get; private set; } = true;
    public event EventHandler<MouseEventArgs> PositionUpdate;
    public event EventHandler<MouseEventArgs> ClickUpdate;
    public event EventHandler<MouseEventArgs> StateUpdate;
    private Point _prevPosition;
    private ButtonState _leftButtonState;
    private ButtonState _prevLeftButtonState;
    private readonly MouseEventArgs _mouseEventArgs;
    public MouseEventSystem()
    {
        _mouseEventArgs = new MouseEventArgs
        {
            MouseCurrentPosition = Point.Zero,
            State = ButtonState.Released
        };
    }
    private void UpdatePosition()
    {
        Point currentPosition = GetMousePosition();
        
        if (_prevPosition != currentPosition)
        {
            _mouseEventArgs.MouseCurrentPosition = currentPosition;
            PositionUpdate?.Invoke(this, _mouseEventArgs);
        }

        _prevPosition = currentPosition;
    }

    private static Point GetMousePosition() => new(Mouse.GetState().X, Mouse.GetState().Y);
    public void Update(GameTime gameTime)
    {
        UpdatePosition();
        UpdateClick();
    }
    private void UpdateClick()
    {
        if (Active)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _leftButtonState = ButtonState.Pressed;
                _prevLeftButtonState = _leftButtonState;
                _mouseEventArgs.State = ButtonState.Pressed;
                StateUpdate?.Invoke(this, _mouseEventArgs);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released &
                    _prevLeftButtonState == ButtonState.Pressed)
            {
                _leftButtonState = ButtonState.Released;

                ClickUpdate?.Invoke(this, _mouseEventArgs);

                _mouseEventArgs.State = ButtonState.Released;
                StateUpdate?.Invoke(this, _mouseEventArgs);

                _prevLeftButtonState = _leftButtonState;
            }
        }
    }

    public void SetActive(bool value) => 
        Active = value;
}