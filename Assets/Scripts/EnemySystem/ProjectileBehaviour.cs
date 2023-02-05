using System;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
    public Transform shootTarget;
    public AttackTargetType attackTargetTypeType;
    public float attackDamage;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.transform == shootTarget) {
            switch (attackTargetTypeType) {
                case AttackTargetType.Tree :
                    col.gameObject.GetComponent<Tree>().Hurt(attackDamage);
                    break;
                case AttackTargetType.Roots :
                    col.gameObject.GetComponent<Root>().Hurt(attackDamage);
                    break;
                case AttackTargetType.Resources :
                    col.gameObject.GetComponent<GameResourcesBehaviour>().Hurt(attackDamage);
                    break;
                case AttackTargetType.TreeAndRoots :
                    if (col.GetComponent<Tree>() == null) {
                        // Is root
                        col.gameObject.GetComponent<Root>().Hurt(attackDamage);
                    }
                    else {
                        col.gameObject.GetComponent<Root>().Hurt(attackDamage);
                    }
                    break;
            }
            DestroyProjectile();
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.transform == shootTarget) {
            DestroyProjectile();
        }
    }

    public void DestroyProjectile() {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
