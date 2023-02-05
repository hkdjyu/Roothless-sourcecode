using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Rope : MonoBehaviour {
    public Rigidbody2D hook;
    public int numLinks = 10;
    public GameObject[] prefabRopeSegments;
    private void Start() {
        GenerateRope();
    }

    private void GenerateRope() {
        Rigidbody2D prevBody = hook;
        for (int i = 0; i < numLinks; i++) {
            GameObject newSegment = Instantiate(prefabRopeSegments[i]);
            newSegment.transform.parent = transform;
            newSegment.transform.position = transform.position;
            HingeJoint2D hj = newSegment.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBody;

            prevBody = newSegment.GetComponent<Rigidbody2D>();
        }
    }
}
