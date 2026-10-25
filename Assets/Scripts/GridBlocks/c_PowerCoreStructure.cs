using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;



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
    List<PowerCoreDestination> PowerCoreDestinations;

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
        PowerCoreDestinations = new List<PowerCoreDestination>();
    }

    public void POST_START_LevelLogic()
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

        GameObject navPoints = gameObject.transform.Find("NavPoints").gameObject;

        GameObject SpawnerObject = null;
        NavMeshPath _path = new NavMeshPath();
        NavMeshAgent navAgent = ValidNavPointObject.GetComponent<NavMeshAgent>();

        // Get a valid NavMeshAgent for a reference point.
        // Navigate to each EnemySpawner until you have PathComplete solution.
        // Snag it and move on.
        
        List<GameObject> tempPCStructList = new List<GameObject>();
        List<float> tempPCStructDistances = new List<float>();
        List<Vector3[]> tempPCPathCorners = new List<Vector3[]>();

        foreach (GameObject spawnerObject in LevelLogic.GetEnemySpawnerObjects())
        {
            Vector3 spawnerStartPos = spawnerObject.transform.Find("NavMeshAgentObj").transform.position;

            navAgent.CalculatePath(spawnerStartPos, _path);

            if(_path.status == NavMeshPathStatus.PathComplete)
            {
                SpawnerObject = spawnerObject;

                tempPCStructList.Add(spawnerObject);

                float tempDist = 0f;
                for(int i = 0; i < _path.corners.Length - 1; i++)
                    tempDist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);
                
                tempPCStructDistances.Add(tempDist);
                tempPCPathCorners.Add(_path.corners);

                break;
            }
        }

        // Cycles through each of the OTHER PowerCoreStructures and determines if there's a 'PathComplete'
        // Adds it to the current list if there's a complete path. Otherwise, ignores.
        foreach(GameObject pcStruct in  LevelLogic.GetPowerCoreStructures())
        {
            if(pcStruct != gameObject)
            {
                // Starts at this PowerCore's NavAgent and paths to one of the PowerCoreStructure list's NavPoints to determine success.
                navAgent.CalculatePath(pcStruct.transform.Find("NavPoints").transform.Find("navpoint_SouthEast").transform.position, _path);

                if(_path.status == NavMeshPathStatus.PathComplete)
                {
                    tempPCStructList.Add(pcStruct);

                    float tempDist = 0f;
                    for(int i = 0; i < _path.corners.Length - 1; i++)
                        tempDist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);

                    tempPCStructDistances.Add(tempDist);
                    tempPCPathCorners.Add(_path.corners);
                }
            }
        }

        for(int i = 0; i < tempPCStructList.Count - 1; i++)
        {
            if (tempPCStructDistances[i] > tempPCStructDistances[i + 1])
            {
                float _dist = tempPCStructDistances[i];
                GameObject _gameObject = tempPCStructList[i];
                Vector3[] _corners = tempPCPathCorners[i];

                tempPCStructDistances.RemoveAt(i);
                tempPCStructList.RemoveAt(i);
                tempPCPathCorners.RemoveAt(i);

                tempPCStructDistances.Add(_dist);
                tempPCStructList.Add(_gameObject);
                tempPCPathCorners.Add(_corners);

                i = -1;
            }
        }

        // for(int i = 0; i < tempPCStructList.Count; i++)
        {
            // Test
            int i = 0;

            NavMeshAgent tempAgent = null;
            NavMeshPath tempPath = new NavMeshPath();
            
            if(tempPCStructList[i] == SpawnerObject)
                tempAgent = tempPCStructList[i].transform.Find("NavMeshAgentObj").transform.GetComponent<NavMeshAgent>();
            else
            {
                Transform navPointsTransform = tempPCStructList[i].transform.Find("NavPoints").transform;

                if (navPointsTransform.Find("navpoint_Outer_West").GetComponent<NavMeshAgent>().isOnNavMesh)
                    tempAgent = navPointsTransform.Find("navpoint_Outer_West").GetComponent<NavMeshAgent>();
                else if (navPointsTransform.Find("navpoint_Outer_East").GetComponent<NavMeshAgent>().isOnNavMesh)
                    tempAgent = navPointsTransform.Find("navpoint_Outer_East").GetComponent<NavMeshAgent>();
                else if (navPointsTransform.Find("navpoint_Outer_North").GetComponent<NavMeshAgent>().isOnNavMesh)
                    tempAgent = navPointsTransform.Find("navpoint_Outer_North").GetComponent<NavMeshAgent>();
                else if (navPointsTransform.Find("navpoint_Outer_South").GetComponent<NavMeshAgent>().isOnNavMesh)
                    tempAgent = navPointsTransform.Find("navpoint_Outer_South").GetComponent<NavMeshAgent>();
                else print("GAME OBJECT HAS NO VALID NavMeshAgent NAV POINTS");
            }

            print(tempAgent.transform.parent.name);

            float _dist = Mathf.Infinity;
            int closestCarouselDirection = 100;
            for (int j = 0; j < CarouselDirections.Length; j++)
            {
                tempAgent.CalculatePath(CarouselDirections[j], tempPath);

                float currDist = 0f;
                for (int k = 0; k < tempPath.corners.Length - 1; k++)
                {
                    currDist += Vector3.Distance(tempPath.corners[i], tempPath.corners[i + 1]);
                }

                if (currDist < _dist)
                {
                    closestCarouselDirection = j;
                    _dist = currDist;

                    j = -1;
                }
            }

            print("-----");

            if(tempAgent.transform.parent.gameObject == SpawnerObject)
            {
                print(tempAgent.transform.parent.name
                + ": Closest Dir: " + gameObject.name + ":"
                + CarouselDirections[closestCarouselDirection]);
            }
            else
            {
                print(tempAgent.transform.parent.transform.parent.name
                    + ": Closest Dir: " + gameObject.name + ":"
                    + CarouselDirections[closestCarouselDirection]);
            }

            print("-----");
        }

        Vector3 v3_CornerDir = tempPCPathCorners[0][1] - tempPCPathCorners[0][0];
        v3_CornerDir.Normalize();

        if (v3_CornerDir.x < -0.5f) v3_CornerDir = Vector3.left;
        else if (v3_CornerDir.x > 0.5f) v3_CornerDir = Vector3.right;
        else if (v3_CornerDir.y > 0.5f) v3_CornerDir = Vector3.forward;
        else v3_CornerDir = Vector3.back;

        Debug.DrawRay(tempPCPathCorners[0][0], v3_CornerDir * 100f, Color.red, 30f);

        if(navpoint_North_Valid)
        {

        }



        // 1.) Find the distance from the Spawner and each of the remaining PowerCoreStructures >> TO >> the center of this power core.
        // 2.) Sort by closest to furthest.
        // 3.) Then, find the shortest distance from each of the NavPoints to each of the objects in #1.
        // 3a.) When a goal has been found for it's shortest distance, assign it and remove it from the list.


        /*
        List<PowerCoreDestination> tempDestinations = new List<PowerCoreDestination>();
        for(int i = 0; i < 4; i++)
        {
            Directions tempDirection = (Directions)i;

            // Get the NavPoint from this direction. If it's valid/exists:
            // Calculate Path to each Spawner/PowerCore. Store the shortest distance
        }




        print("Spawner Object: " + SpawnerObject.name);

        if(DebugThis) print("Power Core Struct "
            + gameObject.name + " is connected to "
            +  SpawnerObject.name);

        // print(_path.corners.Length);

        // If SoloCore, give default roundabout instructions.
        if (powerCoreRank == PowerCoreRank.SoloCore || powerCoreRank == PowerCoreRank.Primary)
        {
            PowerCoreDestination tempDest = new PowerCoreDestination(SpawnerObject, ValidNavPointObject.transform, true);

            PowerCoreDestinations.Add(tempDest);

            if(powerCoreRank == PowerCoreRank.SoloCore) return;
        }

        // Starting with the West navpoint, scan through which destination is viable.
        // 1.) Scan the StartPoint as the initial direct goal, and store the distance/object.
        // 2.) If this PowerCore is 'SoloCore', we're done. Return out.
        // 3.) Otherwise, we loop through other possible PowerCore structures. Store their distances.
        // 4.) I want to store the closest destination for each given entrance.
        //    4.a) As long as all cores/startpoints are given at least one path exit, it's fine for the remaining exits to share a common exit goal.

        // Primary / SoloCore are the only ones that can connect to Spawner.
        // Secondary / Tertiary can only connect to Primary.

        float currDist = Mathf.Infinity;
        GameObject currNavpoint = navPoints.transform.Find("navpoint_Outer_West").gameObject;
        navAgent = currNavpoint.GetComponent<NavMeshAgent>();
        GameObject currentBestGoal = null;

        if (navpoint_West_Valid)
        {
            print("West Valid");
            float tempDist = 0f;

            Vector3 destinationPos = SpawnerObject.transform.Find("NavMeshAgentObj").gameObject.transform.position;

            print("<color=red>-----</color>");
            print("Spawner: " + SpawnerObject.name);
            print("Pos: " + destinationPos);
            print("NavAgent: " + navAgent.gameObject.name);

            navAgent.CalculatePath(destinationPos, _path);

            print("Status: " + _path.status);
            print("Obj: " + gameObject.transform.name + ": " +  navAgent.path.corners.Length);
            print(_path.corners.Length);

            for(int i = 0; i < _path.corners.Length - 1; i++)
                tempDist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);

            print(tempDist);

            if (tempDist < currDist)
            {
                currDist = tempDist;
                currentBestGoal = SpawnerObject; // SpawnerObject gets replaced with whatever potential connections exist
            }
        }

        print(currNavpoint.gameObject.name
            + " current best goal: "
            + currentBestGoal.name + " with a Rank of " + currentBestGoal);

        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
