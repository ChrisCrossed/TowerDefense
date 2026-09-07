using UnityEngine;

public class c_PowerCoreStructure : MonoBehaviour
{
    void Awake()
    {
        GameObject LevelLogicObj = GameObject.Find("LevelLogic");
        c_LevelLogic LevelLogic = LevelLogicObj.GetComponent<c_LevelLogic>();
        
        LevelLogic.RegisterLevelObject(LevelObjectTypes.PowerCoreStructure, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
