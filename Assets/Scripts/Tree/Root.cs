using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData {
     public GameObject contactingEnemy;
     public bool isAttacking = false;
 }

public class Root : MonoBehaviour {
    public Transform tree;
    public List<GameObject> RootSegments = new List<GameObject>();
    public Transform hook;
    public Transform end;
    public Transform radiusObj;
    public float length;
    public float tolerance = 1.0f;
    public bool isAbsorbing = false;
    public bool isRootOnDrag = false;
    public GameObject rootSegment;
    public GameObject absorbingResourcesObj;
    public List<AttackData> ContactingEnemyAttackDatas = new List<AttackData>();

    //UPGRADES
    public int tenacityLevel = 0;
    public int attackLevel = 0;
    public int speedLevel = 0;
    public int absorbLevel = 0;
    public float maxHP;
    public float HP;
    public float attackDamage;
    public float attackCD;
    public float speed;
    public float absorb;

    private void Start() {
        tree = transform.parent;
        GetAllRootSegment(transform.gameObject);
        hook = FindHook();
        end = FindEnd();
        radiusObj = hook.GetChild(0);
        // Debug.Log(hook.position + " end" + end.position);
        length = (hook.position - end.position).magnitude + 1.0f;
        // Debug.Log("root length = " + length);
        maxHP = 300.0f;
        HP = 300.0f;
        attackDamage = 30.0f;
        attackCD = 1.0f;
        speed = 0.17f;
        absorb = 4.5f;
        //ChangeOnDragCursorSpeed(speed);
    }

    private void Update() {
        if (isAbsorbing) rootSegment.transform.position = absorbingResourcesObj.transform.position;
        if (HP <= 0.0f) {
            DeSpawn(this.gameObject);
        }

        if (isRootOnDrag) {
            Attack();
        }
        else {
            length = (hook.position - end.position).magnitude + 1.0f;
        }
        
        // Update RootScale
        float scale = Mathf.Clamp(GameManager.Instance.gameTime * (4f/1500) + 1 , 1.0f, 5f);
        transform.localScale = new Vector3(scale, scale, scale);
        
        MaxHPAutoGrow();
        RecoverHPAuto();
    }
    private void MaxHPAutoGrow() {
        maxHP = Mathf.Clamp(300 + (GameManager.Instance.gameTime * (1100/1500f)), 300f, 2400f);
    }

    private void RecoverHPAuto() {
        float recoverHPAutoRate = 1.5f;
        if(HP<maxHP)
            HP += Time.deltaTime * recoverHPAutoRate + Time.deltaTime * ((maxHP <= 2400) ? 1100 / 1500f : 0);
    }

    private void GetAllRootSegment(GameObject obj) {
        GetAllRootSegmentRecursive(obj);
        for (int i = 0; i<RootSegments.Count; i++) {
            if (RootSegments[i].GetComponent<RootSegment>() == null) {
                //Debug.Log("Remove " + RootSegments[i].name);
                RootSegments.Remove(RootSegments[i]);
                i--;
            }
        }
    }
    
    private void GetAllRootSegmentRecursive(GameObject obj){
        if (null == obj)
            return;

        foreach (Transform child in obj.transform){
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            RootSegments.Add(child.gameObject);
            GetAllRootSegmentRecursive(child.gameObject);
        }
    }
    
