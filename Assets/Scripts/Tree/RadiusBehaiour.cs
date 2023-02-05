using System;
using UnityEngine;

public class RadiusBehaiour : MonoBehaviour {
    private float radius;
    private bool isEnabled = false;

    // [SerializeField] private float multiplicationFactor = 1.8f;
    private void Start() {
        isEnabled = transform.GetComponent<SpriteRenderer>().enabled;
    }

    public void SetVisible(bool isVisible) {
        if (isEnabled != isVisible) {
            isEnabled = isVisible;
            // radius = transform.parent.parent.GetComponent<Root>().length * multiplicationFactor;
            // radius /= transform.parent.parent.localScale.x; // root localscale
            //Debug.Log("radius = " + radius);
            radius = 4f * 2f;
            transform.localScale = new Vector3(radius, radius, radius);
            transform.GetComponent<SpriteRenderer>().enabled = isVisible;
        }
    }
}
