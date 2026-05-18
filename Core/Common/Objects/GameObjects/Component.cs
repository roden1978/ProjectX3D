using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Component
{
    public bool Active { get; private set; } = true;
    public GameObject gameObject { get; private set; }
    public bool Started { get; private set; }
    public virtual void SetActive(bool value)
    {
        Active = value;
        
        if (value) Awake(); 
        else Disable();
    }

    public virtual void Start() => 
        Started = true;
    public virtual void Awake() { }
    public virtual void Draw(SpriteBatch spriteBatch){}
    public virtual void Draw(Camera camera = null) { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void OnCollisionEnter(BoxCollider2D other) { }
    public virtual void OnCollisionExit(BoxCollider2D other) { }
    public virtual void OnCollisionStay(BoxCollider2D other) { }
    public virtual void Disable() { }
    public virtual void Destroy() => 
        Disable();
}