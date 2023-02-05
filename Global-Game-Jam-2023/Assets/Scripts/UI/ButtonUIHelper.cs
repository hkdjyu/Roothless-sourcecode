using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public float magnifiedFactor = 1.15f;

    private void CursorIn() {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 beforeScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(beforeScale.x * magnifiedFactor , beforeScale.y * magnifiedFactor, beforeScale.z * magnifiedFactor);
    }
    private void CursorOut() {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 beforeScale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(beforeScale.x *  1 / magnifiedFactor , beforeScale.y * 1 / magnifiedFactor, beforeScale.z * 1 / magnifiedFactor);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CursorIn();
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorOut();
    }
}
