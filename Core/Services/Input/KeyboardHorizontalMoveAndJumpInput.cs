using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardHorizontalMoveAndJumpInput : IInputService
{
    private bool _isJump;
    public float Jump()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && false == _isJump)
        {
            _isJump = true;
            return -1;
        }

        if (Keyboard.GetState().IsKeyUp(Keys.Space) && _isJump)
            _isJump = false;

        return 0;
    }

    public Vector3 KeyboardMove()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            return new(-1, 0, 0);
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            return new(1, 0, 0);

        return Vector3.Zero;
    }
}
