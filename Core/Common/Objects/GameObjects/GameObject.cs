using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

public class GameObject
{
    public event Action<int> ChildrensCountHasChanged;
    public Vector3 DrawPosition => Transform.Parent != null ? Transform.Position + Transform.Parent.AbsolutePosition : Transform.Position;
    public Vector3 DrawRotation => Transform.Parent != null ? Transform.Rotation + Transform.Parent.AbsoluteRotation : Transform.Rotation;
    public Vector3 DrawScale => Transform.Parent != null ? Transform.Scale * Transform.Parent.AbsoluteScale : Transform.Scale;
    public bool Active { get; private set; } = true;
    public bool Started { get; private set; }
    public Transform Transform => _transform;
    public string Name { get; set; }
    public Tags Tag { get; set; }
    public int Layer { get; set; }
    public int ComponentCount => _componentsContainer.Count;
    public Scene Scene { get; set; }
    public Canvas Canvas { get; set; }
    public int ChildrensCount => _childrensCount;

    private readonly Container<Component> _componentsContainer = new();
    private readonly Transform _transform = new();
    private int _childrensCount;


    public GameObject()
    {
        Name = "GameObject";
        _transform.Gameobject = this;
    }
    public GameObject(string name) : this() => Name = name;
    public GameObject(string name, Transform transform) : this(name)
    {
        _transform = transform;
        _transform.Gameobject = this;
    }
    public GameObject(string name, Transform transform, Transform parent) : this(name, transform) => _transform.Parent = parent;
    public GameObject(string name, Vector3 position, Vector3 rotation, Vector3 scale) : this(name)
    {
        _transform.Position = position;
        _transform.Rotation = rotation;
        _transform.Scale = scale;
    }

    public GameObject(string name, Vector3 position, Vector3 rotation, Vector3 scale, Transform parent) : this(name, position, rotation, scale)
    {
        _transform.Parent = parent;
    }
    public void Awake() => 
        AwakeComponents();
    public void Start()
    {
        Awake();
        StartComponents();

        SetStarted(true);
    }

    private void AwakeComponents() => 
        _componentsContainer.AwakeComponents();
    private void StartComponents() => 
        _componentsContainer.StartComponents();

    public void Update(GameTime gameTime)
    {
        if (false == Active) return;

        for (int i = 0; i < _componentsContainer.Count; i++)
            _componentsContainer[i]?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (false == Active) return;

        for (int i = 0; i < _componentsContainer.Count; i++)
            _componentsContainer[i]?.Draw(spriteBatch);
    }

    public void Draw(Camera camera)
    {
        if (false == Active) return;

        for (int i = 0; i < _componentsContainer.Count; i++)
            _componentsContainer[i]?.Draw(camera);
    }
    public GameObject AddComponent<T>(T component) where T : Component
    {
        Type type = typeof(Component);
        PropertyInfo publicFieldInfo = type.GetProperty("gameObject", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        publicFieldInfo?.SetValue(component, this, null);

        _componentsContainer.Register(component);

        if (Started)
            component.Start();

        return this;
    }
    public void Unregister<T>(T component) where T : Component =>
        _componentsContainer.Unregister(component);

    public T GetComponent<T>() where T : class
    {
        if (_componentsContainer.TryGetComponent(out T component))
            return component;

        return default;
    }
    public bool TryGetComponent<T>(out T component) where T : class
    {
        if (false == _componentsContainer.TryGetComponent(out T c))
        {
            component = default;
            return false;
        }
        
        component = c;
        return true;
    }
    public bool HasAnyIUIComponent() =>
        _componentsContainer.HasAnyIUIComponent();

    public IEnumerable<ICanvasDrawableComponent> GetDraws() =>
        _componentsContainer.GetDraws();

    public IInteractable GetInteractable() =>
        _componentsContainer.GetInteractable();

    public bool ContainsComponent<T>() =>
        _componentsContainer.ContainsComponent<T>();

    public void OnCollisionStay(BoxCollider2D collider2) =>
        _componentsContainer.OnCollisionStay(collider2);

    public void OnCollisionEnter(BoxCollider2D collider2) =>
        _componentsContainer.OnCollisionEnter(collider2);

    public void OnCollisionExit(BoxCollider2D collider2) =>
        _componentsContainer.OnCollisionExit(collider2);

    public override string ToString() => Name;

    public void SetActive(bool value)
    {
        Active = value;

        if (value) Awake(); else Disable();

        foreach (Transform children in Transform.Childrens)
            children.Gameobject.SetActive(value);

        _componentsContainer.SetActive(value);

        if (false == Started & value) Start();
    }

    public void AddChild()
    {
        _childrensCount++;
        ChildrensCountHasChanged?.Invoke(_childrensCount);
    }

    public void RemoveChild()
    {
        if (_childrensCount <= 0) return;
        
        _childrensCount--;
        ChildrensCountHasChanged?.Invoke(_childrensCount);
    }

    public void SetStarted(bool value) => Started = value;

    public void Destroy() => Cleanup();

    public void Disable(){ }

    private void Cleanup()
    {
       CleanupComponents();
       Transform.Parent = null;
    }

    private void CleanupComponents() =>
        _componentsContainer.DestroyComponents();
    
}