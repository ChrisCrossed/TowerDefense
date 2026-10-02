using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;



public class c_PowerCoreStructure : MonoBehaviour
{
    [SerializeField] bool DebugThis;

    PowerCoreRank powerCoreRank;

    bool navpoint_North_Valid = true;
    Vector3[] navpoint_North_Position = new Vector3[2];
    bool navpoint_East_Valid = true;
    Vector3[] navpoint_East_Position = new Vector3[2];
    bool navpoint_West_Valid = true;
    Vector3[] navpoint_West_Position = new Vector3[2];
    bool navpoint_South_Valid = true;
    Vector3[] navpoint_South_Position = new Vector3[2];

    Vector3[] CarouselDirections;

    GameObject LevelLogicObj;
    c_LevelLogic LevelLogic;

    private void Awake()
    {
        LevelLogicObj = GameObject.Find("LevelLogic");
        LevelLogic = LevelLogicObj.GetComponent<c_LevelLogic>();

        LevelLogic.RegisterLevelObject(LevelObjectTypes.PowerCoreStructure, gameObject);
    }

    public void AWAKE_LevelLogic()
    {
        if (DebugThis)
            print("<color=orange>Power Core - Awake: " + gameObject.name);

        gameObject.transform.Find("Trigger_Enemy").GetComponent<c_PowerCore_Trigger_Enemy>().DebugThis = DebugThis;

        DetermineValidWrapPoints();
        SetCarouselDirections();
        DisableEditorVisualGuides();
    }

    public void START_LevelLogic()
    {
        RunPowerCorePointInitialPathing();
    }

    public void SetPowerCoreRank(PowerCoreRank _rank)
    {
        powerCoreRank = _rank;
    }

    public PowerCoreRank GetPowerCoreRank()
    {
        return powerCoreRank;
    }

    // Distance in any cardinal direction from center to any neighbor block

    // Checks from a valid navpoint to various destinations in other functions
    GameObject ValidNavPointObject;
    void DetermineValidWrapPoints()
    {
        ValidNavPointObject = null;

        // Determine if each side has valid entrance
        float blockDist = 6f; // Range from the middle of the PowerCore outward to the nearest empty spot
        float vertCheckDist = 2.0f;
        Transform navPoints = gameObject.transform.Find("NavPoints").transform;

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("GridBlock");

        // Check if North direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.forward * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            if(DebugThis) print("<color=red>HIT</color>");
            navpoint_North_Position[0] = navPoints.Find("navpoint_Outer_North").transform.position;
            navpoint_North_Position[1] = navPoints.Find("navpoint_SouthWest").transform.position;

            ValidNavPointObject = navPoints.Find("navpoint_Outer_North").gameObject;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_North_Valid = false;

            navPoints.Find("navpoint_Outer_North").GetComponent<NavMeshAgent>().enabled = false;
        }


        // Check if West direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.left * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            if (DebugThis) print("<color=red>HIT</color>");
            navpoint_West_Position[0] = navPoints.Find("navpoint_Outer_West").transform.position;
            navpoint_West_Position[1] = navPoints.Find("navpoint_SouthEast").transform.position;

            if (ValidNavPointObject == null)
                ValidNavPointObject = navPoints.Find("navpoint_Outer_West").gameObject;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_West_Valid = false;

