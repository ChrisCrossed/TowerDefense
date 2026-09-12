using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class c_EnemyStartPointLogic : MonoBehaviour
{
    #region NavMesh Data
    GameObject NavMeshChildObject;
    NavMeshAgent NavAgent;
    #endregion NavMesh Data

    #region PowerCoreStructures
    List<GameObject> ValidPowerCoreStructures;
    #endregion PowerCoreStructures

    #region Level Logic Connections
    GameObject levelLogic;
    List<GameObject> AllPowerCoreStructures;
    #endregion Level Logic Connections

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #region NavMesh Data
        NavMeshChildObject = transform.Find("NavMeshAgentObj").gameObject;
        NavAgent = NavMeshChildObject.GetComponent<NavMeshAgent>();
        #endregion NavMesh Data

        #region PowerCoreStructures
        ValidPowerCoreStructures = new List<GameObject>();
        #endregion PowerCoreStructures

        #region Level Logic Connections
        levelLogic = GameObject.Find("LevelLogic").gameObject;
        AllPowerCoreStructures = new List<GameObject>();

        foreach(GameObject powerCoreObj in levelLogic.GetComponent<c_LevelLogic>().GetPowerCoreStructures())
        {
            AllPowerCoreStructures.Add(powerCoreObj);
        }
        #endregion Level Logic Connections

        INIT_GetPowerCoreConnections();
    }

    void INIT_GetPowerCoreConnections()
    {
        print("<color=red>---</color>");
        print("Running test for: " + gameObject.name);

        NavMeshPath _path = new NavMeshPath();

        float currBestDistance = Mathf.Infinity;

        GameObject PrimaryPowerCoreStructure = null;

        for(int i = 0; i < AllPowerCoreStructures.Count; i++)
        {
            print("Number of Power Core Structures: " + AllPowerCoreStructures.Count);

            Vector3 powerCoreStructPos = AllPowerCoreStructures[i].transform.Find("NavPoints").transform.Find("navpoint_SouthEast").transform.position;

            NavAgent.CalculatePath(powerCoreStructPos, _path);
            
            // If NavMeshPathStatus == PathComplete, then we know there's a valid path to that Power Core structure.
            print("Path to " + AllPowerCoreStructures[i].gameObject.name + ": " + _path.status);

            if (_path.status != NavMeshPathStatus.PathComplete)
            {
                AllPowerCoreStructures.Remove(AllPowerCoreStructures[i]);
                i--;
                continue;
            }

            #region Determine best PowerCoreStructure
            // I can use this distance to determine the closest PowerCoreStructure. If they're the same distance, just accept the first.
            float dist = 0f;

            for (int j = 0; j < _path.corners.Length - 1; j++)
                dist += Vector3.Distance(_path.corners[j], _path.corners[j + 1]);

            if (dist < currBestDistance)
            {
                currBestDistance = dist;
                PrimaryPowerCoreStructure = AllPowerCoreStructures[i];
            }
            #endregion Determine best PowerCoreStructure
        }

        if (PrimaryPowerCoreStructure == null)
        {
            print(" NO VALID POWER CORE STRUCTURES FOR " + gameObject.name);
            return;
        }

        ValidPowerCoreStructures.Add(PrimaryPowerCoreStructure);

        AllPowerCoreStructures.Remove(PrimaryPowerCoreStructure);

        foreach(GameObject remainingPowerCoreStructure in AllPowerCoreStructures)
            ValidPowerCoreStructures.Add(remainingPowerCoreStructure);

        // Test output
        foreach (GameObject testPowerCoreStruct in ValidPowerCoreStructures)
            print("Core Structures in order: " + testPowerCoreStruct.name);

    }

    /// <summary>
    /// Runs a check to ensure a path can exist if a Turret GridBlock is about to block the way
    /// </summary>
    void CheckForValidPath()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
