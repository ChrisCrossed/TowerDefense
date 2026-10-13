using UnityEngine;

// N/E/W/S directions
public enum Directions
{
    North = 0,
    West = 1,
    South = 2,
    East = 3
}

public enum UserLayers
{
    Enemy = 0,
    GridBlock = 1
}

public enum LevelObjectTypes
{
    PowerCoreStructure,
    PowerCore,
    EnemySpawnPoint
}

public enum PowerCoreRank
{
    SoloCore = -1,
    Primary = 0,
    Secondary = 1,
    Tertiary = 2,
    Spawner = 3
}

public struct PowerCoreDestination
{
    public GameObject DestinationObject { private set; get; }
    public Transform NavPoint { private set; get; }
    public Directions NavPointDirection { private set; get; }
    public bool IsSpawner { private set; get; }

    public PowerCoreDestination(GameObject _destinationObject, Transform _navPoint, Directions _navPointDirection, bool _isSpawner )
    {
        DestinationObject = _destinationObject;
        NavPoint = _navPoint;
        NavPointDirection = _navPointDirection;
        IsSpawner = _isSpawner;
    }

}