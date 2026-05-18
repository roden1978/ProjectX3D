using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public class CollisionSystem
{
    public bool Active { get; set; } = true;
    public bool Debug { get; }
    private const double Delay = 0.02d; // 1/50 seconds
    private readonly List<BoxCollider2D> _colliders = [];
    private readonly Dictionary<BoxCollider2D, List<BoxCollider2D>> _collisionRepository = [];
    private readonly Scene _scene;
    private readonly ICollisionMatrix _collisionMatrix;
    private double _currentDelay;

    public CollisionSystem(Scene scene, ICollisionMatrix collisionMatrix, bool debug = false)
    {
        Debug = debug;
        _scene = scene;
        _collisionMatrix = collisionMatrix;
        _scene.AddColliderComponent += OnAddColliderComponent;
        _scene.RemoveColliderComponent += OnRemoveColliderComponent;
    }

    private void OnRemoveColliderComponent(BoxCollider2D collider)
    {
        _colliders.Remove(collider);
    }

    public void Register(BoxCollider2D collider)
    {
        collider.IsDraw = Debug;
        _colliders.Add(collider);
    }
    public void Update(GameTime gameTime)
    {
        if (_currentDelay >= Delay)
        {
            RemoveDisabledColliders();
            ObserveCollisions();

            _currentDelay = 0;
        }
        else
            _currentDelay += gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void ObserveCollisions()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            if (false == _colliders[i].gameObject.Active | false == _colliders[i].Active) continue;

            for (int j = 0; j < _colliders.Count; j++)
            {
                if (false == _colliders[j].gameObject.Active | false == _colliders[j].Active) continue;

                if (i == j) continue;

                Intersect(_colliders[i], _colliders[j]);
            }
        }
    }

    private void RemoveDisabledColliders()
    {
        foreach ((BoxCollider2D key, IEnumerable<BoxCollider2D> values) in GetDisabledBoxColliders())
            foreach (BoxCollider2D item in values)
                _collisionRepository[key].Remove(item);

        foreach (BoxCollider2D item in RemoveCollidersKey())
            _collisionRepository.Remove(item);
    }

    public void SetActive(bool value) => Active = value;

    private void OnAddColliderComponent(BoxCollider2D collider2D) => 
        Register(collider2D);

    private IEnumerable<BoxCollider2D> RemoveCollidersKey()
    {
        for (int i = 0; i < _collisionRepository.Count; i++)
        {
            BoxCollider2D key = _collisionRepository.ElementAt(i).Key;
            if (false == key.gameObject.Active)
            {
                _collisionRepository[key].Clear();
                yield return key;
            }
        }
    }

    private IEnumerable<BoxCollider2D> RemoveCollidersValues(BoxCollider2D key)
    {
        for (int i = 0; i < _collisionRepository[key].Count; i++)
            if (false == _collisionRepository[key][i].gameObject.Active)
                yield return _collisionRepository[key][i];
    }

    private IEnumerable<(BoxCollider2D, IEnumerable<BoxCollider2D>)> GetDisabledBoxColliders()
    {
        foreach (BoxCollider2D key in _collisionRepository.Keys)
        {
            IEnumerable<BoxCollider2D> items = RemoveCollidersValues(key);
            if (items.Any())
                yield return (key, items);
        }
    }

    public void Intersect(BoxCollider2D collider1, BoxCollider2D collider2)
    {
        if (false == _collisionMatrix.TryGetLayer(collider1.gameObject.Layer, collider2.gameObject.Layer)) return;

        if (collider1.Box.Intersects(collider2.Box))
        {
            if (_collisionRepository.TryGetValue(collider1, out List<BoxCollider2D> stayList))
            {
                if (stayList.Contains(collider2))
                {
                    collider1.gameObject.OnCollisionStay(collider2);

                    if (CheckUpDownCollision(collider1, collider2, out int depth) & false == collider2.IsTrigger)
                    {
                        collider1.gameObject.Transform.Position = new Vector3(
                           collider1.gameObject.Transform.Position.X,
                        collider1.gameObject.Transform.Position.Y + depth - 1, 0);
                    }
                }
                else
                {
                    stayList.Add(collider2);
                    collider1.gameObject.OnCollisionEnter(collider2);
                }
            }
            else
            {
                _collisionRepository.Add(collider1, []);

                _collisionRepository[collider1].Add(collider2);
                collider1.gameObject.OnCollisionEnter(collider2);
            }
        }
        else
        {
            if (_collisionRepository.TryGetValue(collider1, out List<BoxCollider2D> exitList))
            {
                if (exitList.Contains(collider2))
                {
                    collider1.gameObject.OnCollisionExit(collider2);
                    exitList.Remove(collider2);
                }

            }
        }
    }

    private bool CheckUpDownCollision(BoxCollider2D collider1, BoxCollider2D collider2, out int depth)
    {
        if (collider1.BodyType == BodyTypes.Static)
        {
            depth = 0;
            return false;
        }

        int top = collider1.Box.Top;
        int bottom = collider2.Box.Bottom;

        if (top < bottom)
        {
            depth = Math.Abs(bottom - top);
            return true;
        }

        depth = 0;
        return false;
    }

    public void CleanUp()
    {
        _scene.AddColliderComponent -= OnAddColliderComponent;
        _scene.RemoveColliderComponent -= OnRemoveColliderComponent;
        _colliders.Clear();
        _collisionRepository.Clear();
    }
}