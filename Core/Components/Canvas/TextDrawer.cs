using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextDrawer : Component, ICanvasComponent, ICanvasDrawableComponent
{
    public string Text = string.Empty;
    public Sequence Sequence;
    private readonly Font _font;
    private readonly StringSource _stringSource;
    public Color TextColor = Color.White;
    public Rectangle Rect { get; private set; }
    private Sprite[] _sprites = [];
    private string _prevText = string.Empty;

    public TextDrawer(Font font, StringSource source = null)
    {
        _font = font;
        _stringSource = source;
    }

    public override void Start() => Initialize();

    private void Initialize()
    {
        CreateSpriteString();
        if (_sprites.Length != 0)
            Rect = new Rectangle(0, 0, _sprites.Length * _sprites[0].Width, _sprites[0].Height);
        else
            Rect = new();
    }

    private void CreateSpriteString()
    {
        PrepareText();

        _sprites = _font.Create(Text);
        _prevText = Text;
    }

    private void PrepareText()
    {
        if (_stringSource != null)
        {
            if (_stringSource.Value.Equals(_prevText)) return;

            if (Text.Equals(_stringSource.Value)) return;
            else if (false == _stringSource.Value.Equals(string.Empty))
                Text = _stringSource.Value.ToString();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_font.SpriteFont is null)
            DrawArt(spriteBatch);
        else
            DrawSpriteFont(spriteBatch);
    }

    private void DrawArt(SpriteBatch spriteBatch)
    {
        int allSpritesWidth = 0;
        for (int i = 0; i < _sprites.Length; i++)
        {
            allSpritesWidth += _sprites[i].Width;
        }
        for (int i = 0; i < _sprites.Length; i++)
        {
            float startPosX = gameObject.DrawPosition.X - allSpritesWidth / 2 * gameObject.DrawScale.X;
            Vector2 nextPosition = new(startPosX + i * _sprites[i].Width * gameObject.DrawScale.X, gameObject.DrawPosition.Y);
            spriteBatch.Draw(_sprites[i].Image, nextPosition, _sprites[i]?.Rect, TextColor,
                                 gameObject.DrawRotation.Z, Vector2.Zero, new Vector2(gameObject.DrawScale.X, gameObject.DrawScale.Y), SpriteEffects.None, 0f);
        }
    }

    private void DrawSpriteFont(SpriteBatch spriteBatch)
    {
        Vector2 position = new(gameObject.DrawPosition.X, gameObject.DrawPosition.Y);
        Vector2 scale = new(gameObject.DrawScale.X, gameObject.DrawScale.Y);
        Vector2 origin = new(_font.SpriteFont.MeasureString(Text).X / 2, _font.SpriteFont.MeasureString(Text).Y / 2);
        spriteBatch.DrawString(_font.SpriteFont, Text, position, TextColor, gameObject.DrawRotation.Z, origin, scale, SpriteEffects.None, 0);
    }

    public override void Update(GameTime gameTime) => CreateSpriteString();

    public override void Destroy() => Sequence.Destroy();
}
