using UnityEngine;



public class c_PowerCoreStructure : MonoBehaviour
{
    PowerCoreRank powerCoreRank;

    bool navpoint_North_Valid = true;
    Vector3[] navpoint_North_Position = new Vector3[2];
    bool navpoint_East_Valid = true;
    Vector3[] navpoint_East_Position = new Vector3[2];
    bool navpoint_West_Valid = true;
    Vector3[] navpoint_West_Position = new Vector3[2];
    bool navpoint_South_Valid = true;
    Vector3[] navpoint_South_Position = new Vector3[2];

    void Awake()
    {
        GameObject LevelLogicObj = GameObject.Find("LevelLogic");
        c_LevelLogic LevelLogic = LevelLogicObj.GetComponent<c_LevelLogic>();
        
        LevelLogic.RegisterLevelObject(LevelObjectTypes.PowerCoreStructure, gameObject);

        DetermineValidWrapPoints();
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
    
    void DetermineValidWrapPoints()
    {
        // Determine if West side has valid entrance
        float blockDist = 3.75f;
        float vertCheckDist = 2.0f;
        Transform navPoints = gameObject.transform.Find("NavPoints").transform;

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("GridBlock");

        // Check if North direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.forward * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            print("<color=red>HIT</color>");
            navpoint_North_Position[0] = navPoints.Find("navpoint_Outer_North").transform.position;
            navpoint_North_Position[1] = navPoints.Find("navpoint_SouthWest").transform.position;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_North_Valid = false;
        }

        // Check if East direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.right * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            print("<color=red>HIT</color>");
            navpoint_East_Position[0] = navPoints.Find("navpoint_Outer_East").transform.position;
            navpoint_East_Position[1] = navPoints.Find("navpoint_NorthWest").transform.position;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_East_Valid = false;
        }

        // Check if West direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.left * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            print("<color=red>HIT</color>");
            navpoint_West_Position[0] = navPoints.Find("navpoint_Outer_West").transform.position;
            navpoint_West_Position[1] = navPoints.Find("navpoint_SouthEast").transform.position;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_West_Valid = false;
        }

        // Check if South direction has a valid block
        if (Physics.Raycast(gameObject.transform.position + (Vector3.back * blockDist) + (Vector3.up * vertCheckDist), Vector3.down, out _hit, vertCheckDist, layerMask))
        {
            print("<color=red>HIT</color>");
            navpoint_South_Position[0] = navPoints.Find("navpoint_Outer_South").transform.position;
            navpoint_South_Position[1] = navPoints.Find("navpoint_NorthEast").transform.position;
        }
        else
        {
            // Disable the navpoint_SouthEast obj
            navpoint_South_Valid = false;
        }

        // Now go through all valid positions and navigate toward other valid checkpoints
        // (such as the Enemy Spawner or other Power Cores)
        // If this power core is the Primary, then determine which direction goes toward the Spawner
        // If this power core is the Secondary or Tertiary, determine what are connected.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
