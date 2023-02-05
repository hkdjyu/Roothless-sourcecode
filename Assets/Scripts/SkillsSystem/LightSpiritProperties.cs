using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpiritProperties : MonoBehaviour
{
    public GameObject HealSphere;
    public float CD = 0.2f;
    public float HealAmount = 10.0f;
    public float timeSpawned = 0.0f;
    private bool wandering = false;
    private float timeStartWandering = 0.0f;
    private float randomDir = 0.0f;
    private Vector3 MoveVector;
    private Vector3 resultLoc;
    private Vector3 locationStartWandering = Vector3.zero;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    private GameObject targetedObj;
    private float healTime = 0.0f;
    private Vector3 tempVect = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = spriteArray[(int)((GameManager.Instance.gameTime - timeSpawned)*3.5f)%6];
        spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (healNeeded()) {
            float distance = 99.0f;
            wandering = false;
            //Heal Tree Priority
            if (FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().HP < FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().maxHP) {
                distance = Vector3.Distance(this.transform.position, FindObjectOfType<Tree>().gameObject.transform.position);
                targetedObj = FindObjectOfType<Tree>().gameObject;
            }
            //Else heal nearest root
            else {
                foreach (GameObject obj in FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().Roots) {
                    if (Vector3.Distance(this.transform.position, obj.gameObject.transform.position) < distance
                        && obj.GetComponent<Root>().HP < obj.GetComponent<Root>().maxHP) {
                        distance = Vector3.Distance(this.transform.position, obj.gameObject.transform.position);
                        targetedObj = obj;
                    }
                }
            }
            //Move to target
            if (distance > 3.5f) {
                tempVect = targetedObj.transform.position - this.transform.position;
                this.transform.position += tempVect.normalized * Time.deltaTime * 1.5f;
            }
            //Start Healing
            else {
                if (GameManager.Instance.gameTime - healTime > CD) {
                    HealSphere.GetComponent<HealSphereProperties>().dir = (targetedObj.transform.position - this.transform.position).normalized;
                    HealSphere.GetComponent<HealSphereProperties>().healAmount = HealAmount;
                    Spawn(HealSphere, this.transform.position, Vector3.zero);
                    
                    healTime = GameManager.Instance.gameTime;
                }
            }
        }
        else {
            if (!wandering) {
                timeStartWandering = GameManager.Instance.gameTime;
                locationStartWandering = this.gameObject.transform.position;
                wandering = true;
                do {
                    randomDir = Random.Range(-180.0f, 180.0f);
                    resultLoc = locationStartWandering + new Vector3(3.0f*Mathf.Cos(randomDir*Mathf.Deg2Rad), 3.0f*Mathf.Sin(randomDir*Mathf.Deg2Rad), 0.0f);
                }
                while(resultLoc.x < -Camera.main.orthographicSize * 1.0f ||
                    resultLoc.x > Camera.main.orthographicSize * 1.0f ||
                    resultLoc.y < -Camera.main.orthographicSize * 0.5625f ||
                    resultLoc.y > Camera.main.orthographicSize * 0.5625f);
            }
            if (GameManager.Instance.gameTime - timeStartWandering > 0.5f + (locationStartWandering.x-(int)locationStartWandering.x)*0.5f) {
                if (GameManager.Instance.gameTime - timeStartWandering < 1.5f + (locationStartWandering.x-(int)locationStartWandering.x)*0.5f) {
                    this.gameObject.transform.position
                        = locationStartWandering
                        + new Vector3(3.0f*Mathf.Cos(randomDir*Mathf.Deg2Rad)*(GameManager.Instance.gameTime - timeStartWandering-0.5f-(locationStartWandering.x-(int)locationStartWandering.x)*0.5f),
                                    3.0f*Mathf.Sin(randomDir*Mathf.Deg2Rad)*(GameManager.Instance.gameTime - timeStartWandering-0.5f-(locationStartWandering.x-(int)locationStartWandering.x)*0.5f),
                                    0.0f);
                }
                else if  (GameManager.Instance.gameTime - timeStartWandering > 1.832f + (locationStartWandering.x-(int)locationStartWandering.x)*0.5f) {
                    wandering = false;
                }
            }
        }
    }
    
    bool healNeeded() {
        if (FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().HP < FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().maxHP) {
            return true;
        }
        foreach (GameObject obj in FindObjectOfType<Tree>().gameObject.GetComponent<Tree>().Roots) {
            if (obj.GetComponent<Root>().HP < obj.GetComponent<Root>().maxHP) {
                return true;
            }
        }
        return false;
    }
    
    private void Spawn(GameObject gameObj, Vector3 pPos, Vector3 pRot) {
        GameObject newGameObj = Instantiate(gameObj);
        newGameObj.transform.parent = transform;
        newGameObj.transform.position = pPos;
        newGameObj.transform.eulerAngles = pRot;
    }
}
