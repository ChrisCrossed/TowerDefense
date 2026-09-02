using UnityEngine;

public class c_PowerCore_Trigger_Enemy : MonoBehaviour
{
    public bool IsPassthroughTest;

    private void OnTriggerEnter(Collider other)
    {
        print("Enter: " + other.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision: " + collision.gameObject.name);
        if(IsPassthroughTest)
        {
            if (collision.gameObject.CompareTag(UserLayers.Enemy.ToString()))
            {
                GameObject tempGoalPos = transform.parent.transform.Find("NavPoint_SouthEast").gameObject;

                collision.transform.GetComponent<c_ScriptTest>().GiveTempNavGoalPosition(tempGoalPos.transform.position);
            }
        }
    }
}
