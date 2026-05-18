using System.Collections.Immutable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IContentLoadService
{
    T Load<T>(string assetName);
    Texture2D LoadTexture(string path);
    ImmutableDictionary<TextureTypes, Texture2D> LoadConvertedTextures();
    ImmutableDictionary<AccessTypes, string> LoadConvertedAccessDBStrings();
    Texture2D CreateDefaultTexture2D(int width = 32, int height = 32, Color color = new Color());
    Sprite CreateDefaultSprite(int width = 32, int height = 32, Color color = new Color());
    Model LoadModel(string path);
}