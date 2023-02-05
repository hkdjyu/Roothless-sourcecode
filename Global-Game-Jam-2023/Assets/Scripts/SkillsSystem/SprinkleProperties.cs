using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleProperties : MonoBehaviour
{
    public Vector3 dir = Vector3.zero;
    public float attack;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    public float timeSpawned;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        timeSpawned = GameManager.Instance.gameTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.transform.parent.name.Equals("EnemySystem")) {
            Debug.Log("Hit");
            other.GetComponent<EnemyBehaviour>().Hurt(attack);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0.4f);
        if (dir.x < 0) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        this.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        this.transform.position += dir * Time.deltaTime * 3.5f;
        if(GameManager.Instance.gameTime - timeSpawned < 0.06f)
            spriteRenderer.sprite = spriteArray[0];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.12f)
            spriteRenderer.sprite = spriteArray[1];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.18f)
            spriteRenderer.sprite = spriteArray[2];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.24f)
            spriteRenderer.sprite = spriteArray[3];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.3f)
            spriteRenderer.sprite = spriteArray[4];
        else if (GameManager.Instance.gameTime - timeSpawned > 5.0f) {
            Destroy(this.gameObject);
        }
    }

}
