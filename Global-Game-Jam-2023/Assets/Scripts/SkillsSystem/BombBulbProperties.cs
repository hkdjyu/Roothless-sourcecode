using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBulbProperties : MonoBehaviour
{
    public GameObject BombBulbCollider;
    public GameObject Spore;
    public bool collided = false;
    public float timeSpawned = 0.0f;
    public float timeTriggered = 0.0f;
    public float size = 1.0f;
    public float damage = 15.0f;
    public float bombingCD = 1.0f;
    private float lastBomb = 0.0f;
    private float triggerTimeLength = 15.0f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //spriteArray = Resources.LoadAll<Sprite>("RootSpikes-min");
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = spriteArray[(int)((GameManager.Instance.gameTime - timeSpawned)*2)%2];

        if (timeTriggered != 0.0f) {
            spriteRenderer.color
                = new Color(1, 0, 0,
                    Mathf.Atan(25.0f*Mathf.Sin((GameManager.Instance.gameTime - timeTriggered)*Mathf.PI/(triggerTimeLength * 2.0f) + 0.5f*Mathf.PI))/Mathf.Atan(25.0f));
            this.gameObject.transform.localScale
                = new Vector3(2.3f + Mathf.Sin((GameManager.Instance.gameTime - timeTriggered)*3.0f)*0.3f,
                    2.3f + Mathf.Cos((GameManager.Instance.gameTime - timeTriggered)*3.0f)*0.2f,
                    2.3f);
            if (GameManager.Instance.gameTime - lastBomb > bombingCD) {                
                Spore.GetComponent<SporeProperties>().timeSpawned = GameManager.Instance.gameTime;
                Spore.GetComponent<SporeProperties>().size = size;
                GameObject newObj2 = Instantiate(Spore,  this.gameObject.transform.position, Quaternion.identity);
                
                lastBomb = GameManager.Instance.gameTime;
                
                StartCoroutine(SpawnCollider());
            }
            if (GameManager.Instance.gameTime - timeTriggered > triggerTimeLength)
                Destroy(this.gameObject);
        }
    }

    private IEnumerator SpawnCollider() {
        BombBulbCollider.GetComponent<BombBulbColliderProperties>().attacked = false;
        BombBulbCollider.GetComponent<BombBulbColliderProperties>().size = size;
        BombBulbCollider.GetComponent<BombBulbColliderProperties>().damage = damage;
        yield return new WaitForSeconds(0.6f);
        BombBulbCollider.GetComponent<BombBulbColliderProperties>().timeSpawned = GameManager.Instance.gameTime;
        GameObject newObj = Instantiate(BombBulbCollider,  this.gameObject.transform.position, Quaternion.identity);
    }
}