    private Transform FindHook() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).name.Equals("Hook")) return transform.GetChild(i);
        }
        throw new ArgumentNullException(transform.name + "Cannot Find Hook!!");
    }
    private Transform FindEnd() {
        return RootSegments[RootSegments.Count - 1].transform;
    }
    
    public void SetRadiusVisible(bool isVisible) {
        radiusObj.GetComponent<RadiusBehaiour>().SetVisible(isVisible);
    }

    public void Absorb(GameObject pRootSegment, GameObject obj) {
        // Check only last 3 segment absorbing
        String segmentPosString = pRootSegment.name.Remove(0, 5);
        int segmentPos = int.Parse(segmentPosString);
        if (RootSegments.Count - segmentPos > 4) {
            return; // return if not The last 3 segment
        }
        
        /*
         * Action start here
         */
        AudioManager.Instance.PlaySfx("Resources1");
        // Update root
        if(absorbingResourcesObj == obj) return; // Early return if already absorbing
        isAbsorbing = true;
        rootSegment = pRootSegment;
        absorbingResourcesObj = obj;
        ChangeColor(new Color(255/255f, 120/255f, 120/255f));

        // Update gameResourcesObject
        GameResourcesObject resourcesObject = 
            obj.transform.parent.GetComponent<GameResourcesSystem>().FindResourceObjectWithGameObj(obj);
        resourcesObject.gameResources.isAbsorbing = true;
        
        // Update Tree
        resourcesObject.gameResources.absorbMultiplier = absorb;
        tree.GetComponent<Tree>().Absorb(resourcesObject);
    }

    public void Release(GameObject pRootSegment, GameObject obj) {
        // Debug.Log("Root Release()");
        String segmentPosString = pRootSegment.name.Remove(0, 5);
        int segmentPos = int.Parse(segmentPosString);
        if (RootSegments.Count - segmentPos > 4) {
            return; // return if not The last 3 segment
        }
        
        /*
         * Action start here
         */
        
        // Update root
        isAbsorbing = false;
        rootSegment = null;
        absorbingResourcesObj = null;
        ChangeColor(new Color(255/255f, 255/255f, 255/255f));

        // update gameResourcesObject
        GameResourcesObject resourcesObject = 
            obj.transform.parent.GetComponent<GameResourcesSystem>().FindResourceObjectWithGameObj(obj);
        resourcesObject.gameResources.isAbsorbing = false;
        
        // Update Tree
        tree.GetComponent<Tree>().Release(resourcesObject);
    }

    private void ChangeColor(Color endColor) {
        StopAllCoroutines();
        StartCoroutine(ChangeColourCoroutine(transform.gameObject.GetComponent<SpriteRenderer>().color, endColor));
    }
    
    private IEnumerator ChangeColourCoroutine(Color startColor, Color endColor)
    {
        float tick = 0f;
        while (transform.gameObject.GetComponent<SpriteRenderer>().color != endColor)
        {
            tick += Time.deltaTime * 1.0f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }

    /*
    public void ChangeOnDragCursorSpeed(float newCursorSpeed) {
        for (int i = 0; i < RootSegments.Count; i++) {
            RootSegments[i].gameObject.GetComponent<RootSegment>().cursorSpeed = newCursorSpeed;
        }
    }
    */
    
    private void DeSpawn(GameObject obj) {
        StartCoroutine(DestroyObject(obj));
        // Debug.Log(obj.gameObject.name +"(id=" + obj.id + " destroyed"); // For debugging
    }

    private IEnumerator DestroyObject(GameObject obj) {
        obj.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        tree.GetComponent<Tree>().Roots.Remove(obj);
        Destroy(obj.gameObject);
    }

    protected virtual void Attack() {
        if (ContactingEnemyAttackDatas.Count == 0) return;
        AudioManager.Instance.PlaySfx("hit1");;
        foreach (AttackData data in ContactingEnemyAttackDatas) {
            // Debug.Log(data.isAttacking);
            if (data.isAttacking) continue;
            if (data.contactingEnemy.GetComponent<EnemyBehaviour>().isDie) continue;
            data.isAttacking = true;
            data.contactingEnemy.GetComponent<EnemyBehaviour>().Hurt(attackDamage);
            StartCoroutine(FinishAttack(attackCD, data));
            Vector2 direction = ((Vector2)data.contactingEnemy.transform.position - (Vector2)transform.position)
                .normalized;
            Vector2 force = direction * 200.0f;
            data.contactingEnemy.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }

    private IEnumerator FinishAttack(float cd, AttackData data) {
        yield return new WaitForSeconds(cd);
        data.isAttacking = false;
    }
    
    public void Hurt(float reduceHP) {
        AudioManager.Instance.PlaySfx("Hit2");
        HP -= reduceHP;
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(HurtFinish(0.1f));
    }

    IEnumerator HurtFinish(float time) {
        yield return new WaitForSeconds(time);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void AddContactingEnemyAttackDatas(AttackData testData) {
        bool canAdd = true;
        foreach (var data in ContactingEnemyAttackDatas) {
            if (testData.contactingEnemy == data.contactingEnemy) {
                canAdd = false;
                break;
            }
        }
        if(canAdd) ContactingEnemyAttackDatas.Add(testData);
    }

    public void RemoveContactingEnemyAttackDatas(GameObject obj) {
        if (ContactingEnemyAttackDatas.Count == 0) return;
        for (int i = 0; i < ContactingEnemyAttackDatas.Count; i++) {
            if (ContactingEnemyAttackDatas[i].contactingEnemy == obj) ContactingEnemyAttackDatas.Remove(ContactingEnemyAttackDatas[i]);
        }
    }
}
