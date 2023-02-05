using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragDrop : MonoBehaviour {
    protected bool isDragging;
    public bool allowDrag = true;
    protected Vector3 offset;

    protected virtual void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor"))
            CursorController.onLeftClickDown += OnLeftClickDown;
    }
    protected virtual void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Cursor"))
            CursorController.onLeftClickDown -= OnLeftClickDown;
    }

    protected virtual void OnLeftClickDown() {
        CursorController.onLeftClickDown -= OnLeftClickDown;
        CursorController.onLeftClickUp += OnLeftClickUp;
        offset = transform.position - GameObject.FindWithTag("Cursor").transform.position;
        isDragging = true;
    }

    protected virtual void OnLeftClickUp() {
        isDragging = false;
        allowDrag = true;
        CursorController.onLeftClickUp -= OnLeftClickUp;
    }

    protected virtual void Update() {
        if (isDragging && allowDrag) {
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = GameObject.FindWithTag("Cursor").transform.position;
        }
    }
}
