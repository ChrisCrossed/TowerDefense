using UnityEngine;



public class c_PowerCoreStructure : MonoBehaviour
{
    PowerCoreRank powerCoreRank;

    void Awake()
    {
        GameObject LevelLogicObj = GameObject.Find("LevelLogic");
        c_LevelLogic LevelLogic = LevelLogicObj.GetComponent<c_LevelLogic>();
        
        LevelLogic.RegisterLevelObject(LevelObjectTypes.PowerCoreStructure, gameObject);
    }

    public void SetPowerCoreRank(PowerCoreRank _rank)
    {
        powerCoreRank = _rank;
    }

    public PowerCoreRank GetPowerCoreRank()
    {
        return powerCoreRank;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
