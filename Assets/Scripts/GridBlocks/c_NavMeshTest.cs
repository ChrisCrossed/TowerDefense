using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class c_NavMeshTest : MonoBehaviour
{
    public bool TogglesState;
    public bool StartCarved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carveState = !StartCarved;

        SetState();
    }

    float timer;
    bool carveState;
    // Update is called once per frame
    void Update()
    {
        if(TogglesState)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetState();
            }
        }
    }

    void SetState()
    {
        carveState = !carveState;

        gameObject.GetComponent<NavMeshObstacle>().carving = carveState;

        if (carveState)
        {
            gameObject.GetComponent<NavMeshObstacle>().size = Vector3.one;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<NavMeshObstacle>().size = Vector3.zero;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
