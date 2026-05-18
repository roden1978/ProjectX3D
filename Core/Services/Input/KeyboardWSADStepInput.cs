using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardWSADStepInput : IInputService
{
    private bool _isUp;
    private bool _isDown;

    private bool _isLeft;
    private bool _isRight;
    public float Jump() { return 0; }
    public Vector3 KeyboardMove()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A) && false == _isLeft)
        {
            _isLeft = true;
            return new(-1, 0, 0);
        }else if(Keyboard.GetState().IsKeyDown(Keys.D) && false == _isRight)
        {
            _isRight = true;
            return new(1, 0, 0);
        }
        else if(Keyboard.GetState().IsKeyDown(Keys.W) && false == _isUp)
        {
            _isUp = true;
            return new(0, 1, 0);
        }
        else if(Keyboard.GetState().IsKeyDown(Keys.S) && false == _isDown)
        {
            _isDown = true;
            return new(0, -1, 0);
        }

        if (Keyboard.GetState().IsKeyUp(Keys.A) && _isLeft)
        {
            _isLeft = false;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.D) && _isRight)
        {
            _isRight = false;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.W) && _isUp)
        {
            _isUp = false;
        }else if (Keyboard.GetState().IsKeyUp(Keys.S) && _isDown)
        {
            _isDown = false;
        }

        return Vector3.Zero;
    }
}
