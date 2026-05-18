using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ContentLoadService : IContentLoadService
{
    private const byte BytesForPixel = 4;
    private readonly List<int> _namesLength = [];
    private readonly List<TextureTypes> _textureTypes = [];
    private readonly List<AccessTypes> _accessTypes = [];
    private readonly List<int> _widths = [];
    private readonly List<int> _heigths = [];
    private readonly List<int> _stringsLenght = [];
    private readonly IGraphicsDeviceProvider _graphicsDeviceProvider;
    private readonly IContentManagerProvider _contentManagerProvider;

    public ContentLoadService(IGraphicsDeviceProvider graphicsDeviceProvider, IContentManagerProvider contentManagerProvider)
    {
        _graphicsDeviceProvider = graphicsDeviceProvider;
        _contentManagerProvider = contentManagerProvider;
    }

    
    public Model LoadModel(string path)
    {
        
        return default;
    }

    public T Load<T>(string assetName)
    {
        return _contentManagerProvider.Content.Load<T>($@"{assetName}");
    }

    /// <summary>
    /// Loaded texture from file
    /// </summary>
    /// <param name="path">Path to file</param>
    /// <returns>Return Textur2D</returns>
    public Texture2D LoadTexture(string path)
    {
        Texture2D texture2D = default;

        using (FileStream stream = new(path, FileMode.Open))
            texture2D = Texture2D.FromStream(_graphicsDeviceProvider.GraphicsDevice, stream);

        return texture2D;
    }

    /// <summary>
    /// Load textures from file content.dat converted Texture Converter app
    /// </summary>
    /// <returns>Return immutable dictionary of TextureTypes as key and Texture2D as value</returns>
    public ImmutableDictionary<TextureTypes, Texture2D> LoadConvertedTextures()
    {
        Dictionary<TextureTypes, Texture2D> textures = [];

        if (File.Exists("content.dat"))
        {
            using FileStream stream = File.Open("content.dat", FileMode.Open);
            using GZipStream decompressionStream = new(stream, CompressionMode.Decompress);
            using BinaryReader reader = new(decompressionStream, Encoding.ASCII, false);
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                int lenght = reader.ReadInt32();
                _namesLength.Add(lenght);
            }

            for (int i = 0; i < count; i++)
            {
                string[] name = new string[count];
                name[i] = reader.ReadString();
                _textureTypes.Add((TextureTypes)Enum.Parse(typeof(TextureTypes), name[i]));
            }

            for (int i = 0; i < count; i++)
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                _widths.Add(width);
                _heigths.Add(height);
            }

            for (int i = 0; i < count; i++)
            {
                byte[] tex = reader.ReadBytes(_widths[i] * _heigths[i] * BytesForPixel);

                Texture2D newTexture = new(_graphicsDeviceProvider.GraphicsDevice, _widths[i], _heigths[i], false, SurfaceFormat.Color);
                newTexture.SetData(tex);

                textures.Add(_textureTypes[i], newTexture);
            }
        }

        return textures.ToImmutableDictionary();
    }

    /// <summary>
    /// Load connections strings from file connection.dat
    /// </summary>
    /// <returns>Return the immutable dictionary of AccessTypes as key and String as value</returns>
    public ImmutableDictionary<AccessTypes, string> LoadConvertedAccessDBStrings()
    {
        Dictionary<AccessTypes, string> accessStrings = [];


        if (File.Exists("connection.dat"))
        {
            using FileStream stream = File.Open("connection.dat", FileMode.Open);
            using BinaryReader reader = new(stream, Encoding.ASCII, false);
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                int lenght = reader.ReadInt32();
                _namesLength.Add(lenght);
            }

            for (int i = 0; i < count; i++)
            {
                string[] name = new string[count];
                name[i] = reader.ReadString();
                _accessTypes.Add((AccessTypes)Enum.Parse(typeof(AccessTypes), name[i]));
            }

            for (int i = 0; i < count; i++)
            {
                int lenght = reader.ReadInt32();
                _stringsLenght.Add(lenght);
            }

            for (int i = 0; i < count; i++)
            {
                int[] result = [];
                for (int j = 0; j < _stringsLenght[i]; j++)
                {
                    int value = reader.ReadInt32();
                    result = [.. result, value];
                }
                byte[] bytes = result.Select(x => Convert.ToByte(~x)).ToArray();
                string str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                accessStrings.Add(_accessTypes[i], str);
            }
        }

        return accessStrings.ToImmutableDictionary();
    }

    /// <summary>
    /// Create default Texture2D specified width, height and color
    /// </summary>
    /// <param name="width">Width of texture in px, default 32px</param>
    /// <param name="height">Height of texture in px, , default 32px</param>
    /// <param name="color">Color, default (Color.White)</param>
    /// <returns>New Texture2D</returns>
    public Texture2D CreateDefaultTexture2D(int width = 32, int height = 32, Color color = new Color())
    {
        if (color == new Color())
            color = Color.White;
        IEnumerable<Color> tex = new Color[width * height].Select(x => x = color);
        Texture2D newTexture = new(_graphicsDeviceProvider.GraphicsDevice, width, height, false, SurfaceFormat.Color);
        newTexture.SetData(tex.ToArray());

        return newTexture;
    }

    /// <summary>
    /// Create default Sprite specified width, height and color
    /// </summary>
    /// <param name="width">Width of sprite in px, default 32px</param>
    /// <param name="height">Height of sprite in px, , default 32px</param>
    /// <param name="color">Color, default (Color.White)</param>
    /// <returns>New Sprite</returns>
    public Sprite CreateDefaultSprite(int width = 32, int height = 32, Color color = new Color()) =>
        new(CreateDefaultTexture2D(width, height, color));
}