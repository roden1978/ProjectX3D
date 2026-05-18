using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class UIImage : Component, ICanvasComponent, ICanvasDrawableComponent
{
    public Color Color { get; set; }
    public Rectangle Rect { get => _sprite.Rect;}
    private readonly float _transparent;
    private readonly Sprite _sprite;

    public UIImage(Sprite sprite, float transparent = 1f)
    {
        _sprite = sprite;
        _transparent = transparent;
        Color = Color.White;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active & gameObject.Active)
        {
            Vector2 origin = new (_sprite.Rect.Width / 2, _sprite.Rect.Height / 2);
            spriteBatch.Draw(_sprite.Image, new Vector2(gameObject.DrawPosition.X, gameObject.DrawPosition.Y), _sprite.Rect,
                Color * _transparent, gameObject.DrawRotation.Z, origin, new Vector2(gameObject.DrawScale.X,gameObject.DrawScale.Y), SpriteEffects.None, 1);
        }
    }
}
