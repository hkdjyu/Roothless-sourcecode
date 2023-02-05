using System;
using System.Collections;
using UnityEngine;

public class GameResourcesBehaviour : MonoBehaviour {
    public Animator anim;
    public GameResourcesObject resourcesObject;
    
    private void Start() {
        anim = GetComponent<Animator>();
        //FindObjectOfType<GameResourcesSystem>().maxSpawnedManaWater = 5;
        resourcesObject = transform.parent.GetComponent<GameResourcesSystem>()
            .FindResourceObjectWithGameObj(transform.gameObject);

    }
    
    private float previousTime = 0f;
    private void Update() {
        if (transform.name.Contains("DarkDust")) {
            if (resourcesObject.gameResources.isAbsorbing) {
                anim.SetBool("Dust", true);
                resourcesObject.gameResources.absorbRate = 2.0f;
                resourcesObject.gameResources.canAttractEnemy = true;
            }
        }
        if (transform.name.Contains("Meteorite")) {
            if (GameManager.Instance.gameTime - resourcesObject.timeSpawned > 8.0f) {
                anim.SetBool("Grounded", true);
                resourcesObject.gameResources.canAttractEnemy = true;
            }
        }
        if (GameManager.Instance.gameTime - previousTime >= 1.0f) {
            if (resourcesObject.gameResources.isAbsorbing) {
                resourcesObject.gameResources.resAmount -= resourcesObject.gameResources.absorbRate * resourcesObject.gameResources.absorbMultiplier; // some problem with rate, with Tree.absorb
                // Debug.Log("Reduce Rate = " + resourcesObject.gameResources.absorbRate);
                // Debug.Log(transform.name + " resAmount = " + resourcesObject.gameResources.resAmount);
            }
            previousTime = GameManager.Instance.gameTime;
        }

        if (transform.name.Contains("Tornado")) {
            if (resourcesObject.gameResources.isAbsorbing) {
                resourcesObject.gameResources.canAttractEnemy = true;
            }
            else {
                resourcesObject.gameResources.canAttractEnemy = false;
            }
        }
    }

    public void Hurt(float reduceHP) {
        resourcesObject.gameResources.HP -= reduceHP;
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(HurtFinish(0.1f));
    }

    IEnumerator HurtFinish(float time) {
        yield return new WaitForSeconds(time);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
