using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class c_ScriptTest : MonoBehaviour
{
    public GameObject StartPositionObject;
    public GameObject EndPositionObject;

    NavMeshAgent agent;
    private Vector3[] positions;
    bool flip;
    NavMeshPath path;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions = new Vector3[2];

        // positions[0] = StartPositionObject.transform.Find("NavMeshLink_North").transform.position;
        // positions[1] = EndPositionObject.transform.Find("NavMeshLink_North").transform.position;
        positions[0] = new Vector3(1.25f, 0.25f, 1.25f);
        positions[1] = new Vector3(6f, 0.25f, -1.25f);

        path = new NavMeshPath();
        

        print("Path Test: " + path.status);

        //agent.SetDestination(positions[0]);
    }

    int endPoint = 0;
    // Update is called once per frame
    void Update()
    {
       if(agent.remainingDistance < 0.1f)
        {
            endPoint += 1;
            endPoint %= positions.Length;

            //agent.SetDestination(positions[endPoint]);
        }

       if(Input.GetKeyDown(KeyCode.L))
        {
            agent.CalculatePath(positions[1], path);
            print("Path Test: " + path.status);
        }

       if(agent.remainingDistance == Mathf.Infinity)
        {
            print("PATH BROKEN");
        }
    }
}
