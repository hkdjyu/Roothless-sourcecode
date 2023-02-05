using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeProperties : MonoBehaviour
{
    public float timeSpawned = 0.0f;
    public float size = 1.0f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(3.5f*size, 3.5f*size, 3.5f*size);
        if(GameManager.Instance.gameTime - timeSpawned < 0.1f)
            spriteRenderer.sprite = spriteArray[0];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.2f)
            spriteRenderer.sprite = spriteArray[1];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.3f)
            spriteRenderer.sprite = spriteArray[2];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.4f)
            spriteRenderer.sprite = spriteArray[3];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.5f)
            spriteRenderer.sprite = spriteArray[4];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.6f)
            spriteRenderer.sprite = spriteArray[5];
        else if (GameManager.Instance.gameTime - timeSpawned < 0.7f)
            spriteRenderer.sprite = spriteArray[6];
        else {
            Destroy(this.gameObject);
        }
    }
}