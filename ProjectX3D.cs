using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectX3D;

public class ProjectX3D : Game, IGraphicsDeviceProvider, IContentManagerProvider
{
    private readonly GraphicsDeviceManager _graphics;
    private Bootstraper _bootstraper; 

    public ProjectX3D()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
         InitializeGraphicsDevice();
         Window.Title = "ProjectX3D";
         
        _bootstraper = new(this);
        Components.Add(_bootstraper);

        base.Initialize();
    }
    private void InitializeGraphicsDevice()
    {
        // TODO: Add your initialization logic here

        _graphics.IsFullScreen = true;
        _graphics.HardwareModeSwitch = false;
        _graphics.GraphicsProfile = GraphicsProfile.Reach;
        _graphics.PreferredBackBufferWidth = Settings.ScreenWidth;
        _graphics.PreferredBackBufferHeight = Settings.ScreenHeight;
        _graphics.ApplyChanges();
    }   

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
