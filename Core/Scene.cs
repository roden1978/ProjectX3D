using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
public class Scene
{
    public event Action<BoxCollider2D> AddColliderComponent;
    public event Action<BoxCollider2D> RemoveColliderComponent;
    public string Name;
    public Canvas Canvas => _canvas;
    public bool Active { get; set; } = false;
    public bool Started { get; private set; }
    private readonly List<GameObject> _gameObjects = [];
    private readonly Canvas _canvas;
    private readonly Camera _camera;
    private WorldGrid _worldGrid;
    private readonly bool _debug;

    public Scene(string name, Canvas canvas, Camera camera, bool debug = false)
    {
        Name = name;
        _canvas = canvas;
        _camera = camera;
        _debug = debug;
    }

    public void Initialize()
    {
        if (_debug)
            CreateWorldGrid();
            
        _canvas.Initialize();
    }

    private void CreateWorldGrid()
    {
        _worldGrid = new WorldGrid(_camera.GraphicsDevice);
        _worldGrid.Initialize();
    }

    public void Register(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
            RecursiveAdd(Add, gameObject);
    }

    public Scene Register(GameObject gameObject)
    {
        RecursiveAdd(Add, gameObject);

        return this;
    }
    private void RecursiveAdd(Action<GameObject> add, GameObject gameObject)
    {
        add(gameObject);
        foreach (Transform child in gameObject.Transform.Childrens)
            RecursiveAdd(Add, child.Gameobject);
    }
    private void RecursiveRemove(Action<GameObject> remove, GameObject gameObject)
    {
        foreach (Transform child in gameObject.Transform.Childrens)
            RecursiveAdd(Remove, child.Gameobject);
        remove(gameObject);
    }
    private void Add(GameObject gameObject)
    {
        gameObject.Scene = this;

        if (gameObject.HasAnyIUIComponent())
        {
            if (gameObject.TryGetComponent(out CanvasHandler _) == false)
            {
                gameObject.AddComponent(new CanvasHandler());
            }
            _canvas.Register(gameObject);
            return;
        }

        if (gameObject.TryGetComponent(out BoxCollider2D collider2D))
        {
            AddColliderComponent?.Invoke(collider2D);
        }

        _gameObjects.Add(gameObject);
    }
    private void Remove(GameObject gameObject)
    {
        if (_canvas.Contains(gameObject))
        {
            gameObject.Destroy();
            _canvas.Remove(gameObject);
            return;
        }

        if (gameObject.TryGetComponent(out BoxCollider2D collider2D))
        {
            RemoveColliderComponent?.Invoke(collider2D);
        }

        gameObject.Destroy();
        _gameObjects.Remove(gameObject);
    }
    public void Unregister(GameObject gameObject)
    {
        Console.WriteLine($"Unregister {gameObject.Name}");
        RecursiveRemove(Remove, gameObject);
    }

    public void Update(GameTime gameTime)
    {
        if (false == Active) return;

        UpdateCanvas(gameTime);
        UpdateScene(gameTime);
    }

    public void Draw()
    {
        if (false == Active) return;

        if (_camera.CameraType == CameraTypes.Perspective)
        {
            // Reset for 3D drawing
            _camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _camera.GraphicsDevice.BlendState = BlendState.Opaque;
            _camera.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DrawScene(_camera);
        }
        else
        {
            _camera.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, RasterizerState.CullClockwise, _camera.SpriteBatchEffect);
            DrawScene(_camera.SpriteBatch);
            _camera.SpriteBatch.End();
        }

        DrawCanvas();
    }

    private void UpdateScene(GameTime gameTime)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
            _gameObjects[i]?.Update(gameTime);
    }

    private void DrawScene(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
            _gameObjects[i]?.Draw(spriteBatch);
    }

    private void DrawScene(Camera camera)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
            _gameObjects[i]?.Draw(camera);

        if (_debug)
            _worldGrid.Draw(camera);
    }
    private void UpdateCanvas(GameTime gameTime) =>
        _canvas.Update(gameTime);

    private void DrawCanvas() =>
        _canvas.Draw();

    public void Start()
    {
        for (int i = 0; i < _gameObjects.Count; i++)
            if (_gameObjects[i].Active)
                _gameObjects[i].Start();

        _canvas.Start();

        Active = true;
        SetStarted(true);
    }

    public GameObject FindGameObjectWithTag(Tags tag) =>
        _gameObjects.FirstOrDefault(x => x.Tag == tag);

    public IReadOnlyList<GameObject> FindGameObjectsWithComponent<T>() =>
        _gameObjects.Where(x => x.ContainsComponent<T>()).ToList();

    public IReadOnlyList<GameObject> FindAllGameObjects<T>() =>
        _gameObjects;


    public void CleanUp()
    {
        for (int i = 0; i < _gameObjects.Count; i++)
            Unregister(_gameObjects[i]);

        _gameObjects.Clear();
    }

    public void SetActive(bool value)
    {
        _canvas.SetActive(value);

        for (int i = 0; i < _gameObjects.Count; i++)
            _gameObjects[i]?.SetActive(value);

        Active = value;
    }
    public void SetStarted(bool value) =>
        Started = value;
}