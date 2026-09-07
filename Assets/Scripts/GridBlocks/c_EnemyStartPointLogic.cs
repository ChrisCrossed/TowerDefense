using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class c_EnemyStartPointLogic : MonoBehaviour
{
    GameObject NavMeshChildObject;
    NavMeshAgent NavAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NavMeshChildObject = transform.Find("NavMeshAgentObj").gameObject;
        NavAgent = NavMeshChildObject.GetComponent<NavMeshAgent>();

        GetPowerCoreConnections();
    }

    void GetPowerCoreConnections()
    {
        NavMeshPath _path = new NavMeshPath();

        // Get list of Power Cores from the LevelLogic once they've registered
        GameObject levelLogic = GameObject.Find("LevelLogic").gameObject;
        List<GameObject> AllPowerCoreStructures = levelLogic.GetComponent<c_LevelLogic>().GetPowerCoreStructures();

        foreach (GameObject powerCoreStructure in AllPowerCoreStructures)
        {
            Vector3 powerCoreStructPos = powerCoreStructure.transform.Find("NavPoints").transform.Find("navpoint_SouthEast").transform.position;

            NavAgent.CalculatePath(powerCoreStructPos, _path);

            // If NavMeshPathStatus == PathComplete, then we know there's a valid path to that Power Core structure.
            print("Path to " + powerCoreStructure.gameObject.name + ": " + _path.status);

            float dist = 0f;

            for(int i = 0; i < _path.corners.Length - 1; i++)
                dist += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);

            // I can use this distance to determine the closest PowerCoreStructure. If they're the same distance, just accept the first.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
