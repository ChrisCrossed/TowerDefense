using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class c_EnemyStartPointLogic : MonoBehaviour
{
    #region NavMesh Data
    GameObject NavMeshChildObject;
    NavMeshAgent NavAgent;
    #endregion NavMesh Data

    #region PowerCoreStructures
    #endregion PowerCoreStructures

    #region Level Logic Connections
    GameObject levelLogic;
    List<GameObject> AllPowerCoreStructures;
    #endregion Level Logic Connections

    private void Awake()
    {
        levelLogic = GameObject.Find("LevelLogic").gameObject;

        // Register self for evaluation
        levelLogic.GetComponent<c_LevelLogic>().RegisterLevelObject(LevelObjectTypes.EnemySpawnPoint, gameObject);

        #region NavMesh Data
        NavMeshChildObject = transform.Find("NavMeshAgentObj").gameObject;
        NavAgent = NavMeshChildObject.GetComponent<NavMeshAgent>();
        #endregion NavMesh Data
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // print("<color=orange>Spawner - Start: " + gameObject.name);
    }

    public void RunEnemyStartPointInitialPathing()
    {
        #region Level Logic Connections
        // Get all PowerCoreStructures for evaluation
        AllPowerCoreStructures = new List<GameObject>();
        foreach (GameObject powerCoreObj in levelLogic.GetComponent<c_LevelLogic>().GetPowerCoreStructures())
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

        // get all power cores
        List<float> powerCoreDistances = new List<float>();

        // Cycle through and remove invalid ones that aren't connected
        for (int i = 0; i < AllPowerCoreStructures.Count; i++)
        {
            Vector3 powerCoreStructPos = AllPowerCoreStructures[i].transform.Find("NavPoints").transform.Find("navpoint_SouthEast").transform.position;

            NavAgent.CalculatePath(powerCoreStructPos, _path);

            if (_path.status != NavMeshPathStatus.PathComplete)
            {
                AllPowerCoreStructures.Remove(AllPowerCoreStructures[i]);
                i--;
                continue;
            }

            if (AllPowerCoreStructures.Count == 0)
            {
                print(" NO VALID POWER CORE STRUCTURES FOR " + gameObject.name);
                return;
            }

            /// Need to determine method to check for second/third best if they exist

            #region Determine best PowerCoreStructure
            // I can use this distance to determine the closest PowerCoreStructure. If they're the same distance, just accept the first.
            float dist = 0f;

            for (int j = 0; j < _path.corners.Length - 1; j++)
                dist += Vector3.Distance(_path.corners[j], _path.corners[j + 1]);

            powerCoreDistances.Add(dist);

            // print("<color=red> --------- </color> Distance to " + AllPowerCoreStructures[i].gameObject.name + ": " + powerCoreDistances[i]);
            #endregion Determine best PowerCoreStructure
        }

        List<GameObject> tempPowerCoreList = new List<GameObject>();
        List<float> tempPowerCoreDistList = new List<float>();


        while(AllPowerCoreStructures.Count > 0)
        {
            int currShortest = AllPowerCoreStructures.Count - 1;

            // Sort by distance
            for (int i = 0; i < AllPowerCoreStructures.Count; i++)
            {
                if (powerCoreDistances[i] < powerCoreDistances[currShortest])
                {
                    currShortest = i;
                }
            }

            tempPowerCoreList.Add(AllPowerCoreStructures[currShortest]);
            tempPowerCoreDistList.Add(powerCoreDistances[currShortest]);

            AllPowerCoreStructures.RemoveAt(currShortest);
            powerCoreDistances.RemoveAt(currShortest);
        }

        AllPowerCoreStructures.Clear();
        powerCoreDistances.Clear();

        EnemyDestinationList = new List<GameObject>();

        foreach(GameObject tempPowerCore in tempPowerCoreList)
        {
            AllPowerCoreStructures.Add(tempPowerCore);
            powerCoreDistances.Add(tempPowerCoreDistList[0]);
            tempPowerCoreDistList.RemoveAt(0);

            // Adds the PowerCore to the list of destinations for PowerCore NavAgent evaluation
            EnemyDestinationList.Add(tempPowerCore);
        }

        // DEBUG TEXT
        /*
        print("<color=red>" + gameObject.name + " found " + AllPowerCoreStructures.Count + " valid Power Cores.</color>");
        for (int i = 0; i < AllPowerCoreStructures.Count; ++i)
        {
            print("<color=red>" + AllPowerCoreStructures[i].name + " has a dist of " + powerCoreDistances[i] + "</color>");
        }
        */

        if(AllPowerCoreStructures.Count == 1)
        {
            AllPowerCoreStructures[0].GetComponent<c_PowerCoreStructure>().SetPowerCoreRank(PowerCoreRank.SoloCore);
            return;
        }

        // Assign Core Rank by distance
        for (int i = 0; i < AllPowerCoreStructures.Count; i++)
        {
            c_PowerCoreStructure powerCoreStructure = AllPowerCoreStructures[i].GetComponent<c_PowerCoreStructure>();

            powerCoreStructure.SetPowerCoreRank((PowerCoreRank)i);
        }

        // DEBUG TEXT
        /*
        for (int i = 0; i < AllPowerCoreStructures.Count; ++i)
        {
            print("<color=red>" + AllPowerCoreStructures[i].name + " has a Rank of " + AllPowerCoreStructures[i].GetComponent<c_PowerCoreStructure>().GetPowerCoreRank() + "</color>");
        }
        */
    }

    List<GameObject> EnemyDestinationList;
    public List<GameObject> GetEnemyDestinationList()
    {
        List<GameObject> tempDestList = new List<GameObject>();

        return tempDestList;
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
