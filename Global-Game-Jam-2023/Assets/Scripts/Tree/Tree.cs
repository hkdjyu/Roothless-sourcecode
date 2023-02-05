using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    public GameObject RootPrefab;
    public float maxHP = 1000.0f; // 8000f is maximum at the end
    public float HP = 1000.0f;
    
    public Dictionary<GameResourcesType, float> resAmount = new Dictionary<GameResourcesType, float>();
    // How to use:
    // 
    // resAmount[GameResourcesType.Meteorite] = 0; // set Meteorite amount
    
    //DEBUGGER in Inspector
    [SerializeField] private float NutriJewelAmount = 0.0f;
    [SerializeField] private float SlimeStoneAmount = 0.0f;
    [SerializeField] private float ManaWaterAmount = 0.0f;
    [SerializeField] private float MeteoriteAmount = 0.0f;
    [SerializeField] private float DarkDustAmount = 0.0f;
    
    private bool isAbsorbing = false;
    public List<GameResourcesObject> AbsorbingResourcesObjects = new List<GameResourcesObject>();


    public int stageLevel = 0;
    private int maxStage = 5;
    public List<GameObject> Roots = new List<GameObject>();

    private void Start() {
        GetRoots();
        InitResources();
    }

    private float previousTime = 0f;
    private void Update() {
        
        if(HP<0) GameManager.Instance.GameOver();
        // DEBUGGER
        NutriJewelAmount = resAmount[GameResourcesType.Nutri_Jewel];
        SlimeStoneAmount = resAmount[GameResourcesType.Slime_Stone];
        ManaWaterAmount = resAmount[GameResourcesType.Mana_Water];
        MeteoriteAmount = resAmount[GameResourcesType.Meteorite];
        DarkDustAmount = resAmount[GameResourcesType.Dark_Dust];
        
        if (stageLevel != Mathf.FloorToInt(GameManager.Instance.gameTime) / 180 && stageLevel < maxStage) {
            stageLevel++; // 180s (3min) update stage
            NextStage();
        }
        
        // Tree Grow
        // float scale = Mathf.Clamp((GameManager.Instance.gameTime - stageLevel * 180) * (0.5f / 180f) + 1 , 1.0f, 1.5f);
        float scale = Mathf.Clamp(GameManager.Instance.gameTime * (1f/1500) + 1 , 1.0f, 2f);
        transform.parent.localScale = new Vector3(scale, scale, scale);
        
        // Absorb handling
        if (GameManager.Instance.gameTime - previousTime >= 1.0f) {
            if (isAbsorbing) {
                for(int i = 0 ; i<AbsorbingResourcesObjects.Count ; i++) {
                    GameResourcesObject resObj = AbsorbingResourcesObjects[i];
                    resAmount[resObj.gameResources.resType] += resObj.gameResources.absorbRate * resObj.gameResources.absorbMultiplier; // some problem with the rate
                    // Debug.Log("Absorb Rate = " + resObj.gameResources.absorbRate);
                    // Debug.Log("Absorb " + resObj.gameResources.resType + ": amount = " + resAmount[resObj.gameResources.resType]);
                }
            }
            previousTime = GameManager.Instance.gameTime;
            if (Input.GetKeyDown(KeyCode.Z)) {
                SpawnRoot();
            }
        }

        MaxHPAutoGrow();
        RecoverHPAuto();
    }

    private void MaxHPAutoGrow() {
        maxHP = Mathf.Clamp(1000 + (GameManager.Instance.gameTime * (7000/1500f)), 1000f, 8000f);
    }

    private void RecoverHPAuto() {
        float recoverHPAutoRate = 2.0f;
        if(HP<maxHP)
            HP += Time.deltaTime * recoverHPAutoRate + Time.deltaTime * ((maxHP <= 8000) ? 7000 / 1500f : 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor")) {
            SpriteRenderer sp = GetComponent<SpriteRenderer>();
            sp.color = new Color(1, 1, 1, 0.25f);
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor")) {
            SpriteRenderer sp = GetComponent<SpriteRenderer>();
            sp.color = new Color(1, 1, 1, 1f);
        }
    }

    private void GetRoots() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Root>() != null) {
                Roots.Add(transform.GetChild(i).gameObject);
            }
        }
    }
    
    private void InitResources() {
        GameResources[] temp = Resources.LoadAll<GameResources>("GameResources");
        for (int i = 0; i < temp.Length; i++) {
            resAmount.Add(temp[i].resType, 0f);
        }
    }

    public void SpawnRoot() {
        GameObject newRoot = null;
        switch (Roots.Count) {
            case 0:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f,-1.0f,0.0f), new Vector3(0.0f, 0.0f, 20.0f));
                break;
            case 1:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f,-1.0f,0.0f), new Vector3(0.0f, 0.0f, 110.0f));
                break;
            case 2:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -600.0f));
                break;
            case 3:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -155.0f));
                break;
            case 4:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 60.0f));
                break;
            case 5:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -105.0f));
                break;
            case 6:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 160.0f));
                break;
            case 7:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -20.0f));
                break;
            default:
                break;
        }
        
        /*
        switch (Roots.Count) {
            case 2: //Just in case
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 110.0f));
                break;
            case 3:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -155.0f));
                break;
            case 4:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 60.0f));
                break;
            case 5:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -105.0f));
                break;
            case 6:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 160.0f));
                break;
            case 7:
                newRoot = Spawn(RootPrefab, new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -20.0f));
                break;
            default:
                break;
        }
        */
        
        //newRoot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        Roots.Add(newRoot);
    }
    
    public void Hurt(float reduceHP) {
        AudioManager.Instance.PlaySfx("Hit2");
        HP -= reduceHP;
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(HurtFinish(0.1f));
    }

    IEnumerator HurtFinish(float time) {
        yield return new WaitForSeconds(time);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Absorb(GameResourcesObject resourcesObject) {
        isAbsorbing = true;
        AbsorbingResourcesObjects.Add(resourcesObject);
    }
    public void Release(GameResourcesObject resourcesObject) {
        // Debug.Log("Tree Release()");
        isAbsorbing = false;
        AbsorbingResourcesObjects.Remove(resourcesObject);
    }
    
    //Assets/Resources/Tree/Stage1/tree_2.asset
    private void NextStage() {
        Debug.Log("NexStage");
        transform.gameObject.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Tree/tree_" + stageLevel);
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D col = gameObject.AddComponent<PolygonCollider2D>();
        col.isTrigger = true;
    }
    
    private GameObject Spawn(GameObject gameObj, Vector3 pPos, Vector3 pRot) {
        GameObject newGameObj = Instantiate(gameObj);
        newGameObj.transform.parent = transform;
        newGameObj.transform.position = pPos;
        newGameObj.transform.eulerAngles = pRot;
        return newGameObj;
    }
}
