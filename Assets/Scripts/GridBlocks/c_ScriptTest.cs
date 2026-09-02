using NUnit.Framework;
using System.IO;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

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
        positions[1] = EndPositionObject.transform.position;

        path = new NavMeshPath();
        

        // print("Path Test: " + path.status);

        agent.SetDestination(positions[endPoint]);

        print("Turn Speed: " + agent.angularSpeed);
        agent.acceleration = 30f; // Default 8f
        agent.angularSpeed = 360f * 5f; // Default 120f degrees per second

        CheckForPath();
    }

    int endPoint = 1;
    // Update is called once per frame
    void Update()
    {
        /*
        if(agent.remainingDistance < 0.1f)
        {
            endPoint += 1;
            endPoint %= positions.Length;

            agent.SetDestination(positions[endPoint]);

            CheckForPath();
        }
        */

        SetForwardAngle();

        // Test for when I press Space to change the state of the turret boxes and their blocking
        if (Input.GetKeyDown(KeyCode.L))
        {
            CheckForPath();
        }
    }

    void CheckForPath()
    {
        if (!DebugThis) return;

        print("Ran Check for Path");
        // agent.CalculatePath(positions[endPoint], path);
        agent.CalculatePath(EndPositionObject.transform.position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            print("Path Successful");
            // agent.path = path;
            agent.SetPath(path);
            agent.speed = 3.5f;

            // agent.SetDestination(positions[endPoint]);

            NavigationPositions = new List<Vector3>();

            NavigationPositions.Add(EndPositionObject.transform.position);
            StartCoroutine(PathingThread());

            if (DebugThis)
            {
                // CreatePathList();

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

    void SetForwardAngle()
    {
        Vector3 forwardPos = gameObject.transform.position + gameObject.transform.forward;
        Vector3 nextPos = agent.steeringTarget;
        nextPos.y = gameObject.transform.position.y;

        Vector3 lookAtPos = Vector3.Lerp(forwardPos, nextPos, 0.98f * Time.deltaTime);

        gameObject.transform.LookAt(lookAtPos);
    }

    List<Vector3> NavigationPositions;
    float RaycastDist = 0.5f;
    void CreatePathList()
    {
        print("Started CreatePathList()");
        NavigationPositions = new List<Vector3>();

        GameObject[] spheres = new GameObject[20];
        for (int x = 0; x < 20; x++)
        {
            spheres[x] = GameObject.Find("Sphere (" + x + ")").gameObject;
        }

        agent.CalculatePath(positions[endPoint], path);

        int layerMask = LayerMask.NameToLayer(UserLayers.GridBlock.ToString());
        foreach(Vector3 pos in path.corners)
        {
            print("Running Raycast");
            RaycastHit hit;

            Vector3 newPos = pos;
            newPos.y += RaycastDist;

            Debug.DrawRay(newPos, Vector3.down * (RaycastDist + 0.5f), Color.red, 1.0f);

            if(Physics.Raycast(newPos, Vector3.down, out hit, RaycastDist + 0.5f, layerMask))
            {
                print("FOUND: " + hit.collider.name);
                NavigationPositions.Add(hit.collider.transform.Find("NavPoint").transform.position);
            }
        }
        
        for (int j = 0; j < NavigationPositions.Count; j++)
        {
            spheres[j].transform.position = NavigationPositions[j];
        }

        StartCoroutine(PathingThread());
    }

    bool HasNewTempGoalPos;
    IEnumerator PathingThread()
    {
        print("Num Positions: " + NavigationPositions.Count);

        for(int i = 0; i < NavigationPositions.Count; i++)
        {
            agent.SetDestination(NavigationPositions[i]);

            float dist = Vector3.Distance(gameObject.transform.position, NavigationPositions[i]);
            
            while(dist > 1 || HasNewTempGoalPos)
            {
                dist = Vector3.Distance(gameObject.transform.position, NavigationPositions[i]);

                // print(dist);
                yield return new WaitForSeconds(1f / 30f);
            }

            HasNewTempGoalPos = false;
            print("Reached Goal");

            if (NavigationPositions.Count > 0)
            {
                NavigationPositions.RemoveAt(i);
                print("Removed");
            }

            if(NavigationPositions.Count == 0)
            {
                agent.isStopped = true;
            }
        }

        yield return null;
    }

    public void GiveTempNavGoalPosition(Vector3 tempWorldPos)
    {
        print("Adding New Position at front of path list");

        List<Vector3> tempNavPositions = new List<Vector3>();

        // Determine if world position is valid

        // Determine if world position is available on agent path

        // If clear, push to the front of the NavigationPositions list
        if(NavigationPositions == new List<Vector3>())
            NavigationPositions = new List<Vector3>();

        tempNavPositions.Add(tempWorldPos);

        foreach (Vector3 navPos in NavigationPositions)
            tempNavPositions.Add(navPos);

        NavigationPositions.Clear();
        NavigationPositions = tempNavPositions;

        HasNewTempGoalPos = true;
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
