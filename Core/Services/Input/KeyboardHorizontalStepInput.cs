using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardHorizontalStepInput : IInputService
{
    private bool _isLeft;
    private bool _isRight;
    public float Jump() { return 0; }
    public Vector3 KeyboardMove()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A) && false == _isLeft)
        {
            _isLeft = true;
            return new(-1, 0, 0);
        }
        else 
            if(Keyboard.GetState().IsKeyDown(Keys.D) && false == _isRight)
        {
            _isRight = true;
            return new(1, 0, 0);
        }

        if (Keyboard.GetState().IsKeyUp(Keys.A) && _isLeft)
        {
            _isLeft = false;
        }
        else 
            if (Keyboard.GetState().IsKeyUp(Keys.D) && _isRight)
        {
            _isRight = false;
        }

        return Vector3.Zero;
    }
}
