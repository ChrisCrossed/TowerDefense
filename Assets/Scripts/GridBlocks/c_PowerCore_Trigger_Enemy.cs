using UnityEngine;

public class c_PowerCore_Trigger_Enemy : MonoBehaviour
{
    public bool IsPassthroughTest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(UserLayers.Enemy.ToString()))
        {
            print("Found enemy: " + other.gameObject.name + " with PowerCore Goal: " + other.gameObject.GetComponent<c_ScriptTest>().PowerCoreGoal);
        }

        // print("Collision: " + collision.gameObject.name);
        if (IsPassthroughTest)
        {
            if (other.gameObject.CompareTag(UserLayers.Enemy.ToString()))
            {
                GameObject tempGoalPos = transform.parent.transform.Find("NavPoint_SouthEast").gameObject;

                other.transform.GetComponent<c_ScriptTest>().GiveTempNavGoalPosition(tempGoalPos.transform.position);
            }
        }
    }
}
