using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyboardWSADDirectionInput : IInputService
{

    public float Jump() { return 0; }
    public Vector3 KeyboardMove()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            return new(-1, 0, 0);
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            return new(1, 0, 0);
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
            return new(0, 1, 0);
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
            return new(0, -1, 0);

        return Vector3.Zero;
    }
}
