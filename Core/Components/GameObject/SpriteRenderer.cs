using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteRenderer : Component
{
    public Sprite Sprite { get; set; }
    public Color Color { get; set; }

    private readonly float _transparent;
    public bool Flip;
    private readonly float _depth;


    public SpriteRenderer(Sprite sprite = null)
    {
        Sprite = sprite;
        Color = Color.White;
    }

    public SpriteRenderer(float depth = 0, Sprite sprite = null, float transparent = 1f)
    {
        _depth = depth;
        Sprite = sprite;
        Color = Color.White;
        _transparent = transparent;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active & gameObject.Active)
        {
            SpriteEffects flip = Flip ? SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally : SpriteEffects.FlipVertically; 

            Vector2 position = new(gameObject.DrawPosition.X, gameObject.DrawPosition.Y);
            float rotation = gameObject.DrawRotation.Z;
            Vector2 scale = new(gameObject.DrawScale.X,gameObject.DrawScale.Y);
            Vector2 origin = new (Sprite.Width / 2, Sprite.Height / 2);
            spriteBatch.Draw(Sprite.Image, position, Sprite?.Rect, Color * _transparent, rotation, origin, scale, flip, _depth);
        }
    }
}