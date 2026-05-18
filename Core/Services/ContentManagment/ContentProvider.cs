using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ContentProvider : IContentProvider
{
    private readonly IContentLoadService _contentLoadService;
    private ImmutableDictionary<TextureTypes, Texture2D> _textures;
    private ImmutableDictionary<AccessTypes, string> _accessStrings;
    private readonly Dictionary<ModelTypes, Model> _models = [];
    private readonly Random _random;
    private Font _artFont;

    public ContentProvider(IContentLoadService contentLoadService)
    {
        _contentLoadService = contentLoadService;
        _random = new();
    }

    public void LoadAll()
    {
        _textures = _contentLoadService.LoadConvertedTextures();
        _accessStrings = _contentLoadService.LoadConvertedAccessDBStrings();
        _contentLoadService.CreateDefaultTexture2D();
        CreateArtFont();
    }

    private void CreateArtFont()
    {
        _artFont = new ArtFont
        (
            new Sequence(new SpriteOrder(_textures[TextureTypes.Font], Point.Zero, 10, 9, new Rectangle(0, 0, 480, 414)))
        );
    }

    public Texture2D GetTextureByType(TextureTypes type)
    {
        if (_textures.TryGetValue(type, out Texture2D value))
            return value;

        throw new ArgumentException($"Texture type {type} is not exist!");
    }

    public string GetAccessStringByType(AccessTypes type)
    {
        if (_accessStrings.TryGetValue(type, out string value))
            return value;

        throw new ArgumentException($"Texture type {type} is not exist!");
    }

    public int GetRandomIndex() =>
        _random.Next(0, 1);

    public void Start() => 
        LoadAll();

    public Sprite CreateDefaultSprite(int width = 32, int height = 32, Color color = new Color()) => 
        _contentLoadService.CreateDefaultSprite(width, height, color);

    public Sprite GenerateBackground()
    {
        int backgroundSize = Settings.ScreenWidth * Settings.ScreenHeight;
        Color[] background = new Color[backgroundSize];
        for (int i = 0; i < backgroundSize; i++)
        {
            background[i] = _random.Next(0, 100) > 98 ? Color.White : Color.Black;
        }

        Texture2D tex = _contentLoadService.CreateDefaultTexture2D(Settings.ScreenWidth, Settings.ScreenHeight);

        tex.SetData(background);

        return new Sprite(tex);
    }

    public Font GetArtFont()
    {
        return _artFont;
    }

    public SpriteFont GetSpriteFont(string path)
    {
        return _contentLoadService.Load<SpriteFont>(path);
    }
}
