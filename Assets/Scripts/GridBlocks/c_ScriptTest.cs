using NUnit.Framework;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class c_ScriptTest : MonoBehaviour
{
    public GameObject StartPositionObject;
    public GameObject EndPositionObject;

    public bool DebugThis;

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

            if(DebugThis)
            {
                CreatePathList();

                /*
                
                */
            }
        }
        else
        {
            print("*** NO PATH ***");
            agent.speed = 0f;
        }
    }

    
    void CreatePathList()
    {
        GameObject[] spheres = new GameObject[20];
        for (int x = 0; x < 20; x++)
        {
            spheres[x] = GameObject.Find("Sphere (" + x + ")").gameObject;
        }

        int numStops = 0;
        agent.CalculatePath(positions[endPoint], path);

        Vector3 newPos = path.corners[1];
        newPos = GetNewPosition(newPos);

        // newPos.z = newPos.z + (2.5f / 2f);
        // newPos.z = Mathf.Floor(newPos.z / 2.5f) * 2.5f;

        spheres[0].transform.position = newPos;

        /*

        PathList = path.corners;

        for (int i = 0; i < PathList.Length; i++)
        {
            if(i != 0)
            {
                float dist = Vector3.Distance(PathList[i], PathList[i - 1]);

                if (dist < (2.5f / 2f))
                {
                    Vector3[] newList = new Vector3[PathList.Length - 1];
                    for(int j = 0; j < i; j++)
                    {
                        newList[j] = PathList[i];
                    }

                    PathList = newList;

                    foreach (Vector3 pos in PathList)
                    {
                        print(pos);

                        for (int x = 0; x < PathList.Length; x++)
                        {
                            spheres[x] = GameObject.Find("Sphere (" + x + ")").gameObject;
                            spheres[x].transform.position = PathList[x];
                        }
                    }
                }

                
            }
        }*/
    }

    Vector3 GetNewPosition(Vector3 pos)
    {
        Vector3 temp = pos;

        temp.x = GetNewPosition(temp.x);
        temp.z = GetNewPosition(temp.z);

        return temp;
    }
    
    float GetNewPosition(float pos)
    {
        float val = (2.5f / 2f);

        pos = pos + val;
        int temp = (int)(pos / val);
        pos = temp * val;

        return pos;
    }
}
