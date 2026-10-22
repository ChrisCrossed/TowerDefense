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
    public Directions NavPointDirection { private set; get; }
    public PowerCoreRank DestinationRank { private set; get; }

    public PowerCoreDestination(Directions _navPointDirection, GameObject _destinationObject, PowerCoreRank _destinationRank)
    {
        DestinationObject = _destinationObject;
        NavPointDirection = _navPointDirection;
        DestinationRank = _destinationRank;
    }
}