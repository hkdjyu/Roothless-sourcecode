using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


/*
 * I added a lot of comments for you to understand my code and learn quickly. 
 * You do not need to add as many comments as me but I do recommend you to add comments on
 * important, complicated codes and large function.
 *
 * If you have any question, DO take a screenshot and ask on Discord
 * or stream your screen on Discord when I am sitting there.
 * 
 * TODO:
 * 1. Finish the TODO part
 * 2. Design how to determine the position to spawn GameResourcesObject
 * 3. Think how to interact with other object like root and AI enemy (maybe done by Thomas)
 */


/*
 * The GameResources Object is a data type, like float, Vector2, position, etc.
 * To manipulate the GameResources in game, the GameObject that allows you to manipulate in Screen are
 * 'bounded' by GameResourcesObject.
 *
 * To access the data inside the GameResourcesObject DataType, Just like Vector2 and position
 * e.g. Vector2 coor = new Vector2()
 *      float x_value = coor.x;
 * e.g. GameResourcesObject resObj = new GameResourcesObject() {
            gameObject = someGameObject, gameResources = someGameResources, id = someID
        };
        GameObject a_game_object = resObj.gameObject;
 */

public struct GameResourcesObject {
    public GameObject gameObject;
    public GameResources gameResources;
    public int id;
    public Vector3 startPos;
    public float rot;
    public float timeSpawned;
}

/*
 * The following GameResourcesSystem Class is going to handle LOTS of things, as I have not
 * carefully think and divide it into other subclasses to make it easier to read and code.
 *
 * 
 */

public class GameResourcesSystem : MonoBehaviour {
    
    public int maxSpawnedGameResources = 1024; // maximum GameResources spawned during game
    
    private float ScaleMultiplier = 0.95f;
    private int maxSpawnedNutriJewel = 5;
    private int maxSpawnedSlimeStone = 6;
    private int maxSpawnedManaWater = 6;
    private int maxSpawnedMeteorite = 6;
    private int maxSpawnedDarkDust = 3;

    //Number of Times each resource tried to spawn at a valid position, to make sure no endless looping
    private int SpawnTrial = 0;

    //Cooldown for each resource to spawn
    private float SpawnCD_NutriJewel = 70.0f;
    private float SpawnCD_SlimeStone = 12.0f;
    private float SpawnCD_ManaWater = 25.0f;
    private float SpawnCD_Meteorite = 25.0f;
    private float SpawnCD_DarkDust = 7.0f;

    //Records the time each resource was spawned recently
    public float TimeLast_NutriJewel = 0.0f;
    public float TimeLast_SlimeStone = 0.0f;
    public float TimeLast_ManaWater = 0.0f;
    public float TimeLast_Meteorite = 0.0f;
    public float TimeLast_DarkDust = 0.0f;

    //Spawn position Vector for randomization
    private float xPos, yPos;
    private Vector3 SpawnPos_NutriJewel;
    private Vector3 SpawnPos_SlimeStone;
    private Vector3 SpawnPos_ManaWater;
    private Vector3 SpawnPos_Meteorite;
    private Vector3 SpawnPos_DarkDust;
    
    private GameResources[] gameResources; // The 5 base GameResources
    
    // This List will keep track on and save all spawned GameResourcesObject.
    public List<GameResourcesObject> spawnedGameResourcesObjects = new List<GameResourcesObject>();
    
    public List<GameResourcesObject> spawnedNutriJewel = new List<GameResourcesObject>();
    public List<GameResourcesObject> spawnedSlimeStone = new List<GameResourcesObject>();
    public List<GameResourcesObject> spawnedManaWater = new List<GameResourcesObject>();
    public List<GameResourcesObject> spawnedMeteorite = new List<GameResourcesObject>();
    public List<GameResourcesObject> spawnedDarkDust = new List<GameResourcesObject>();

