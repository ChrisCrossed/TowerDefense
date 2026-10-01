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
    void Awake()
    {
        if (DebugThis)
            print("<color=orange>Power Core - Awake: " + gameObject.name);

        gameObject.transform.Find("Trigger_Enemy").GetComponent<c_PowerCore_Trigger_Enemy>().DebugThis = DebugThis;

        LevelLogicObj = GameObject.Find("LevelLogic");
        LevelLogic = LevelLogicObj.GetComponent<c_LevelLogic>();
        
        LevelLogic.RegisterLevelObject(LevelObjectTypes.PowerCoreStructure, gameObject);

        DetermineValidWrapPoints();
        SetCarouselDirections();
        DisableEditorVisualGuides();
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

        // Determine if West side has valid entrance
        float blockDist = 3.75f;
        float vertCheckDist = 2.0f;
        Transform navPoints = gameObject.transform.Find("NavPoints").transform;

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("GridBlock");

        // LOGIC NEEDS TO CHANGE IF BLOCK DOESN'T HAVE CONNECTIONS OUTWARD
        // This is for navpoint_<DIR>_Valid booleans

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

        // Check if West direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.left * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            if(DebugThis) print("<color=red>HIT</color>");
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
            if(DebugThis) print("<color=red>HIT</color>");
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

    public void START_LevelLogic()
    {
        RunPowerCorePointInitialPathing();
    }

    void RunPowerCorePointInitialPathing()
    {
        if(ValidNavPointObject == null)
        {
            print("<color=red>POWER CORE STRUCTURE HAS NO VALID NAVPOINT OBJECTS - </color>" + gameObject.name);
            return;
        }

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

        if(DebugThis) print("Power Core Struct " + gameObject.name + " is connected to " +  SpawnerObject.name);

        // When an Enemy walks INTO this PowerCore, if they are passing through, I need to know which exit they're taking.
        // In that case, I need to know the correct EXIT they need, and guide them there.

        // BUT. If this PowerCore is their destination, I want them to roundabout the core and take the same entrance back.

        // Examples:
        // 1.) Enemy walks into PowerCore struct. THIS is their goal.
        //    1a.) Give Roundabout instructions. Give Core if available. Direct back through same entrance.
        //
        // 2.) Enemy walks into PowerCore struct. DIFFERENT PowerCore struct is their goal.
        //    2a.) Is there a PowerCore here? Yes: Perform 1a.
        //    2b.) No? Get desired goal and give Roundabout instructions toward desired goal.
        //
        // 3.) Enemy walks into PowerCore struct. They are returning to spawner.
        //    3a.) Get desired goal and give Roundabout instructions toward desired goal.
        //
        // Closing Notes: Need entrance position, need desired goal.
        // I could find the 'Midpoint' of the roundabout pathing and run a 'Give PowerCore'-style logic at that point, maybe?

        // If SoloCore, give default roundabout instructions.
        if (powerCoreRank == PowerCoreRank.SoloCore) return;

        // I need to get the positions to test for the other PowerCores/Spawner.
        // if this is the Primary, I need to also consider if the Spawner is connected.
        // if this is the Secondary, I need to consider for Tertiary AND Primary.
        // If this is the Tertiary, I need to consider for the Primary AND Secondary.
        List<Vector3> ConnectedDestinationList = new List<Vector3>();

        if (navpoint_North_Valid)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
