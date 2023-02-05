using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpikesProperties : MonoBehaviour
{
    public bool collided = false;
    public float timeSpawned = 0.0f;
    public float damage = 15.0f;
    public float size = 1.0f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    private float upTime = 1.7f;

    private List<GameObject> contactingEnemies = new List<GameObject>() {};

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        AudioManager.Instance.PlaySfx("Spawn1");
        //spriteArray = Resources.LoadAll<Sprite>("RootSpikes-min");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.transform.parent.name.Equals("EnemySystem")) {
            if(contactingEnemies.Contains(other.gameObject))
                return;
            contactingEnemies.Add(other.gameObject);
            Debug.Log("Hit");
            other.GetComponent<EnemyBehaviour>().Hurt(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        this.gameObject.transform.localScale
                = new Vector3(size, size, size);
        spriteRenderer.color
            = new Color(1, 1, 1,
                Mathf.Atan(20.0f*Mathf.Sin((GameManager.Instance.gameTime - timeSpawned)*Mathf.PI/(upTime * 2.0f) + 0.5f*Mathf.PI))/Mathf.Atan(20.0f));
        if(GameManager.Instance.gameTime - timeSpawned < 0.06f)
            spriteRenderer.sprite = spriteArray[0];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.12f)
            spriteRenderer.sprite = spriteArray[1];
        else if (GameManager.Instance.gameTime - timeSpawned < (upTime - 0.12f))
            spriteRenderer.sprite = spriteArray[2];
        else if (GameManager.Instance.gameTime - timeSpawned < (upTime - 0.06f))
            spriteRenderer.sprite = spriteArray[1];
        else if (GameManager.Instance.gameTime - timeSpawned < upTime)
            spriteRenderer.sprite = spriteArray[0];
        else {
            Destroy(this.gameObject);
        }
    }
}
