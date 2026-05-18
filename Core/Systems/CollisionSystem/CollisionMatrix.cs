using System.Collections.Generic;
public class CollisionMatrix : ICollisionMatrix
{
    private readonly Dictionary<int, List<int>> _matrix = new()
    {
        {
            (int)CollisionLayers.Ship, 
            [
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Ufo, 
                (int)CollisionLayers.Pickeable
            ]
        },
        {
            (int)CollisionLayers.Asteroid, 
            [
                (int)CollisionLayers.Laser, 
                (int)CollisionLayers.Ship, 
                (int)CollisionLayers.Alien, 
                (int)CollisionLayers.Shield,
                (int)CollisionLayers.Pickeable
            ]
        },
        {
            (int)CollisionLayers.Ufo, 
            [
                (int)CollisionLayers.Ship, 
                (int)CollisionLayers.Laser, 
                (int)CollisionLayers.Alien, 
                (int)CollisionLayers.Shield, 
                (int)CollisionLayers.Pickeable
            ]
        },
        {
            (int)CollisionLayers.Laser, 
            [
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Ufo
            ]
        },
        {
            (int)CollisionLayers.EnemyLaser, 
            [
                (int)CollisionLayers.Ship, 
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Alien, 
                (int)CollisionLayers.Shield, 
                (int)CollisionLayers.Pickeable
            ]
        },
        {
            (int)CollisionLayers.Pickeable, 
            [
                (int)CollisionLayers.Ship, 
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Ufo, 
                (int)CollisionLayers.EnemyLaser
            ]
        },
        {
            (int)CollisionLayers.Alien, 
            [
                (int)CollisionLayers.Ship, 
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Ufo, 
                (int)CollisionLayers.EnemyLaser, 
                (int)CollisionLayers.Laser, 
                (int)CollisionLayers.Shield
            ]
        },
        {
            (int)CollisionLayers.Shield, 
            [
                (int)CollisionLayers.Asteroid, 
                (int)CollisionLayers.Ufo, 
                (int)CollisionLayers.EnemyLaser, 
                (int)CollisionLayers.Alien
            ]
        },
    };

    public bool TryGetLayer(int layer, int otherLayer)
    {
        if(_matrix.TryGetValue(layer, out List<int> others))
            if(others.Contains(otherLayer)) return true;

        return false;
    }

}