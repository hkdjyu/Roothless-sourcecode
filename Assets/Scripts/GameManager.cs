using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    private DateTime _sessionStartTime;
    private DateTime _sessionEndTime;

    public float gameTime { get; private set;} // expected end time = 1500
    private bool allowRecordGameTime = false;
    private bool isGameStart = false;
    public string Version { get; private set; } = "0.1.3";
    [SerializeField] private float timeScale = 1.0f;
    
    public GameSettings GameSetting;
    
    void Start() {
        Time.timeScale = 0.0f;
        _sessionStartTime = DateTime.Now;
        Debug.Log("Game session start @: " + DateTime.Now);
        EventBus.Subscribe(GameEvent.START, OnGameStart);
        //InvokeRepeating("UpdateWalkablePath", 0f, 1.0f);
        GameSetting = Resources.Load<GameSettings>("GameSetting");
        
        FindObjectOfType<GameResourcesSystem>().enabled = false;
        FindObjectOfType<EnemySystem>().enabled = false;
        FindObjectOfType<SkillSystem>().enabled = false;
        FindObjectOfType<ShopSystem>().enabled = false;
        FindObjectOfType<Rigidbody2D>().simulated = false;
        FindObjectOfType<Tree>().enabled = false;
        foreach (var rt in FindObjectsOfType<Root>()) {
            rt.enabled = false;
        }
    }


    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);
        isGameStart = true;
        gameTime = 0.0f;
        allowRecordGameTime = true;
        Time.timeScale = 1.0f;
        
        FindObjectOfType<GameResourcesSystem>().enabled = true;
        FindObjectOfType<EnemySystem>().enabled = true;
        FindObjectOfType<SkillSystem>().enabled = true;
        FindObjectOfType<ShopSystem>().enabled = true;
        FindObjectOfType<Rigidbody2D>().simulated = true;
        FindObjectOfType<Tree>().enabled = true;
        foreach (var rt in FindObjectsOfType<Root>()) {
            rt.enabled = true;
        }
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
        EventBus.Subscribe(GameEvent.STOP, OnGameStop);
    }
    private void OnGamePause() {
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        
        allowRecordGameTime = false;
        Time.timeScale = 0.0f;
        foreach(Animator animator in FindObjectsOfType<Animator>()) {
            animator.enabled = false;
        }
        foreach(Collider collider in FindObjectsOfType<Collider>()) {
            collider.enabled = false;
        }
        foreach(Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>()) {
            rb.simulated = false;
        }
        FindObjectOfType<GameResourcesSystem>().enabled = false;
        FindObjectOfType<EnemySystem>().enabled = false;
        FindObjectOfType<SkillSystem>().enabled = false;
        FindObjectOfType<ShopSystem>().enabled = false;
        FindObjectOfType<Rigidbody2D>().simulated = false;
        FindObjectOfType<Tree>().enabled = false;
        foreach (var rt in FindObjectsOfType<Root>()) {
            rt.enabled = false;
        }

        EventBus.Subscribe(GameEvent.RESUME, OnGameResume);
    }
    private void OnGameResume() {
        EventBus.Unsubscribe(GameEvent.RESUME, OnGameResume);
        
        allowRecordGameTime = true;
        Time.timeScale = 1.0f;
        foreach(Animator animator in FindObjectsOfType<Animator>()) {
            animator.enabled = true;
        }
        foreach(Collider collider in FindObjectsOfType<Collider>()) {
            collider.enabled = true;
        }
        foreach(Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>()) {
            rb.simulated = true;
        }
        FindObjectOfType<GameResourcesSystem>().enabled = true;
        FindObjectOfType<EnemySystem>().enabled = true;
        FindObjectOfType<SkillSystem>().enabled = true;
        FindObjectOfType<ShopSystem>().enabled = true;
        FindObjectOfType<Rigidbody2D>().simulated = true;
        FindObjectOfType<Tree>().enabled = true;
        foreach (var rt in FindObjectsOfType<Root>()) {
            rt.enabled = true;
        }
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }
    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        
        isGameStart = false;
        allowRecordGameTime = false;
        FindObjectOfType<GameResourcesSystem>().enabled = false;
        FindObjectOfType<EnemySystem>().enabled = false;
        FindObjectOfType<SkillSystem>().enabled = false;
        FindObjectOfType<ShopSystem>().enabled = false;
        FindObjectOfType<Rigidbody2D>().simulated = false;
        FindObjectOfType<Tree>().enabled = false;
        foreach (var rt in FindObjectsOfType<Root>()) {
            rt.enabled = false;
        }
        foreach (var en in FindObjectsOfType<EnemyBehaviour>()) {
            en.enabled = false;
        }

        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void Update() {
        if (allowRecordGameTime)
            gameTime += Time.deltaTime;
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.Delete)) {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameStart) {
                if (allowRecordGameTime) {
                    EventBus.Publish(GameEvent.PAUSE);
                    FindObjectOfType<CursorController>().ResetCursorPosition();
                }
                    
            }
        }
    }

    private void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void Restart() {
        EventBus.Publish(GameEvent.STOP);
        gameTime = 0f;
        string temp = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(temp);
        SceneManager.sceneLoaded += (Scene sceneName, LoadSceneMode loadSceneMode) => {
            FindObjectOfType<GameResourcesSystem>().enabled = false;
            FindObjectOfType<EnemySystem>().enabled = false;
            FindObjectOfType<SkillSystem>().enabled = false;
            FindObjectOfType<ShopSystem>().enabled = false;
            FindObjectOfType<Rigidbody2D>().simulated = false;
            FindObjectOfType<Tree>().enabled = false;
            foreach (var rt in FindObjectsOfType<Root>()) {
                rt.enabled = false;
            }
        };
    }

    public void GameOver() {
        EventBus.Publish(GameEvent.STOP);
        UIController controller = FindObjectOfType<UIController>();
        controller.PageSwitch("GameOverPage");
    }
    

    void OnApplicationQuit() {
        _sessionEndTime = DateTime.Now;
        TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);
        Debug.Log("Game session ended @: " + DateTime.Now);
        Debug.Log("Game session lasted: " + timeDifference);
    }
}
