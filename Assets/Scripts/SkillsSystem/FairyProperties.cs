using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyProperties : MonoBehaviour
{
    public GameObject Sprinkle;
    public float CD = 0.5f;
    public float attack = 15.0f;
    public float timeSpawned = 0.0f;
    private bool wandering = false;
    private float timeStartWandering = 0.0f;
    private float randomDir = 0.0f;
    private Vector3 MoveVector;
    private Vector3 resultLoc;
    private Vector3 locationStartWandering = Vector3.zero;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    private EnemyObject targetedObj;
    private float attackTime = 0.0f;
    private Vector3 tempVect = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = spriteArray[(int)((GameManager.Instance.gameTime - timeSpawned)*2)%2];
        this.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        if (targetDetected()) {
            float distance = 99.0f;
            wandering = false;
            foreach (EnemyObject obj in FindObjectOfType<EnemySystem>().spawnedEnemyObjects) {
                if (Vector3.Distance(this.transform.position, obj.gameObject.transform.position) < distance) {
                    distance = Vector3.Distance(this.transform.position, obj.gameObject.transform.position);
                    targetedObj = obj;
                }
            }
            if (distance > 4.5f) {
                tempVect = targetedObj.gameObject.transform.position - this.transform.position;
                this.transform.position += tempVect.normalized * Time.deltaTime * 1.5f;
                if (tempVect.x < 0) {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else {
                if (GameManager.Instance.gameTime - attackTime > CD) {
                    Sprinkle.GetComponent<SprinkleProperties>().dir = (targetedObj.gameObject.transform.position - this.transform.position).normalized;
                    Sprinkle.GetComponent<SprinkleProperties>().attack = attack;
                    Spawn(Sprinkle, this.transform.position, Vector3.zero);
                    
                    attackTime = GameManager.Instance.gameTime;
                }
            }
            //for loop enemies, get closest enemy, closest enemy direction, move to that direction
            //if not close enough, move that direction
            //else don't move and start attacking
                //Fairy Projectiles
                //Attack Animations
        }
        else {
            //RANDOMIZE MOVEMENT
            if (!wandering) {
                timeStartWandering = GameManager.Instance.gameTime;
                locationStartWandering = this.gameObject.transform.position;
                wandering = true;
                do {
                    randomDir = Random.Range(-180.0f, 180.0f);
                    resultLoc = locationStartWandering + new Vector3(5.0f*Mathf.Cos(randomDir*Mathf.Deg2Rad), 5.0f*Mathf.Sin(randomDir*Mathf.Deg2Rad), 0.0f);
                }
                while(resultLoc.x < -Camera.main.orthographicSize * 1.3333f ||
                    resultLoc.x > Camera.main.orthographicSize * 1.3333f ||
                    resultLoc.y < -Camera.main.orthographicSize * 0.75f ||
                    resultLoc.y > Camera.main.orthographicSize * 0.75f);
            }
            //FLIP DIRECTION (bug)
            if (randomDir > -90.0f && randomDir < 90.0f) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            //MOVEMENT
            if (GameManager.Instance.gameTime - timeStartWandering > 1.0f + (locationStartWandering.x-(int)locationStartWandering.x)) {
                if (GameManager.Instance.gameTime - timeStartWandering < 2.0f + (locationStartWandering.x-(int)locationStartWandering.x)) {
                    this.gameObject.transform.position
                        = locationStartWandering
                        + new Vector3(5.0f*Mathf.Cos(randomDir*Mathf.Deg2Rad)*(GameManager.Instance.gameTime - timeStartWandering-1.0f-(locationStartWandering.x-(int)locationStartWandering.x)),
                                    5.0f*Mathf.Sin(randomDir*Mathf.Deg2Rad)*(GameManager.Instance.gameTime - timeStartWandering-1.0f-(locationStartWandering.x-(int)locationStartWandering.x)),
                                    0.0f);
                }
                else if  (GameManager.Instance.gameTime - timeStartWandering > 2.8f + (locationStartWandering.x-(int)locationStartWandering.x)) {
                    wandering = false;
                }
            }
        }
    }

    bool targetDetected() {
        return (FindObjectOfType<EnemySystem>().spawnedEnemyObjects.Count > 0);
    }
    
    private void Spawn(GameObject gameObj, Vector3 pPos, Vector3 pRot) {
        GameObject newGameObj = Instantiate(gameObj);
        newGameObj.transform.parent = transform;
        newGameObj.transform.position = pPos;
        newGameObj.transform.eulerAngles = pRot;
    }
}