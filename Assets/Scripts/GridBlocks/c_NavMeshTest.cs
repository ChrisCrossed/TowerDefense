using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class c_NavMeshTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    float timer;
    bool carveState;
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            carveState = !carveState;

            gameObject.GetComponent<NavMeshObstacle>().carving = carveState;

            if(carveState)
            {
                gameObject.GetComponent<NavMeshObstacle>().size = Vector3.one;
            }
            else
            {
                gameObject.GetComponent<NavMeshObstacle>().size = Vector3.zero;
            }
        }
    }
}