    private void Start() {
        LoadGameResources(); // Load all type of GameResources from project folder
        SpawnPos_NutriJewel = new Vector3(0.0f, 0.0f, 0.0f);
        SpawnPos_SlimeStone = new Vector3(0.0f, 0.0f, 0.0f);
        SpawnPos_ManaWater = new Vector3(0.0f, 0.0f, 0.0f);
        SpawnPos_Meteorite = new Vector3(0.0f, 0.0f, 0.0f);
        SpawnPos_DarkDust = new Vector3(0.0f, 0.0f, 0.0f);
        //Spawn("TestGameResources1", Vector3.zero, Vector3.zero); // for testing only
        Spawn("NutriJewel", new Vector3(0.2f, -4.1f, 0.0f), Vector3.zero);
        Spawn("SlimeStone", new Vector3(3.0f, 3.4f, 0.0f), Vector3.zero);
        Spawn("ManaWater", new Vector3(5.0f, -1.2f, 0.0f), Vector3.zero);
        Spawn("NutriJewel", new Vector3(7.2f, 2.7f, 0.0f), Vector3.zero);
        Spawn("SlimeStone", new Vector3(-3.0f, 5.0f, 0.0f), Vector3.zero);
        Spawn("ManaWater", new Vector3(-6.8f, -4.2f, 0.0f), Vector3.zero);
        Spawn("NutriJewel", new Vector3(-7.5f, 6.0f, 0.0f), Vector3.zero);
    }

