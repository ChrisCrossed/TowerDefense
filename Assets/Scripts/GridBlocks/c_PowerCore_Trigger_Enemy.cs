using UnityEngine;

public class c_PowerCore_Trigger_Enemy : MonoBehaviour
{
    public bool DebugThis { private get; set; }

    public bool IsPassthroughTest { private get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if(DebugThis) print("Found enemy: " + other.gameObject.name + " with PowerCore Goal: " + other.gameObject.GetComponent<c_ScriptTest>().PowerCoreGoal);

            Vector2 entranceDir = new Vector2(other.transform.position.x - gameObject.transform.position.x, other.transform.position.z - gameObject.transform.position.z);
            entranceDir.Normalize();

            Directions dir = Directions.West;

            if      (entranceDir.x > 0.5f)   dir = Directions.East;
            else if (entranceDir.y > 0.5f)   dir = Directions.North;
            else if (entranceDir.y < -0.05f) dir = Directions.South;

            print(dir.ToString());
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
