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
        positions[0] = new Vector3(-3.75f, 0.5f, 3.755f);
        positions[1] = new Vector3(11.5f, 0.5f, -3.75f);

        path = new NavMeshPath();
        

        print("Path Test: " + path.status);

        agent.SetDestination(positions[endPoint]);

        CheckForPath();
    }

    int endPoint = 1;
    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 0.1f)
        {
            endPoint += 1;
            endPoint %= positions.Length;

            agent.SetDestination(positions[endPoint]);

            CheckForPath();
        }

        // Test for when I press Space to change the state of the turret boxes and their blocking
        if(Input.GetKeyDown(KeyCode.L))
        {
            CheckForPath();
        }
    }

    void CheckForPath()
    {
        agent.CalculatePath(positions[endPoint], path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            print("Path Successful");
            // agent.path = path;
            agent.SetPath(path);
            agent.speed = 3.5f;

            agent.SetDestination(positions[endPoint]);
        }
        else
        {
            print("*** NO PATH ***");
            agent.speed = 0f;
        }
    }
}
