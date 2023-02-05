using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public struct EnemyObject {
    public GameObject gameObject;
    public Enemy enemy;
    public int id;
}

public class EnemySystem : MonoBehaviour {

    public int maxEnemyInWorld = 70;

    private Enemy[] enemies; // The enemy base
    public List<EnemyObject> spawnedEnemyObjects = new List<EnemyObject>();
    private float[] spawnCDTimer;
    [SerializeField] private float spawnRadius;
    private Tree tree;
    private int stageLevel;

    private void Start() {
        LoadEnemyBase();
        spawnRadius = 15.0f;
        tree = FindObjectOfType<Tree>();
        stageLevel = 0;
    }

    private void Update() {
        // Update tree level
        if (stageLevel != tree.stageLevel) stageLevel = tree.stageLevel;
        
        /*
        if (Input.GetKeyDown("space")) {
            if (Random.Range(0, 1.0f) > 0.5f) Spawn("Dragon", GetSpawnPosition(), Vector3.zero); // For testing
            else Spawn("Orc", GetSpawnPosition(), Vector3.zero); // For testing
        }
        */
        
        SpawnerAuto();
        spawnRadius = Mathf.Clamp(spawnRadius += Time.deltaTime * 53f/1500f, 15.0f, 68.0f);

        for(int i = 0; i < spawnCDTimer.Length ; i++) {
            if (spawnCDTimer[i] > 0) spawnCDTimer[i] -= Time.deltaTime;
        }
    }

    private Vector3 GetSpawnPosition() {
        float angle = Random.Range(0, 2 * Mathf.PI);
        return new Vector3(spawnRadius * Mathf.Cos(angle), spawnRadius * Mathf.Sin(angle), 0);
    }
    
    public void SpawnerAuto() {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 spawnPos = new Vector3(spawnRadius * Mathf.Cos(angle), spawnRadius * Mathf.Sin(angle), 0);
        int index = Mathf.FloorToInt(Random.Range(0, enemies.Length));
        if (spawnCDTimer[index] < 0) {
            if (!enemies[index].isAllDataReady) return;
            if (stageLevel >= enemies[index].beginStage && stageLevel < enemies[index].lastStage) {
                Spawn(enemies[index].enemyName, spawnPos, Vector3.zero);
                spawnCDTimer[index] = enemies[index].spawnCD;
            }
        }
    }

    private void LoadEnemyBase() {
        List<float> tempList = new List<float>();
        enemies = Resources.LoadAll<Enemy>("Enemy");
        for (int i = 0; i < enemies.Length; i++) {
            tempList.Add(enemies[i].spawnCD);
        }
        spawnCDTimer = tempList.ToArray();
        // Debug.Log("LoadEnemyBase() done. EnemyBase.Length = " + enemies.Length);
    }

    public class WaitSpawnParameter {
        public string name;
        public Vector3 pos;
        public Vector3 rot;
    }
    
    private void Spawn(string pName, Vector3 pPos, Vector3 pRot) {
        // Handle maximum enemies case
        if (spawnedEnemyObjects.Count >= maxEnemyInWorld) {
            Debug.LogWarning("Max Spawned Enemy reached");
            return;
        }

        int spawnedID = GenID();
        Enemy en = ScriptableObject.CreateInstance<Enemy>();
        Enemy.CopyEnemy(GetEnemyWithName(pName), en);
        GameObject obj = CreateGameObject(en, spawnedID, pPos, pRot);
        
        EnemyObject enemyObject = new EnemyObject() {
            gameObject = obj, enemy = en, id = spawnedID
        };
        spawnedEnemyObjects.Add(enemyObject);
    }
    
    public void DeSpawn(EnemyObject obj) {
        if (!spawnedEnemyObjects.Contains(obj)) return;
        obj.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        obj.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(DestroyObject(obj)); 
        Debug.Log(obj.gameObject.name +"(id=" + obj.id + " despawning"); // For debugging
    }

    private IEnumerator DestroyObject(EnemyObject obj) {
        yield return new WaitForSeconds(0.5f); // wait 0.5s
        spawnedEnemyObjects.Remove(obj);
        Destroy(obj.gameObject);
        Destroy(obj.enemy);
        // Debug.Log(obj.gameObject.name +"(id=" + obj.id + " destroyed"); // For debugging
    }

    private GameObject CreateGameObject(Enemy en, int id, Vector3 pPos, Vector3 pRot) {
        GameObject obj = new GameObject(en.enemyName + "-" + (id-'0').ToString());
        Transform objTransform = obj.GetComponent<Transform>();
        objTransform.position = pPos;
        objTransform.eulerAngles = pRot;
        obj.transform.parent = this.transform; // Attach the new GameObject to EnemySystem gameobject
        SpriteRenderer _renderer = obj.AddComponent<SpriteRenderer>() as SpriteRenderer;
        _renderer.sprite = en.sprite;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = obj.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.drag = en.linearDrag;
        
        PolygonCollider2D polygonCollider2D = obj.AddComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = false;
        Animator animator = obj.AddComponent<Animator>();
        animator.runtimeAnimatorController = en.animatorController;
        obj.AddComponent<AnimationEventDispatcher>();
        obj.AddComponent<Seeker>();
        obj.AddComponent<EnemyBehaviour>();
        return obj;
    }

    private int GenID() {
        int result = Mathf.FloorToInt(Random.Range(0, maxEnemyInWorld-1));
        
        if (IsIDRepeated(result)) {
            result = result*1000 + (int)GameManager.Instance.gameTime*100;
        }
        return result;
    }
    private bool IsIDRepeated(int testID) {
        foreach (EnemyObject obj in spawnedEnemyObjects) {
            if (obj.id.Equals(testID)) return true;
        }
        return false;
    }
    
    public Enemy GetEnemyWithName(string name) {
        foreach (Enemy e in enemies) {
            if (e.enemyName.Equals(name)) return e;
        }
        throw new ArgumentNullException("Resources not exist, name: " + name);
    }
    
    public EnemyObject FindEnemyObjectWithGameObj(GameObject gameObj) {
        for (int i = 0; i < spawnedEnemyObjects.Count; i++) {
            if (spawnedEnemyObjects[i].gameObject.name.Equals(gameObj.name)) {
                return spawnedEnemyObjects[i];
            }
        }
        throw new ArgumentNullException(gameObj.name + " is not in spawned list");
    }
}
