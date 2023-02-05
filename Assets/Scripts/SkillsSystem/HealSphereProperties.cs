using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSphereProperties : MonoBehaviour
{
    public Vector3 dir = Vector3.zero;
    public float healAmount;
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
        if (other.gameObject.name.Contains("bone")) {
            other.GetComponent<RootSegment>().root.GetComponent<Root>().HP += healAmount;
            if (other.GetComponent<RootSegment>().root.GetComponent<Root>().HP+0.2f > other.GetComponent<RootSegment>().root.GetComponent<Root>().maxHP) {
                other.GetComponent<RootSegment>().root.GetComponent<Root>().HP = other.GetComponent<RootSegment>().root.GetComponent<Root>().maxHP;
            }
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "tree") {
            other.GetComponent<Tree>().HP += healAmount;
            if (other.GetComponent<Tree>().HP+0.2f > other.GetComponent<Tree>().maxHP) {
                other.GetComponent<Tree>().HP = other.GetComponent<Tree>().maxHP;
            }
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        this.transform.position += dir * Time.deltaTime * 3.5f;
        spriteRenderer.sprite = spriteArray[(int)((GameManager.Instance.gameTime - timeSpawned)*15)%4];
        if(GameManager.Instance.gameTime - timeSpawned > 5.0f) {
            Destroy(this.gameObject);
        }
    }

}

