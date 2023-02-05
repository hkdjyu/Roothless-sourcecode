using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBulbColliderProperties : MonoBehaviour
{
    public bool attacked = false;
    public float size = 1.0f;
    public float timeSpawned = 0.0f;
    public float damage = 0.0f;
    public SpriteRenderer spriteRenderer;

    private List<GameObject> contactingEnemies = new List<GameObject>() {};
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        //Invisible this
        spriteRenderer.color = new Color(0, 0, 0, 0);
        this.gameObject.transform.localScale
                = new Vector3(14.0f*size, 14.0f*size, 14.0f*size);
        if (!attacked) {
            //for each enemy touched minus HP
            attacked = true;
        }
        if (GameManager.Instance.gameTime - timeSpawned > 0.1f) {
            Destroy(this.gameObject);
        }
    }
}
