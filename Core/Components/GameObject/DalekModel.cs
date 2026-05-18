using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MeshFilter : Component
{
    public Model Model {get; private set;}
    public Texture2D Texture {get; set;}

    public MeshFilter(Model model, Texture2D texture = null)
    {
        Model = model;
        Texture = texture;
    }
}

public class MeshRenderer : Component
{
    private readonly MeshFilter _mesh;

    public MeshRenderer(MeshFilter mesh)
    {
        _mesh = mesh;
    }

    public override void Draw(Camera camera)
    {
        
    }
}
public class DalekModel : Component
{
    private readonly int _meshCount;
    private readonly Model _model;
    private Matrix _world;
    private Texture2D _texture;

    public DalekModel(Model model, Texture2D texture)
    {
        _model = model;
        _texture = texture;
        _meshCount = model.Meshes.Count;
        _world = Matrix.Identity;
    }

    public override void Draw(Camera camera)
    {
        var transforms = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(transforms);

        for (int i = 0; i < _meshCount; i++)
        {
            foreach (var meshEffect in _model.Meshes[i].Effects)
            {
                var effect = (BasicEffect)meshEffect;
                effect.EnableDefaultLighting();
                effect.TextureEnabled = true;
                effect.Texture = _texture;

                effect.DirectionalLight0.DiffuseColor = Color.Red.ToVector3();
                effect.DirectionalLight0.Direction = new Vector3(0, -4, -4);

                effect.World = transforms[_model.Meshes[i].ParentBone.Index] * _world;
                camera.Display(effect);
            }
            _model.Meshes[i].Draw();
        }
    }

    public override void Update(GameTime gameTime)
    {
        var modelAngleDalek = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _world *= Matrix.CreateRotationY(modelAngleDalek);
    }
}
