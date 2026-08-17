using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class c_GridBlockLogic : MonoBehaviour
{
    public bool DebugThis;

    private NavMeshLink[] NavMeshLinks;

    private void Awake()
    {
        NavMeshLinks = new NavMeshLink[4];
        NavMeshLinks[(int)Directions.North] = transform.Find("NavMeshLink_North").gameObject.GetComponent<NavMeshLink>();
        NavMeshLinks[(int)Directions.East] = transform.Find("NavMeshLink_East").gameObject.GetComponent<NavMeshLink>();
        NavMeshLinks[(int)Directions.West] = transform.Find("NavMeshLink_West").gameObject.GetComponent<NavMeshLink>();
        NavMeshLinks[(int)Directions.South] = transform.Find("NavMeshLink_South").gameObject.GetComponent<NavMeshLink>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        GetNeighborConnections();
    }

    /// <summary>
    /// GetNavMeshLinkTransform
    /// </summary> Returns the NavMeshLink Transform facing the requested direction
    /// <param name="_fromDirection"></param> Gives the NavMeshLink Transform in the direction requested
    /// For example: If the piece requesting the info is West, and is REQUESTING the info East of it, then
    /// use Directions.East for the function.
    /// <returns></returns>
    public Transform GetNavMeshLinkTransform(Directions _fromDirection)
    {
        return NavMeshLinks[(int)_fromDirection].gameObject.transform;
    }

    GameObject[] GO_NeighborConnections;
    void GetNeighborConnections()
    {
        GO_NeighborConnections = new GameObject[4];
        GameObject TEMP_CONNECTION;

        // N/E/W/S directions
        Vector3[] directions = new Vector3[4];
        directions[(int)Directions.North] = gameObject.transform.position - (Vector3.forward * gameObject.transform.localScale.x);
        directions[(int)Directions.East] = gameObject.transform.position - (Vector3.right * gameObject.transform.localScale.x);
        directions[(int)Directions.West] = gameObject.transform.position - (Vector3.left * gameObject.transform.localScale.x);
        directions[(int)Directions.South] = gameObject.transform.position - (Vector3.back * gameObject.transform.localScale.x);

        Vector3 overlapBoxSize = new Vector3(0.1f, 0.1f, 0.1f);

        for (int i = 0; i < directions.Length; i++)
        {
            GO_NeighborConnections[i] = null;

            if (Physics.CheckBox(directions[i], overlapBoxSize))
            {
                // Probably a better way to get the game object associated with the connection
                TEMP_CONNECTION = Physics.OverlapBox(directions[i], overlapBoxSize)[0].gameObject;

                if (TEMP_CONNECTION)
                {
                    if(DebugThis) print(gameObject.transform.name + " is next to " + TEMP_CONNECTION.name);

                    // Also need a better way to get the relevant NavMeshLink object (for scenarios such as bridges and elongated pieces that have 2+ navmesh points)
                    NavMeshLinks[i].endTransform = TEMP_CONNECTION.gameObject.GetComponent<c_GridBlockLogic>().GetNavMeshLinkTransform( (Directions)i );

                    print("Setting transform: " + NavMeshLinks[i].endTransform.position);

                    GO_NeighborConnections[i] = TEMP_CONNECTION;
                }
            }
        }
    }

    void Update()
    {
        
    }
}