            navPoints.Find("navpoint_Outer_West").GetComponent<NavMeshAgent>().enabled = false;
        }

        // Check if South direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.back * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            if (DebugThis) print("<color=red>HIT</color>");
            navpoint_South_Position[0] = navPoints.Find("navpoint_Outer_South").transform.position;
            navpoint_South_Position[1] = navPoints.Find("navpoint_NorthEast").transform.position;

            if (ValidNavPointObject == null)
                ValidNavPointObject = navPoints.Find("navpoint_Outer_South").gameObject;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_South_Valid = false;

            navPoints.Find("navpoint_Outer_South").GetComponent<NavMeshAgent>().enabled = false;
        }

        // Check if East direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.right * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            if(DebugThis) print("<color=red>HIT</color>");
            navpoint_East_Position[0] = navPoints.Find("navpoint_Outer_East").transform.position;
            navpoint_East_Position[1] = navPoints.Find("navpoint_NorthWest").transform.position;

            if(ValidNavPointObject == null)
                ValidNavPointObject = navPoints.Find("navpoint_Outer_East").gameObject;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_East_Valid = false;

            navPoints.Find("navpoint_Outer_East").GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    // Assigns the four Carousel positions in CounterClockwise order when Enemies walk in the Trigger
    void SetCarouselDirections()
    {
        CarouselDirections = new Vector3[4];

        CarouselDirections[0] = navpoint_North_Position[1];
        CarouselDirections[1] = navpoint_West_Position[1];
        CarouselDirections[2] = navpoint_South_Position[1];
        CarouselDirections[3] = navpoint_East_Position[1];
    }

    void DisableEditorVisualGuides()
    {
        transform.Find("Corner_NorthWest").gameObject.SetActive(false);
        transform.Find("Corner_NorthEast").gameObject.SetActive(false);
        transform.Find("Corner_SouthWest").gameObject.SetActive(false);
        transform.Find("Corner_SouthEast").gameObject.SetActive(false);

        Transform pathingNavBlocks = transform.Find("PathingNavBlocks");

        pathingNavBlocks.Find("Path_North").gameObject.SetActive(navpoint_North_Valid);
        pathingNavBlocks.Find("Path_West").gameObject.SetActive(navpoint_West_Valid);
        pathingNavBlocks.Find("Path_South").gameObject.SetActive(navpoint_South_Valid);
        pathingNavBlocks.Find("Path_East").gameObject.SetActive(navpoint_East_Valid);
    }

    

    

    void RunPowerCorePointInitialPathing()
    {
        if(ValidNavPointObject == null)
        {
            print("<color=red>POWER CORE STRUCTURE HAS NO VALID NAVPOINT OBJECTS - </color>" + gameObject.name);
            return;
        }

        Transform navPoints = gameObject.transform.Find("NavPoints").transform;

        GameObject SpawnerObject = null;
        NavMeshPath _path = new NavMeshPath();
        NavMeshAgent navAgent = ValidNavPointObject.GetComponent<NavMeshAgent>();

        // Get a valid NavMeshAgent for a reference point.
        // Navigate to each EnemySpawner until you have PathComplete solution.
        // Snag it and move on.

        foreach (GameObject spawnerObject in LevelLogic.GetEnemySpawnerObjects())
        {
            Vector3 spawnerStartPos = spawnerObject.transform.Find("NavMeshAgentObj").transform.position;

            navAgent.CalculatePath(spawnerStartPos, _path);

            if(_path.status == NavMeshPathStatus.PathComplete)
            {
                SpawnerObject = spawnerObject;
                break;
            }
        }

        if(DebugThis) print("Power Core Struct "
            + gameObject.name + " is connected to "
            +  SpawnerObject.name);

        // If SoloCore, give default roundabout instructions.
        // if (powerCoreRank == PowerCoreRank.SoloCore) return;

        // Starting with the West navpoint, scan through which destination is viable.
        // 1.) Scan the StartPoint as the initial direct goal, and store the distance/object.
        // 2.) If this PowerCore is 'SoloCore', we're done. Return out.
        // 3.) Otherwise, we loop through other possible PowerCore structures. Store their distances.
        // 4.) I want to store the closest destination for each given entrance.
        //    4.a) As long as all cores/startpoints are given at least one path exit, it's fine for the remaining exits to share a common exit goal.

        float currDist = Mathf.Infinity;
        GameObject currNavpoint = navPoints.Find("navpoint_Outer_West").gameObject;
        navAgent = currNavpoint.GetComponent<NavMeshAgent>();
        GameObject currentBestGoal = null;

        if (navpoint_West_Valid)
        {
            _path = new NavMeshPath();

            print("West Valid");
            float tempDist = 0f;

            print("Spawner: " + SpawnerObject.name);

            navAgent.CalculatePath(SpawnerObject.transform.position, _path);

            print(_path.corners[0] + " + " + _path.corners[1] + " = " + Vector3.Distance(_path.corners[0], _path.corners[1]));
            print(_path.corners[1] + " + " + _path.corners[2] + " = " + Vector3.Distance(_path.corners[1], _path.corners[2]));
            print(_path.corners[2] + " + " + _path.corners[3] + " = " + Vector3.Distance(_path.corners[2], _path.corners[3]));
            print(_path.corners[3] + " + " + _path.corners[4] + " = " + Vector3.Distance(_path.corners[3], _path.corners[4]));

            /*
            for(int i = 0; i < _path.corners.Length - 1; i++)
                tempDist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);
            */

            print(tempDist);

            if (tempDist < currDist)
            {
                currDist = tempDist;
                currentBestGoal = SpawnerObject; // SpawnerObject gets replaced with whatever potential connections exist
            }
        }

        print(currNavpoint.gameObject.name
            + " current best goal: "
            + currentBestGoal.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
