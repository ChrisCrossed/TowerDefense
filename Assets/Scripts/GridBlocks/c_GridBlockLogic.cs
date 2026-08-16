using Unity.AI.Navigation;
using UnityEngine;

public class c_GridBlockLogic : MonoBehaviour
{
    public bool DebugThis;

    private NavMeshLink thisNavMeshLink;

    private void Awake()
    {
        thisNavMeshLink = transform.Find("NavMesh Link").gameObject.GetComponent<NavMeshLink>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        GetNeighborConnections();
    }

    // Update is called once per frame
    public Transform GetNavMeshLinkTransform()
    {
        return thisNavMeshLink.gameObject.transform;
    }

    GameObject[] GO_NeighborConnections;
    void GetNeighborConnections()
    {
        GO_NeighborConnections = new GameObject[4];
        GameObject TEMP_CONNECTION;

        // N/E/W/S directions
        Vector3[] directions = new Vector3[4];
        directions[0] = gameObject.transform.position - (Vector3.forward * gameObject.transform.localScale.x);
        directions[1] = gameObject.transform.position - (Vector3.right * gameObject.transform.localScale.x);
        directions[2] = gameObject.transform.position - (Vector3.left * gameObject.transform.localScale.x);
        directions[3] = gameObject.transform.position - (Vector3.back * gameObject.transform.localScale.x);

        for (int i = 0; i < directions.Length; i++)
        {
            GO_NeighborConnections[i] = null;

            if (Physics.CheckBox(directions[i], new Vector3(0.1f, 0.1f, 0.1f)))
            {
                TEMP_CONNECTION = Physics.OverlapBox(directions[i], new Vector3(0.1f, 0.1f, 0.1f))[0].gameObject;

                if (TEMP_CONNECTION)
                {
                    if(DebugThis) print(gameObject.transform.name + " is next to " + TEMP_CONNECTION.name);
                    thisNavMeshLink.endTransform = TEMP_CONNECTION.gameObject.GetComponent<c_GridBlockLogic>().GetNavMeshLinkTransform();

                    print("Setting transform: " + thisNavMeshLink.endTransform.position);

                    GO_NeighborConnections[i] = TEMP_CONNECTION;
                }
            }
        }
    }

    void Update()
    {
        
    }
}
