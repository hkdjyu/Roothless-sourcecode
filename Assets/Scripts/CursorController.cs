using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour {
    
    private GameObject _cursor;
    private float h = 0;
    private float v = 0;
    private bool allowMove = false;
    [HideInInspector] 
    public static float sensitivity = 0.5f; // player custom setting
    public static float speed = 1.0f; // Game change Factor
    private readonly float speedFactor = 100.0f; // fixed speed Factor
        
    private Camera _camera;
    private float screenWidthWorldSpace;
    private float screenHeightWorldSpace;

    public bool isEnable = false;

    public static event Action onLeftClickDown;
    public static event Action onLeftClickUp;
    public static event Action onRightClickDown;
    public static event Action onRightClickUp;

    private void Start() {
        isEnable = false;
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        FindBoundaries();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
        _cursor = Instantiate(Resources.Load<GameObject>("Cursor"), Vector3.zero,Quaternion.identity);
        _cursor.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
        _cursor.transform.position = Vector3.zero;
        _cursor.transform.localPosition = Vector3.zero;
        onLeftClickDown += OnLeftClickDown;
        
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        
        _cursor.SetActive(false);
    }

    private void OnEnable() {
        GameManager.Instance.GameSetting.mouseSensitivityUpdated += OnMouseSensitityChanged;
    }

    private void OnMouseSensitityChanged(float obj) {
        sensitivity = obj;
    }

    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);

        isEnable = true;
        _cursor.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
        EventBus.Subscribe(GameEvent.STOP, OnGameStop);
    }
    private void OnGamePause() {
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        
        isEnable = false;
        _cursor.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventBus.Subscribe(GameEvent.RESUME, OnGameResume);
    }
    private void OnGameResume() {
        EventBus.Unsubscribe(GameEvent.RESUME, OnGameResume);
        
        isEnable = true;
        _cursor.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }
    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        
        isEnable = false;
        _cursor.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void OnLeftClickDown(){
        allowMove = true;
        onLeftClickDown -= OnLeftClickDown;
    }
    private void Update() {
        if (isEnable) {
            FindBoundaries();
            UpdateCursor();
            if (Input.GetMouseButtonDown(0)) onLeftClickDown?.Invoke();
            if (Input.GetMouseButtonUp(0)) onLeftClickUp?.Invoke();
            if (Input.GetMouseButtonDown(1)) onRightClickDown?.Invoke();
            if (Input.GetMouseButtonUp(1)) onRightClickUp?.Invoke();
        }
    }

    private void FindBoundaries() {
        //Debug.Log(" w=" + Screen.width + " h=" + Screen.height);
        screenWidthWorldSpace = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10.0f)).x * 2 * 0.99f;
        screenHeightWorldSpace = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,10.0f)).y * 2 * 0.99f;
    }
    

    private void UpdateCursor() {
        FindBoundaries();
        if (!allowMove) return;
        h = Input.GetAxis("Mouse X");
        v = Input.GetAxis("Mouse Y");
        float x = h * Time.unscaledDeltaTime * sensitivity * speedFactor * speed;
        float y = v * Time.unscaledDeltaTime * sensitivity * speedFactor * speed;
        if (_cursor.transform.position.x > screenWidthWorldSpace / 2 && x > 0) x = 0;
        if (_cursor.transform.position.x < -1 * screenWidthWorldSpace / 2 && x <0) x = 0;
        if (_cursor.transform.position.y > screenHeightWorldSpace / 2 && y > 0) y = 0;
        if (_cursor.transform.position.y < -1 * screenHeightWorldSpace / 2 && y < 0) y = 0;
        _cursor.transform.Translate(x,y,0);
    }

    public void ResetCursorPosition() {
        _cursor.transform.position = Vector3.zero;
    }

    public void SetCursorAllowMove(bool canMove) {
        allowMove = canMove;
    }
}
