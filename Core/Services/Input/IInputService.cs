using Microsoft.Xna.Framework;

public interface IInputService
{
    public float Jump();
    public Vector3 KeyboardMove();
    public Vector3 MouseMove() => Vector3.Zero;
}