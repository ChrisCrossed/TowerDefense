using UnityEngine;
using UnityEngine.AI;

public class c_ScriptTest : MonoBehaviour
{
    NavMeshAgent agent;
    private Vector3[] positions;
    bool flip;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions = new Vector3[2];

        positions[0] = GameObject.Find("GridBlock").transform.Find("NavMesh Link").transform.position;
        positions[1] = GameObject.Find("GridBlock (1)").transform.Find("NavMesh Link").transform.position;
    }

    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.5f)
        {
            timer = 0f;

            flip = !flip;

            if (flip)
            {
                agent.SetDestination(positions[0]);
            }
            else
            {
                agent.SetDestination(positions[1]);
            }
        }
    }
}
