using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public GameObject RootSpikes;
    public GameObject BombBulb;

    public float rootSpikes_damage;
    public float rootSpikes_size;
    public float bombBulb_damage;
    public float bombBulb_size;

    private float lastUsedRootSpikes = -20.0f;
    private float lastUsedBombBulb = -20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("F is pressed");
            if (FindObjectOfType<ShopSystem>().rootSpikesLevel > 0) {
                if (GameManager.Instance.gameTime - lastUsedRootSpikes > 10.0f) {
                    RootSpikes.GetComponent<RootSpikesProperties>().damage = rootSpikes_damage;
                    RootSpikes.GetComponent<RootSpikesProperties>().size = rootSpikes_size;
                    RootSpikes.GetComponent<RootSpikesProperties>().timeSpawned = GameManager.Instance.gameTime;
                    RootSpikes.GetComponent<RootSpikesProperties>().collided = false;
                    // Instantiate(RootSpikes, GameObject.FindWithTag("Cursor").transform.position, Quaternion.identity);
                    Spawn(RootSpikes, GameObject.FindWithTag("Cursor").transform.position, Vector3.zero);
                    lastUsedRootSpikes = GameManager.Instance.gameTime;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("G is pressed");
            if (FindObjectOfType<ShopSystem>().bombBulbLevel > 0) {
                if (GameManager.Instance.gameTime - lastUsedBombBulb > 15.0f) {
                    BombBulb.GetComponent<BombBulbProperties>().damage = bombBulb_damage;
                    BombBulb.GetComponent<BombBulbProperties>().size = bombBulb_size;
                    BombBulb.GetComponent<BombBulbProperties>().timeSpawned = GameManager.Instance.gameTime;
                    BombBulb.GetComponent<BombBulbProperties>().timeTriggered = GameManager.Instance.gameTime;
                    BombBulb.GetComponent<BombBulbProperties>().collided = false;
                    // Instantiate(BombBulb, GameObject.FindWithTag("Cursor").transform.position, Quaternion.identity);
                    Spawn(BombBulb, GameObject.FindWithTag("Cursor").transform.position, Vector3.zero);
                    lastUsedBombBulb = GameManager.Instance.gameTime;
                }
            }
        }
    }

    
    private void Spawn(GameObject gameObj, Vector3 pPos, Vector3 pRot) {
        GameObject newGameObj = Instantiate(gameObj);
        newGameObj.transform.parent = transform;
        newGameObj.transform.position = pPos;
        newGameObj.transform.eulerAngles = pRot;
    }
    
}