    private void Update() {
        //Debug.Log(Camera.main.orthographicSize);
        if (GameManager.Instance.gameTime - TimeLast_NutriJewel > SpawnCD_NutriJewel) {
            TimeLast_NutriJewel = GameManager.Instance.gameTime;
            SpawnTrial = 0;
            do {
                xPos = Random.Range(-Camera.main.orthographicSize * 1.6667f, Camera.main.orthographicSize * 1.6667f);
                yPos = Random.Range(-Camera.main.orthographicSize * 0.9375f, Camera.main.orthographicSize * 0.9375f);
                SpawnTrial++;
            }
            while ((Mathf.Sqrt(xPos*xPos + yPos*yPos) < Camera.main.orthographicSize * 0.75f || Overlapping(xPos, yPos)) && SpawnTrial < 80);
            //Debug.Log("TimeLast_NutriJewel = " + TimeLast_NutriJewel);
            if (SpawnTrial != 80)
                Spawn("NutriJewel", new Vector3(xPos, yPos, 0.0f), Vector3.zero);
        }
        if (GameManager.Instance.gameTime - TimeLast_SlimeStone > SpawnCD_SlimeStone) {
            TimeLast_SlimeStone = GameManager.Instance.gameTime;
            SpawnTrial = 0;
            do {
                xPos = Random.Range(-Camera.main.orthographicSize * 1.6667f, Camera.main.orthographicSize * 1.6667f);
                yPos = Random.Range(-Camera.main.orthographicSize * 0.9375f, Camera.main.orthographicSize * 0.9375f);
                SpawnTrial++;
            }
            while ((Mathf.Sqrt(xPos*xPos + yPos*yPos) < Camera.main.orthographicSize * 0.75f || Overlapping(xPos, yPos)) && SpawnTrial < 80);
            //Debug.Log("TimeLast_SlimeStone = " + TimeLast_SlimeStone);
            if (SpawnTrial != 80)
                Spawn("SlimeStone", new Vector3(xPos, yPos, 0.0f), Vector3.zero);
        }
        if (GameManager.Instance.gameTime - TimeLast_ManaWater > SpawnCD_ManaWater) {
            TimeLast_ManaWater = GameManager.Instance.gameTime;
            SpawnTrial = 0;
            do {
                xPos = Random.Range(-Camera.main.orthographicSize * 1.6667f, Camera.main.orthographicSize * 1.6667f);
                yPos = Random.Range(-Camera.main.orthographicSize * 0.9375f, Camera.main.orthographicSize * 0.9375f);
                SpawnTrial++;
            }
            while ((Mathf.Sqrt(xPos*xPos + yPos*yPos) < Camera.main.orthographicSize * 0.75f || Overlapping(xPos, yPos)) && SpawnTrial < 80);
            //Debug.Log("TimeLast_ManaWater = " + TimeLast_ManaWater);
            if (SpawnTrial != 80)
                Spawn("ManaWater", new Vector3(xPos, yPos, 0.0f), Vector3.zero);
        }
        if (GameManager.Instance.gameTime - TimeLast_Meteorite > SpawnCD_Meteorite) {
            TimeLast_Meteorite = GameManager.Instance.gameTime;
            SpawnTrial = 0;
            do {
                xPos = Random.Range(-Camera.main.orthographicSize * 1.6667f, Camera.main.orthographicSize * 1.6667f);
                yPos = Random.Range(-Camera.main.orthographicSize * 0.9375f, Camera.main.orthographicSize * 0.9375f);
                SpawnTrial++;
            }
            while ((Mathf.Sqrt(xPos*xPos + yPos*yPos) < 10.0f || Mathf.Sqrt(xPos*xPos + yPos*yPos) < Camera.main.orthographicSize * 0.75f || Overlapping(xPos, yPos)) && SpawnTrial < 80);
            //Debug.Log("TimeLast_Meteorite = " + TimeLast_Meteorite);
            if (SpawnTrial != 80)
                Spawn("MeteoriteOfBlessings", new Vector3(xPos, yPos, 0.0f), Vector3.zero);
        }
        if (GameManager.Instance.gameTime - TimeLast_DarkDust > SpawnCD_DarkDust) {
            TimeLast_DarkDust = GameManager.Instance.gameTime;
            SpawnTrial = 0;
            do {
                xPos = Random.Range(-Camera.main.orthographicSize * 1.6667f, Camera.main.orthographicSize * 1.6667f);
                yPos = Random.Range(-Camera.main.orthographicSize * 0.9375f, Camera.main.orthographicSize * 0.9375f);
                SpawnTrial++;
            }
            while ((Mathf.Sqrt(xPos*xPos + yPos*yPos) < 17.0f || Mathf.Sqrt(xPos*xPos + yPos*yPos) < Camera.main.orthographicSize * 0.75f || Overlapping(xPos, yPos)) && SpawnTrial < 80);
            //Debug.Log("TimeLast_DarkDust = " + TimeLast_DarkDust);
            if (SpawnTrial != 80)
                Spawn("DarkDust", new Vector3(xPos, yPos, 0.0f), Vector3.zero);
        }
        //Debug.Log(GameManager.Instance.gameTime);
        //FindObjectOfType<GameResourcesSystem>().maxSpawnedManaWater = 5;
        //spawnedGameResourcesObjects[i].gameObject.transform.position = new Vector3(0,0,0)
            //Above gets spawnedGameResources[i].gameObject position

        for (int i=0 ; i<spawnedGameResourcesObjects.Count ; i++) {
            // if any spawnedGameResourcesObjects HP is below 0, Destroy it.
            // Debug.Log(spawnedGameResourcesObjects[i].startPos);
            if (spawnedGameResourcesObjects[i].gameResources.resName == "NutriJewel"
             || spawnedGameResourcesObjects[i].gameResources.resName == "SlimeStone") {
                if(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned < 3.0f) {
                    spawnedGameResourcesObjects[i].gameObject.transform.localScale
                    = new Vector3((GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/3.0f*ScaleMultiplier,
                                  (GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/3.0f*ScaleMultiplier,
                                  (GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/3.0f*ScaleMultiplier);
                }
            }
            if (spawnedGameResourcesObjects[i].gameResources.resName == "ManaWater") {
                if(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned < 3.0f) {
                    spawnedGameResourcesObjects[i].gameObject.transform.localScale
                    = new Vector3((GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/2.0f*ScaleMultiplier,
                                  (GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/2.0f*ScaleMultiplier,
                                  (GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)/2.0f*ScaleMultiplier);
                }
            }
            if (spawnedGameResourcesObjects[i].gameResources.resName == "Meteorite") {
                if(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned <= 8.0f) {
                    spawnedGameResourcesObjects[i].gameObject.transform.position
                    = spawnedGameResourcesObjects[i].startPos
                        + new Vector3(4.0f, 36.0f, 0.0f)
                        + new Vector3(-(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)*0.5f,
                        -(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)*4.5f,
                        0.0f);
                    spawnedGameResourcesObjects[i].gameObject.transform.localScale
                    = new Vector3(ScaleMultiplier, ScaleMultiplier, ScaleMultiplier);
                }
            }
            if (spawnedGameResourcesObjects[i].gameResources.resName == "DarkDust") {
                //if (spawnedGameResourcesObjects[i].gameResources.resAmount)
                if (!spawnedGameResourcesObjects[i].gameResources.canAttractEnemy) {
                    spawnedGameResourcesObjects[i].gameObject.transform.localScale
                    = new Vector3((Mathf.Atan(10.0f*Mathf.Sin((GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)*Mathf.PI/17.0f)))/(Mathf.Atan(10.0f))*1.5f*ScaleMultiplier,
                                    (Mathf.Atan(10.0f*Mathf.Sin((GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)*Mathf.PI/17.0f)))/(Mathf.Atan(10.0f))*1.5f*ScaleMultiplier,
                                    (Mathf.Atan(10.0f*Mathf.Sin((GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned)*Mathf.PI/17.0f)))/(Mathf.Atan(10.0f))*1.5f*ScaleMultiplier);
                    
                    spawnedGameResourcesObjects[i].gameObject.transform.position
                    = spawnedGameResourcesObjects[i].startPos
                        + new Vector3(4.0f * Mathf.Sin(GameManager.Instance.gameTime + i) * Mathf.Cos(Mathf.Deg2Rad*spawnedGameResourcesObjects[i].rot),
                        4.0f * Mathf.Sin(GameManager.Instance.gameTime + i) * Mathf.Sin(Mathf.Deg2Rad*spawnedGameResourcesObjects[i].rot),
                        0.0f);

                    if(GameManager.Instance.gameTime - spawnedGameResourcesObjects[i].timeSpawned > 17.0f) {
                        DeSpawn(spawnedGameResourcesObjects[i]);
                    }
                }
                else {
                    spawnedGameResourcesObjects[i].gameObject.transform.localScale
                    = new Vector3(ScaleMultiplier, ScaleMultiplier, ScaleMultiplier);
                }
            }
            if (spawnedGameResourcesObjects[i].gameResources.HP <= 0.0f || spawnedGameResourcesObjects[i].gameResources.resAmount <= 0.0f) {
                DeSpawn(spawnedGameResourcesObjects[i]);
            }
        }
    }

    // Load the GameResources in project folder to gameResources
    private void LoadGameResources() {
        gameResources = Resources.LoadAll<GameResources>("GameResources");
    }
    
    // To Spawn an actual GameResources that interactable during game.
    private void Spawn(string pResName, Vector3 pPos, Vector3 pRot) {
        
        switch(GetResourcesWithName(pResName).resType) {
            case GameResourcesType.Meteorite:
                if (spawnedMeteorite.Count >= maxSpawnedMeteorite) return;
                break;
            case GameResourcesType.Dark_Dust:
                if (spawnedDarkDust.Count >= maxSpawnedDarkDust) return;
                break;
            case GameResourcesType.Mana_Water:
                if (spawnedManaWater.Count >= maxSpawnedManaWater) return;
                break;
            case GameResourcesType.Nutri_Jewel:
                if (spawnedNutriJewel.Count >= maxSpawnedNutriJewel) return;
                break;
            case GameResourcesType.Slime_Stone:
                if (spawnedSlimeStone.Count >= maxSpawnedSlimeStone) return;
                break;
        }
        
        int spawnedid = GenID();
        GameResources res = ScriptableObject.CreateInstance<GameResources>(); // Declare a new GameResources 
        GameResources.CopyGameResources(GetResourcesWithName(pResName), res); // Initialize the GameResources value
        GameObject obj = CreateGameObject(res, spawnedid, pPos, pRot); // Declare and Initialize a new GameObject
        
        // Declare and Initialize a new GameResourcesObject
        GameResourcesObject gameResourcesObject = new GameResourcesObject() {
            gameObject = obj, gameResources = res, id = spawnedid, startPos = new Vector3(xPos, yPos, 0.0f), rot = Random.Range(-180.0f, 180.0f), timeSpawned = GameManager.Instance.gameTime
        };

        // Add the new GameResourcesObject to the spawnedGameResourcesObjects List
        spawnedGameResourcesObjects.Add(gameResourcesObject);

        switch (gameResourcesObject.gameResources.resType) {
            case GameResourcesType.Meteorite:
                spawnedMeteorite.Add(gameResourcesObject);
                break;
            case GameResourcesType.Dark_Dust:
                spawnedDarkDust.Add(gameResourcesObject);
                break;
            case GameResourcesType.Mana_Water:
                spawnedManaWater.Add(gameResourcesObject);
                break;
            case GameResourcesType.Nutri_Jewel:
                spawnedNutriJewel.Add(gameResourcesObject);
                break;
            case GameResourcesType.Slime_Stone:
                spawnedSlimeStone.Add(gameResourcesObject);
                break;
        }
        //Debug.Log("Spawned "+ obj.name + " is at " + obj.transform.position); // For debugging
    }

    // To Destroy an actual GameResources that interactable during game.
    private void DeSpawn(GameResourcesObject obj) {
        StartCoroutine(DestroyObject(obj));
        // Debug.Log(obj.gameObject.name +"(id=" + obj.id + " destroyed"); // For debugging
    }

    private IEnumerator DestroyObject(GameResourcesObject obj) {
        obj.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        obj.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f); // wait 0.5s
        spawnedGameResourcesObjects.Remove(obj);
        switch (obj.gameResources.resType) {
            case GameResourcesType.Meteorite:
                spawnedMeteorite.Remove(obj);
                break;
            case GameResourcesType.Dark_Dust:
                spawnedDarkDust.Remove(obj);
                break;
            case GameResourcesType.Mana_Water:
                spawnedManaWater.Remove(obj);
                break;
            case GameResourcesType.Nutri_Jewel:
                spawnedNutriJewel.Remove(obj);
                break;
            case GameResourcesType.Slime_Stone:
                spawnedSlimeStone.Remove(obj);
                break;
        }
        Destroy(obj.gameObject);
        Destroy(obj.gameResources);
    }

    // To instantiate a GameObject
    private GameObject CreateGameObject(GameResources res, int id, Vector3 pPos, Vector3 pRot) {
        GameObject obj = new GameObject(res.resName + "-" + (id-'0').ToString());
        obj.GetComponent<Transform>().transform.position = pPos;
        obj.GetComponent<Transform>().transform.eulerAngles = pRot;
        obj.transform.parent = this.transform; // Attach the new GameObject to GameResSystem gameobject
        SpriteRenderer _renderer = obj.AddComponent<SpriteRenderer>() as SpriteRenderer;
        _renderer.sprite = res.resSprite;
        obj.transform.parent = transform;
        
        // TODO: Maybe we need to add collider and other components...
        // ...
        obj.AddComponent<PolygonCollider2D>();
        obj.GetComponent<PolygonCollider2D>().isTrigger = true;
        obj.AddComponent<GameResourcesBehaviour>();
        obj.AddComponent<Animator>();
        obj.GetComponent<Animator>().runtimeAnimatorController = res.animatorController;

        return obj;
    }
    
    // a helper function that will return a Base GameResources Type by passing a name
    private GameResources GetResourcesWithName(string name) {
        foreach (GameResources res in gameResources) {
                if (res.resName.Equals(name)) return res;
        }
        throw new ArgumentNullException("Resources not exist, name: " + name);
    }

    // Generate ID for each independent spawned GameResourcesObject
    private int GenID() {
        int result = Mathf.FloorToInt(Random.Range(0, maxSpawnedGameResources-1));
        if (IsIDRepeated(result)) {
            result = result*1000 + (int)GameManager.Instance.gameTime*100;
        }
        return result;
    }
    
    // Check if current ID is existed
    private bool IsIDRepeated(int testID) {
        foreach (GameResourcesObject obj in spawnedGameResourcesObjects) {
            if (obj.id.Equals(testID)) return true;
        }
        return false;
    }
    private bool Overlapping(float xPos, float yPos) {
        for (int i=0 ; i<spawnedGameResourcesObjects.Count ; i++) {
            // if any spawnedGameResourcesObjects HP is below 0, Destroy it.
            if (Mathf.Sqrt(
                Mathf.Pow((xPos-spawnedGameResourcesObjects[i].gameObject.transform.position.x), 2)
                + Mathf.Pow((yPos-spawnedGameResourcesObjects[i].gameObject.transform.position.y), 2)) < 2.0f) {
                return true;
            }
        }
        return false;
    }

    public GameResourcesObject FindResourceObjectWithGameObj(GameObject gameObj) {
        for (int i = 0; i < spawnedGameResourcesObjects.Count; i++) {
            if (spawnedGameResourcesObjects[i].gameObject.name.Equals(gameObj.name)) {
                return spawnedGameResourcesObjects[i];
            }
        }
        throw new ArgumentNullException(gameObj.name + " is not in spawned list");
    }
}
