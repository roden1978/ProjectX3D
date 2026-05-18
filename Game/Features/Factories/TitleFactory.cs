using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TitleFactory : IFactory<GameObject>
{
    public string Name => GetType().Name;
    private readonly IContentProvider _contentProvider;

    public TitleFactory(IContentProvider contentProvider)
    {
        _contentProvider = contentProvider; 
    }
    public GameObject Create()
    {
        GameObject spaceDrawer = new("ProjectDrawer", new(Settings.ScreenWidth / 2, 0, 0), Vector3.Zero, new(1.5f, 1.8f, 0));

        TextDrawer space = new (_contentProvider.GetArtFont())
        {
            Text = "ProjectX",
            TextColor = Color.Blue
        };
        spaceDrawer
            .AddComponent(space);

        
        GameObject fighterDrawer = new("3DDrawer", new(0, 100, 0), new Vector3(0, 0, 100), Vector3.One);
        fighterDrawer.Transform.Parent = spaceDrawer.Transform;

        SpriteFont sf = _contentProvider.GetSpriteFont("JBFont");
        TrueTypeFont ttFont = new (sf); 
        
        TextDrawer fighter = new (ttFont)
        {
            Text = "3D this is very long string for test",
            TextColor = Color.Magenta
        };
        fighterDrawer
            .AddComponent(fighter);

        return spaceDrawer;
    }
}
