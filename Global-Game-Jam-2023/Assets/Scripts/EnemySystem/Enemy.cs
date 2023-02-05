using UnityEditor;
using UnityEngine;
public enum EnemyType {
    Ground_Light, Ground_Medium, Ground_Heavy, Air_Light, Air_Heavy
}

public enum AttackTargetType {
    Resources, Tree, Roots, TreeAndRoots
}

public enum AttackMode {
    Melee, Ranged
}

[CreateAssetMenu(fileName = "new Enemy", menuName = "RootGame/Enemy" )]
public class Enemy : ScriptableObject {
    public bool isAllDataReady = true;
    public string enemyName;
    
    public Sprite sprite;
    public Sprite projectileSprite;
    public RuntimeAnimatorController animatorController;

    [SerializeField] private float maxHP;
    [HideInInspector] public float HP;
    
    [SerializeField] private float maxSpeed;
    [HideInInspector] public float speed;
    public float linearDrag;

    public EnemyType enemyType;
    public AttackTargetType attackTargetType;
    public AttackMode attackMode;
    
    public float attackDamage;
    public float attackCD;
    public float attackRange;
    public float projectileSpeed;
    public float projectileLinearDrag;

    [Tooltip("spawn one enemy in x seconds")]
    public float spawnCD; // in seconds
    [Tooltip("at which stage to start spawning")]
    public int beginStage;
    [Tooltip("at which stage to last spawning")]
    public int lastStage;
    
    private void OnEnable() {
        HP = maxHP;
        speed = maxSpeed;
    }
    
    public static void CopyEnemy(Enemy existEnemy,Enemy newEnemy) {
        newEnemy.enemyName = existEnemy.enemyName;
        newEnemy.sprite = existEnemy.sprite;
        newEnemy.projectileSprite = existEnemy.projectileSprite;
        newEnemy.animatorController = existEnemy.animatorController;
        newEnemy.attackDamage = existEnemy.attackDamage;
        newEnemy.beginStage = existEnemy.beginStage;
        newEnemy.lastStage = existEnemy.lastStage;
        newEnemy.spawnCD = existEnemy.spawnCD;
        newEnemy.attackCD = existEnemy.attackCD;
        newEnemy.HP = existEnemy.HP;
        newEnemy.maxHP = existEnemy.maxHP;
        newEnemy.enemyType = existEnemy.enemyType;
        newEnemy.attackMode = existEnemy.attackMode;
        newEnemy.attackTargetType = existEnemy.attackTargetType;
        newEnemy.speed = existEnemy.speed;
        newEnemy.maxSpeed = existEnemy.maxSpeed;
        newEnemy.linearDrag = existEnemy.linearDrag;
        newEnemy.attackRange = existEnemy.attackRange;
        newEnemy.projectileSpeed = existEnemy.projectileSpeed;
        newEnemy.projectileLinearDrag = existEnemy.projectileLinearDrag;
    }
}
