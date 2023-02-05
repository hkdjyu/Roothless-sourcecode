using UnityEngine;

/*
 * This class GameResources is going to use for store and update all GameResources that appear in game.
 * We will create GameResources as BaseType in the Unity Project Window. (current is 5 GameResources)
 *
 * When game is running, the baseType GameResources that exist in UnityProjectWindow will not have any
 * changes on their data. To create an actual GameResources that appear on screen, We will handle it in
 * GameResourcesSystem.cs and other script (if any are created later)
 */

public enum GameResourcesType{
    Nutri_Jewel, Slime_Stone, Mana_Water, Meteorite, Dark_Dust
}

[CreateAssetMenu(fileName = "new GameResources", menuName = "RootGame/GameResources" )]
public class GameResources : ScriptableObject {

    public GameResourcesType resType;
    
    public string resName;
    public Sprite resSprite;
    public RuntimeAnimatorController animatorController;
    
    [SerializeField] private float maxHP;
    [HideInInspector] public float HP;
    
    [SerializeField] private float maxResAmount;
    [HideInInspector] public float resAmount;
    
    [SerializeField] private float defaultAbsorbRate;
    [HideInInspector] public float absorbRate;
    
    [HideInInspector] public bool isAbsorbing;
    public float absorbMultiplier = 3.0f;
    public bool canAttractEnemy;
    public int beginStage; // start appear at which stage
    
    /*
     * How many resources will be spawned are decided by GameResourcesSystem
     */

    private void OnEnable() {
        HP = maxHP;
        resAmount = maxResAmount;
        absorbRate = defaultAbsorbRate;
    }

    public static void CopyGameResources(GameResources existRes,GameResources newRes) {
        newRes.resType = existRes.resType;
        newRes.resName = existRes.resName;
        newRes.resSprite = existRes.resSprite;
        newRes.animatorController = existRes.animatorController;
        newRes.maxHP = existRes.maxHP;
        newRes.HP = existRes.HP;
        newRes.maxResAmount = existRes.maxResAmount;
        newRes.resAmount = existRes.resAmount;
        newRes.defaultAbsorbRate = existRes.defaultAbsorbRate;
        newRes.absorbRate = existRes.absorbRate;
        newRes.isAbsorbing = existRes.isAbsorbing;
        newRes.canAttractEnemy = existRes.canAttractEnemy;
        newRes.beginStage = existRes.beginStage;
    }
}
