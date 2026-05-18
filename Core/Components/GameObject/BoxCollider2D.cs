using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class BoxCollider2D : Component
{
    public Rectangle Box => _box;
    public Point Offset { get; set; }
    public bool IsTrigger { get; set; }
    public bool IsDraw { get; set; }
    public BodyTypes BodyType => _bodyType;
    public Sprite Sprite { get; set; }
    private Rectangle _box;

    private readonly BodyTypes _bodyType;
    public BoxCollider2D(int width, int height, BodyTypes bodyType)
    {
        _box = new Rectangle(0, 0, width, height);
        _bodyType = bodyType;
    }

    public BoxCollider2D(int x, int y, int width, int height, BodyTypes bodyType)
    {
        _box = new Rectangle(x, y, width, height);
        _bodyType = bodyType;
    }

    public override void Start() => 
        UpdatePosition(new Vector2(gameObject.Transform.AbsolutePosition.X, gameObject.Transform.AbsolutePosition.Y));

    private void UpdatePosition(Vector2 position)
    {
        _box.X = Convert.ToInt32(position.X - _box.Width / 2 + Offset.X);
        _box.Y = Convert.ToInt32(position.Y - _box.Height / 2 + Offset.Y);
    }

    public override void Update(GameTime gameTime)
    {
        if (Active == false) return;

        UpdatePosition(new Vector2(gameObject.Transform.AbsolutePosition.X, gameObject.Transform.AbsolutePosition.Y));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active == false | IsDraw == false) return;

        if (Sprite == null)
            CreateDebugTexture(spriteBatch);

        Vector2 position = new(gameObject.DrawPosition.X + Offset.X, gameObject.DrawPosition.Y + Offset.Y);
        float rotation = gameObject.DrawRotation.Z;
        Vector2 scale = new(gameObject.DrawScale.X,gameObject.DrawScale.Y);
        Vector2 origin = new(_box.Width / 2, _box.Height / 2);
        spriteBatch.Draw(Sprite.Image, position, _box, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
    }

    private void CreateDebugTexture(SpriteBatch spriteBatch)
    {
        Color[] colors = new Color[_box.Width * _box.Height];

        for (int i = 0; i < colors.Length; i++)
            colors[i] = new Color(Color.Red, .5f);

        Texture2D newTexture2D = new(spriteBatch.GraphicsDevice, _box.Width, _box.Height);
        newTexture2D.SetData(0, 0, null, colors, 0, colors.Length);

        Sprite = new Sprite(newTexture2D);
    }


}