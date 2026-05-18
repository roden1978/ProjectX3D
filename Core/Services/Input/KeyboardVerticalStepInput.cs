using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardVerticalStepInput : IInputService
{
    private bool _isUp;
    private bool _isDown;
    public float Jump() { return 0; }
    public Vector3 KeyboardMove()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W) && !_isUp)
        {
            _isUp = true;
            return new(0, 1, 0);
        }
        else
            if (Keyboard.GetState().IsKeyDown(Keys.S) && !_isDown)
        {
            _isDown = true;
            return new(0, -1, 0);
        }

        if (Keyboard.GetState().IsKeyUp(Keys.W) && _isUp)
        {
            _isUp = false;
        }
        else 
            if (Keyboard.GetState().IsKeyUp(Keys.S) && _isDown)
        {
            _isDown = false;
        }

        return Vector3.Zero;
    }
}
