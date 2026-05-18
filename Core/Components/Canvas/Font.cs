using System.Collections.Immutable;
using Microsoft.Xna.Framework.Graphics;

public abstract class Font
{
    public SpriteFont SpriteFont {get;}
    public Font(SpriteFont spriteFont = null)
    {
        SpriteFont = spriteFont;
    }
    protected Sequence Sequence;
    protected ImmutableDictionary<char, int> Characters;
    public virtual Sprite[] Create(string word){return [];}
}
