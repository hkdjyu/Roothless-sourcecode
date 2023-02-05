using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SUIButton : MonoBehaviour {
    [SerializeField] private bool isEnable = true;

    [SerializeField] private AudioClip SFX;
    
    [SerializeField] public float magnifiedFactor = 1.15f;
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private UnityEvent m_OnClick = new UnityEvent();
    public UnityEvent onClick { get { return m_OnClick; } set { m_OnClick = value; } }

    private void Start() {
        transform.gameObject.GetComponent<SpriteRenderer>().enabled = isEnable;
    }

    protected virtual void HandleClick() {
        if (SFX == null) {
            AudioManager.Instance.PlaySfx("HitResources1");
        }
        else {
            AudioManager.Instance.PlaySfx(SFX.name);
        }
        m_OnClick?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (!isEnable) return;
        if (col.gameObject.CompareTag("Cursor")) {
            CursorController.onLeftClickDown += OnLeftClickDown;
            CursorIn();
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (!isEnable) return;
        if (col.gameObject.CompareTag("Cursor")) {
            CursorController.onLeftClickDown -= OnLeftClickDown;
            CursorOut();
        }
    }
    private void OnLeftClickDown() {
             HandleClick();
    }

    private void CursorIn() {
        Vector3 beforeScale = transform.localScale;
        transform.localScale = new Vector3(beforeScale.x * magnifiedFactor , beforeScale.y * magnifiedFactor, beforeScale.z * magnifiedFactor);
    }
    private void CursorOut() {
        Vector3 beforeScale = transform.localScale;
        transform.localScale = new Vector3(beforeScale.x *  1 / magnifiedFactor , beforeScale.y * 1 / magnifiedFactor, beforeScale.z * 1 / magnifiedFactor);
    }
    
    public void SetEnable(bool pIsEnable) {
        isEnable = pIsEnable;
        transform.gameObject.GetComponent<SpriteRenderer>().enabled = isEnable;
    }
    
}
