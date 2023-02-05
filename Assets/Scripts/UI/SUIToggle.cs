using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SUIToggle : MonoBehaviour {

    [SerializeField] private Sprite _spriteNotToggled;
    [SerializeField] private Sprite _spriteToggled;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private bool isEnable = true;
    public bool isToggled = false;
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private UnityEvent m_OnClickWhenCurrentIsNotToggled = new UnityEvent();
    [SerializeField]
    private UnityEvent m_OnClickWhenCurrentIsToggled = new UnityEvent();
    
    public UnityEvent OnClickWhenCurrentIsWhenCurrentIsNotToggled { get { return m_OnClickWhenCurrentIsNotToggled; } set { m_OnClickWhenCurrentIsNotToggled = value; } }
    
    public UnityEvent OnClickWhenCurrentIsWhenCurrentIsToggled { get { return m_OnClickWhenCurrentIsToggled; } set { m_OnClickWhenCurrentIsToggled = value; } }
    

    public bool enableMagnifiedWhenHover;
    public float magnifiedFactor = 1.05f; //default 1.05f
    
    private void Start() {
        _spriteRenderer = transform.gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = isEnable;
        if (isToggled) ChangeSprite(_spriteToggled);
        else ChangeSprite(_spriteNotToggled);
    }

    protected virtual void HandleClick() {
        if (isToggled) {
            // current is Toggled;
            m_OnClickWhenCurrentIsToggled?.Invoke();
            ChangeSprite(_spriteNotToggled);
            isToggled = false;
        }
        else {
            // current is not Toggled
            m_OnClickWhenCurrentIsNotToggled?.Invoke();
            ChangeSprite(_spriteToggled);
            isToggled = true;
        }
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
        if (enableMagnifiedWhenHover) {
            Vector3 beforeScale = transform.localScale;
            transform.localScale = new Vector3(beforeScale.x * magnifiedFactor, beforeScale.y * magnifiedFactor,
                beforeScale.z * magnifiedFactor);
        }
    }
    private void CursorOut() {
        if (enableMagnifiedWhenHover) {
            Vector3 beforeScale = transform.localScale;
            transform.localScale = new Vector3(beforeScale.x * 1 / magnifiedFactor, beforeScale.y * 1 / magnifiedFactor,
                beforeScale.z * 1 / magnifiedFactor);
        }
    }
    
    public void SetEnable(bool pIsEnable) {
        isEnable = pIsEnable;
        transform.gameObject.GetComponent<SpriteRenderer>().enabled = isEnable;
    }

    private void ChangeSprite(Sprite target) {
        _spriteRenderer.sprite = target;
    }
}
