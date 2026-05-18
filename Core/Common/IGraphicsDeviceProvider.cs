using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public interface IGraphicsDeviceProvider
{
    GraphicsDevice GraphicsDevice { get; }
}

public interface IContentManagerProvider
{
    ContentManager Content {get; }
}
