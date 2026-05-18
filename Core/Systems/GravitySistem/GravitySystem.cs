using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class GravitySystem
{
    public bool Active { get; private set; } = true;
    private readonly float _gravity;
    private readonly List<Rigidbody> _bodies = [];

    public GravitySystem(float gravity = -9.81f)
    {
        _gravity = gravity;
    }

    public void SetActive(bool value) => 
        Active = value;

    public void Update(GameTime gameTime)
    {
        if (Active == false) return;

        foreach (Rigidbody rigidbody in _bodies)
        {
            if (rigidbody.UseGravity == false && rigidbody.Velocity != 0)
            {
                rigidbody.Velocity = 0.0f;
            }

            if (rigidbody.UseGravity)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                rigidbody.Velocity += _gravity * deltaTime * 40;
                rigidbody.gameObject.Transform.Position += new Vector3(0, rigidbody.Velocity * deltaTime, 0);
            }
        }
    }

    public void Register(Scene scene)
    {
        foreach (GameObject go in scene.FindGameObjectsWithComponent<Rigidbody>())
            _bodies.Add(go.GetComponent<Rigidbody>());
    }
    public void CleanUp() => 
        _bodies.Clear();
}