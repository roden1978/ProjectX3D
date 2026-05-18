using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IContentProvider : IStartable
{
    void LoadAll();
    Texture2D GetTextureByType(TextureTypes type);
    string GetAccessStringByType(AccessTypes type);
    int GetRandomIndex();
    Sprite CreateDefaultSprite(int width = 32, int height = 32, Color color = new Color());
    Sprite GenerateBackground();
    Font GetArtFont();
    SpriteFont GetSpriteFont(string name);
}
