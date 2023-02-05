using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RootSegment : DragDrop {
    private Transform hook;
    private Transform end;
    public Transform root;
    private float length;
    private float tolerance = 1.0f;
    //public float cursorSpeed = 0.17f; // default 1.0, OnDrag = 0.2f
    private Rigidbody2D _rigidbody;
    private HingeJoint2D _hingeJoint2D;
    private Collider2D _collider;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _collider = GetComponent<Collider2D>();
        hook = FindHook(transform);
        end = FindEnd(transform);
        root = FindRoot();
        length = (hook.position - end.position).magnitude;

        RigidBodyInit();
        HingeJoint2DInit();
        _collider.isTrigger = true;

        if (transform.name.Equals("bone_1")) allowDrag = false;
    }

    protected override void OnLeftClickDown() {
        CursorController.onLeftClickDown -= OnLeftClickDown;
        CursorController.onLeftClickUp += OnLeftClickUp;
        offset = transform.position - GameObject.FindWithTag("Cursor").transform.position;
        isDragging = true;
        root.GetComponent<Root>().isRootOnDrag = true;
        CursorController.speed = root.GetComponent<Root>().speed;
    }
    
    protected override void OnLeftClickUp() {
        SetRadiusVisible(false);
        isDragging = false;
        root.GetComponent<Root>().isRootOnDrag = false;
        allowDrag = true;
        CursorController.speed = 1.0f;
        CursorController.onLeftClickUp -= OnLeftClickUp;
    }
    
    protected override void Update() {
        
        if (isDragging && allowDrag) {

            SetRadiusVisible(true);

            transform.position = GameObject.FindWithTag("Cursor").transform.position + offset;
            if ((hook.position - end.position).magnitude > length + tolerance && !root.GetComponent<Root>().isAbsorbing) {
                allowDrag = false;
            }
        }
        else {
            length = (hook.position - end.position).magnitude;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor"))
            CursorController.onLeftClickDown += OnLeftClickDown;
        else if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.name.Equals("GameResSystem")) {
            root.GetComponent<Root>().Absorb(transform.gameObject, col.gameObject);
        }
        else if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.name.Equals("EnemySystem")) {
            AttackData data = new AttackData() { contactingEnemy = col.gameObject, isAttacking = false };
            root.GetComponent<Root>().AddContactingEnemyAttackDatas(data);
        }
    }
    protected override void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor"))
            CursorController.onLeftClickDown -= OnLeftClickDown;
        else if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.name.Equals("GameResSystem")) {
            root.GetComponent<Root>().Release(transform.gameObject, col.gameObject);
        }
        else if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.name.Equals("EnemySystem")) {
            root.GetComponent<Root>().RemoveContactingEnemyAttackDatas(col.gameObject);
        }
    }

    private Transform FindHook(Transform child) {
        Transform result;
        if (child.name.Equals("Hook")) return child;
        if (child.parent.name != "Hook") {
            result = FindHook(child.parent);
        }
        else {
            result = child.parent;
        }
        return result;
    }

    private Transform FindEnd(Transform parent) {
        Transform result;
        if (parent.transform.childCount == 0) return parent;
        if (parent.GetChild(0).name != "bone_7") {
            result = FindEnd(parent.GetChild(0));
        }
        else {
            result = parent.GetChild(0);
        }
        return result;
    }

    private Transform FindRoot() {
        return hook.parent;
    }

    private void SetRadiusVisible(bool isVisible) {
        root.GetComponent<Root>().SetRadiusVisible(isVisible);
        // hook.GetChild(0).GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    private void RigidBodyInit() {
        _rigidbody.gravityScale = 0;
    }
    
    private void HingeJoint2DInit() {
        _hingeJoint2D.connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        _hingeJoint2D.autoConfigureConnectedAnchor = false;
        _hingeJoint2D.useLimits = true;
        JointAngleLimits2D angleLimits = new JointAngleLimits2D();
        angleLimits.max = (transform.name[5] - '0') * 10.0f;
        angleLimits.min = (transform.name[5] - '0') * -10.0f;
        _hingeJoint2D.limits = angleLimits;
    }
}

