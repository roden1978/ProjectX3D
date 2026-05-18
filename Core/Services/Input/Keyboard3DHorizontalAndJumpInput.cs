using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Keyboard3DHorizontalAndJumpInput : IInputService
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
        int leftRight = 0;
        int forwardBackward = 0;
        
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            leftRight = -1;
            
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            leftRight = 1;
            
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            forwardBackward =  1;
            
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            forwardBackward =  -1;
        
        return new Vector3(leftRight, 0, forwardBackward);
    }

    public Vector3 MouseMove() => new(Mouse.GetState().X, Mouse.GetState().Y, 0);
}
