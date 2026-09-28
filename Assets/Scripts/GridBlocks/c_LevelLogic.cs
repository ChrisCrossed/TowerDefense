using System.Collections.Generic;
using UnityEngine;

public class c_LevelLogic : MonoBehaviour
{
    List<GameObject> PowerCoreStructures;
    List<GameObject> EnemySpawnObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("<color=orange>Level Logic - Start: " + gameObject.name);

        foreach (GameObject obj in EnemySpawnObjects)
            obj.GetComponent<c_EnemyStartPointLogic>().RunEnemyStartPointInitialPathing();

        foreach (GameObject obj in PowerCoreStructures)
            obj.GetComponent<c_PowerCoreStructure>().RunPowerCorePointInitialPathing();
    }

    /// <summary>
    /// Board Objects that need to register submit their existence during the Awake phase
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_self"></param>
    public void RegisterLevelObject(LevelObjectTypes _type, GameObject _self)
    {
        if (PowerCoreStructures == null)
        {
            PowerCoreStructures = new List<GameObject>();
            print("Created new Power Core Struct List");
        }

        if (EnemySpawnObjects == null)
        {
            EnemySpawnObjects = new List<GameObject>();
            print("Created new Enemy Spawn Objects List");
        }

        switch (_type)
        {
            case LevelObjectTypes.PowerCoreStructure:
                PowerCoreStructures.Add(_self);
                break;
            case LevelObjectTypes.PowerCore:
                break;
            case LevelObjectTypes.EnemySpawnPoint:
                EnemySpawnObjects.Add(_self);
                break;
            default:
                break;
        }

        print("Registered Level Object: " + _self.name);
    }

    

    public IReadOnlyList<GameObject> GetPowerCoreStructures()
    {
        return PowerCoreStructures as IReadOnlyList<GameObject>;
    }

    public IReadOnlyList<GameObject> GetEnemySpawnerObjects()
    {
        return EnemySpawnObjects as IReadOnlyList<GameObject>;
    }

    public void BeginRound(int _round = 0)
    {
        // If round is 0, apply an even number of cores unless otherwise stated

        // If round is 0, run initial pathing for the NavMesh

        // Tell Enemy & Ally Spawn points which round to begin playing
    }
}
