using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Pathfinding;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyBehaviour: MonoBehaviour {

    public EnemyObject enemyObj;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    public Transform pathTarget;
    public float speed;
    public float nextWaypointDistance = 3f;

    protected Path _path;
    protected int _currentWayPoint = 0;
    protected bool _reachedEndOfPath = false;

    protected Seeker _seeker;
    protected Rigidbody2D _rb;

    protected Transform attackTarget;
    protected bool isAttacking = false;
    protected bool isHurt = false;
    public bool isDie = false;

    private EnemySystem enemySystem;
    private Transform treeTransform;
    private Transform resSystemTransform;
    protected void Start() {
        enemySystem = transform.parent.GetComponent<EnemySystem>();
        treeTransform = FindObjectOfType<Tree>().transform;
        resSystemTransform = FindObjectOfType<GameResourcesSystem>().transform;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        enemyObj = enemySystem.FindEnemyObjectWithGameObj(this.gameObject);
        pathTarget = FindTarget();
        speed = enemyObj.enemy.speed;
        
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        // _seeker.StartPath(_rb.position, target.position, OnPathComplete);

        InvokeRepeating("UpdateTarget", 0f, 1.0f);
    }

    protected void UpdatePath() {
        if(_seeker.IsDone())
            _seeker.StartPath(_rb.position, pathTarget.position, OnPathComplete);
    }

    protected void UpdateTarget() {
        pathTarget = FindTarget();
    }

    private void Update() {
        if (enemyObj.enemy.HP <= 0) {
            if (!isDie) Die();
        }

        if (attackTarget.transform != null) {
            if ((transform.position - attackTarget.position).magnitude < enemyObj.enemy.attackRange) {
                if (!isAttacking) Attack();
            }
        }

        if (GameManager.Instance.gameTime > 1080.0f) {
            switch (enemyObj.enemy.name) {
                case "QuinJet":
                    enemyObj.enemy.HP += (GameManager.Instance.gameTime - 1080);
                    break;
                case "Mech":
                    enemyObj.enemy.HP += (GameManager.Instance.gameTime - 1080) * 0.5f;
                    break;
                case "Terraformer":
                    enemyObj.enemy.HP += (GameManager.Instance.gameTime - 1080);
                    break;
                case "AssaultRobort":
                    enemyObj.enemy.HP += (GameManager.Instance.gameTime - 1080) * 0.5f;
                    break;
            }    
        }
    }

    protected void FixedUpdate() {
        if (isDie) return;
        if (pathTarget == null) pathTarget = FindTarget();
        if (_path != null) {
            if (_currentWayPoint >= _path.vectorPath.Count) {
                _reachedEndOfPath = true;
                return;
            }
            else {
                _reachedEndOfPath = false;
            }
            Move();
            float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWayPoint]);
            if (distance < nextWaypointDistance) {
                _currentWayPoint++;
            }
        }
    }

    protected void OnPathComplete(Path p) {
        if (!p.error) {
            _path = p;
            _currentWayPoint = 0;
        }
    }

    protected virtual Transform FindTarget() {
        Transform result;
        switch (enemyObj.enemy.attackTargetType) {
            case AttackTargetType.Tree :
                result = treeTransform;
                break;
            case AttackTargetType.Resources :
                result =  FindClosestResources();
                break;
            case AttackTargetType.Roots :
                result = FindClosestRoot();
                break;
            case AttackTargetType.TreeAndRoots:
                result = FindClosestTreeOrRoot();
                break;
            default:
                throw new ArgumentNullException("unknown target:" + enemyObj.enemy.attackTargetType.ToString());
        }

        attackTarget = result;
        return result;
    }

    protected virtual void Move() {
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWayPoint] - _rb.position + new Vector2(Random.Range(-1,1),Random.Range(-1,1))).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);
        _rb.AddForce(force);
        if (enemyObj.enemy.enemyName.Contains("Dragon")) {
            if (direction.x > 0) {
                _spriteRenderer.flipX = false;
            }
            else {
                _spriteRenderer.flipX = true;
            } 
        }
        else {
            if (direction.x > 0) {
                _spriteRenderer.flipX = true;
            }
            else {
                _spriteRenderer.flipX = false;
            }
        }
        
        //_rb.velocity = force;

        if (enemyObj.enemy.attackMode == AttackMode.Ranged) {
            if (((Vector2)attackTarget.position - _rb.position +
                 new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).magnitude <=
                enemyObj.enemy.attackRange / 2f) {
                /* Debug.Log("currentDistance" + ((Vector2)attackTarget.position - _rb.position +
                               new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).magnitude.ToString() +
                          " Range/2 = "+ enemyObj.enemy.attackRange / 2f +
                    "\nAdded repulsive force to " + this.gameObject.name);
                */
                // Debug.Log("\nAdded repulsive force to " + this.gameObject.name);
                Vector2 negativeDirection = new Vector2(direction.x * -1, direction.y * -1);
                Vector2 repulsiveForce = negativeDirection * (speed * Time.deltaTime);
                _rb.AddForce(repulsiveForce);
            }
        }
    }

    protected virtual void Attack() {
        if(isAttacking) return;
        if (attackTarget == null || attackTarget == transform) return;
        // Debug.Log(transform.name + " Attack()");
        isAttacking = true;
        _animator.Play("Attack");
        AnimationEventDispatcher dispatcher = GetComponent<AnimationEventDispatcher>();
        dispatcher.OnAnimationComplete.AddListener(OnAnimationComplete);

        if (enemyObj.enemy.attackMode == AttackMode.Melee) {
            // AudioManager.Instance.PlaySfx("hit1");
            switch (enemyObj.enemy.attackTargetType) {
                case AttackTargetType.Tree :
                    attackTarget.GetComponent<Tree>().Hurt(enemyObj.enemy.attackDamage);
                    break;
                case AttackTargetType.Roots :
                    attackTarget.GetComponent<RootSegment>().root.GetComponent<Root>().Hurt(enemyObj.enemy.attackDamage);
                    break;
                case AttackTargetType.Resources :
                    attackTarget.GetComponent<GameResourcesBehaviour>().Hurt(enemyObj.enemy.attackDamage);
                    break;
                case AttackTargetType.TreeAndRoots :
                    if (attackTarget.GetComponent<Tree>() != null) {
                        attackTarget.GetComponent<Tree>().Hurt(enemyObj.enemy.attackDamage);
                    }
                    else {
                        attackTarget.GetComponent<RootSegment>().root.GetComponent<Root>().Hurt(enemyObj.enemy.attackDamage);
                    }
                    break;
            }
        }
        
        else if (enemyObj.enemy.attackMode == AttackMode.Ranged) {
            Shoot(attackTarget);
        }

        StartCoroutine(FinishAttack(enemyObj.enemy.attackCD));
    }
    IEnumerator FinishAttack(float cd) {
        yield return new WaitForSeconds(cd);
        isAttacking = false;
        if(!isDie)_animator.Play("Move");
    }

    private void Shoot(Transform shootTarget) {
        // AudioManager.Instance.PlaySfx("Fire1");
        Vector2 direction = ((Vector2)shootTarget.transform.position - (Vector2)GetComponent<Rigidbody2D>().position).normalized;
        Vector2 force = direction * enemyObj.enemy.projectileSpeed;
        
        GameObject newProjectile = new GameObject("projectile-" + enemyObj.enemy.projectileSprite.name);
        newProjectile.transform.position = this.transform.position;
        newProjectile.transform.eulerAngles = new Vector3(0,0,Mathf.Atan(direction.y / direction.x) * (180 / (float)Math.PI) + ((direction.x < 0) ? 180f : 0f));
        // newProjectile.transform.eulerAngles = new Vector3(0,0,0);
        newProjectile.transform.parent = transform;
        
        SpriteRenderer spr = newProjectile.AddComponent<SpriteRenderer>();
        spr.sprite = enemyObj.enemy.projectileSprite;
        
        Rigidbody2D rb = newProjectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = enemyObj.enemy.projectileLinearDrag;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        BoxCollider2D col = newProjectile.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        ProjectileBehaviour pB = newProjectile.AddComponent<ProjectileBehaviour>();
        pB.shootTarget = attackTarget;
        pB.attackTargetTypeType = enemyObj.enemy.attackTargetType;
        pB.attackDamage = enemyObj.enemy.attackDamage;

        
        rb.AddForce(force);
    }

    public void Hurt(float reduceHP) {
        isHurt = true;
        enemyObj.enemy.HP -= reduceHP;
        _spriteRenderer.color = Color.red;
        // Debug.Log("Hurt! current HP = " + enemyObj.enemy.HP);
        StartCoroutine(HurtFinish(0.1f));
    }

    IEnumerator HurtFinish(float time) {
        yield return new WaitForSeconds(time);
        _spriteRenderer.color = Color.white;
        isHurt = false;
    }

    protected virtual void Die() {
        if(isDie) return;
        isDie = true;
        _animator.Play("Die");
        //AnimationEventDispatcher dispatcher = GetComponent<AnimationEventDispatcher>();
        //dispatcher.OnAnimationComplete.AddListener(_animator.runtimeAnimatorController.animationClips.Length);
        /*
        float animationLength = 0f;
        foreach (var clip in _animator.runtimeAnimatorController.animationClips) {
            if (clip.name.Equals("Die")) animationLength = clip.length;
        }
        */
        // StartCoroutine(WaitAnimationComplete(animationLength));
        StartCoroutine(WaitAnimationComplete(0.2f));
    }

    IEnumerator WaitAnimationComplete(float time) {
        yield return new WaitForSeconds(time);
        enemySystem.DeSpawn(enemyObj);
    }
    

    private void OnAnimationComplete(string animationName) {
        // Debug.Log(transform.name + " complete " + animationName);
        AnimationEventDispatcher dispatcher = GetComponent<AnimationEventDispatcher>();
        // if(isDie || animationName.Equals("Die")) enemySystem.DeSpawn(enemyObj);
        if(animationName.Equals("Attack")) _animator.Play("Move");
        dispatcher.OnAnimationComplete.RemoveListener(OnAnimationComplete);
    }
    

    private Transform FindClosestResources() {
        GameResourcesSystem resSystem = resSystemTransform.GetComponent<GameResourcesSystem>();
        if (resSystem.spawnedGameResourcesObjects.Count == 0) {
            Debug.LogWarning("No resources Found");
            return transform; // no resources then return itself
        }
        Transform closestRes = resSystem.spawnedGameResourcesObjects[0].gameObject.transform;
        foreach (var obj in resSystem.spawnedGameResourcesObjects) {
            if (!obj.gameResources.canAttractEnemy) continue;
            if ((transform.position - obj.gameObject.transform.position).magnitude <
                (transform.position - closestRes.position).magnitude) {
                closestRes = obj.gameObject.transform;
            }
        }
        return closestRes;
    }

    private Transform FindClosestRoot() {
        Tree tree = treeTransform.GetComponent<Tree>();
        if (tree.Roots.Count == 0) {
            Debug.LogWarning("No roots Found");
            return transform; // no roots then return itself
        }
        Transform closestRoot = tree.Roots[0].GetComponent<Root>().end.parent.parent.parent.transform; // the 4th end
        foreach (var obj in tree.Roots) {
            if ((transform.position - obj.GetComponent<Root>().end.parent.parent.parent.transform.position).magnitude <
                (transform.position - closestRoot.position).magnitude) {
                closestRoot = obj.GetComponent<Root>().end.parent.parent.parent.transform;
            }
        }
        return closestRoot;
    }
    private Transform FindClosestTreeOrRoot() {
        Transform closestRoot = FindClosestRoot();
        if (closestRoot != transform) {
            if ((transform.position - GameObject.FindWithTag("Tree").transform.position).magnitude <
                (transform.position - closestRoot.position).magnitude) {
                return closestRoot;
            }
            else {
                return treeTransform;
            }
        }
        else {
            return treeTransform;
        }
    }
}
