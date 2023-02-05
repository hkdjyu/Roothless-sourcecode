using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueElementBehaviour : MonoBehaviour {
    private Transform _transform;
    private void Start() {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.name.Contains("bone")) {
            _transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.name.Contains("bone")) {
            _transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }
}
