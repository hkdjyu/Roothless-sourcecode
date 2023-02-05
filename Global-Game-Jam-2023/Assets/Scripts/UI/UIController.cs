using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private TMP_Text versionText;
    private TMP_Text gameTimeText;
    private TMP_Text nutriJewelText;
    private TMP_Text slimeStoneText;
    private TMP_Text manaWaterText;
    private TMP_Text meteoriteText;
    private TMP_Text darkDustText;
    
    private List<RectTransform> allRectTransform = new List<RectTransform>();
    private List<GameObject> Pages = new List<GameObject>();
    
    private void Start() {
        /*
        SetVersionText();
        gameTimeText = BindText("Text-GameTime");
        
        nutriJewelText = BindText("Text-NutriJewel");
        slimeStoneText = BindText("Text-SlimeStone");
        manaWaterText = BindText("Text-ManaWater");
        meteoriteText = BindText("Text-MeteoriteOfBlessings");
        darkDustText = BindText("Text-DarkDust");
        */

        FindAllRectTransform();
        FindPages();


        // Debug.Log(Pages.Count);
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    private void Update() {
        /*
        gameTimeText.text = String.Concat("gameTime = ", GameManager.Instance.gameTime.ToString("0.00"));
        nutriJewelText.text = String.Concat("NutriJewel = ", GameObject.FindWithTag("Tree").GetComponent<Tree>().resAmount[GameResourcesType.Nutri_Jewel].ToString("0"));
        slimeStoneText.text = String.Concat("SlimeStone = ", GameObject.FindWithTag("Tree").GetComponent<Tree>().resAmount[GameResourcesType.Slime_Stone].ToString("0"));
        manaWaterText.text = String.Concat("ManaWater = ", GameObject.FindWithTag("Tree").GetComponent<Tree>().resAmount[GameResourcesType.Mana_Water].ToString("0"));
        meteoriteText.text = String.Concat("MeteoriteOfB... = ", GameObject.FindWithTag("Tree").GetComponent<Tree>().resAmount[GameResourcesType.Meteorite].ToString("0"));
        darkDustText.text = String.Concat("DarkDust = ", GameObject.FindWithTag("Tree").GetComponent<Tree>().resAmount[GameResourcesType.Dark_Dust].ToString("0"));
        */
    }
    
    private void OnGameStart() {
        EventBus.Unsubscribe(GameEvent.START, OnGameStart);

        PageSwitch("InGamePage");
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
        EventBus.Subscribe(GameEvent.STOP, OnGameStop);
    }
    private void OnGamePause() {
        EventBus.Unsubscribe(GameEvent.PAUSE, OnGamePause);
        
        PageSwitch("PausePage");

        EventBus.Subscribe(GameEvent.RESUME, OnGameResume);
    }
    private void OnGameResume() {
        EventBus.Unsubscribe(GameEvent.RESUME, OnGameResume);
        
        PageSwitch("InGamePage");
        
        EventBus.Subscribe(GameEvent.PAUSE, OnGamePause);
    }
    private void OnGameStop() {
        EventBus.Unsubscribe(GameEvent.STOP, OnGameStop);
        
        PageSwitch("MainPage");
        
        EventBus.Subscribe(GameEvent.START, OnGameStart);
    }

    public void Restart() {
        GameManager.Instance.Restart();
    }

    /*
    private void SetVersionText() {
        versionText = BindText("Text-Version");
        versionText.text = String.Concat("Version ", GameManager.Instance.Version);
    }

    private TMP_Text BindText(string searchText) {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text obj in texts) {
            if (obj.gameObject.name.Equals(searchText)) {
                return obj;
            }
        }
        throw new ArgumentException("Cannot Find TMPtext with name = " + searchText);
    }
    */

    public void FireGameEvent(string pEventString) {
        GameEvent pEvent = EventBus.StringToGameEvent(pEventString);
        EventBus.Publish(pEvent);
    }

    private void FindAllRectTransform() {
        RectTransform[] objs = Resources.FindObjectsOfTypeAll<RectTransform>() as RectTransform[];
        foreach (RectTransform obj in objs) {
            allRectTransform.Add(obj);
        }
    }
    
    private void FindPages() {
        /*
        RectTransform[] objs = Resources.FindObjectsOfTypeAll<RectTransform>() as RectTransform[];
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].CompareTag("Page")) {
                Pages.Add(objs[i].gameObject);
            }
        }
        */
        foreach (RectTransform t in allRectTransform) {
            if (t.CompareTag("Page")) {
                Pages.Add(t.gameObject);
            }
        }
    }
    
    public void PageSwitch(string pageName) {
        foreach (GameObject page in Pages) {
            page.SetActive(false);
            if (page.name.Equals(pageName)) {
                page.SetActive(true);
            }
        }
    }
    public void PageOpen(string pageName) {
        foreach (GameObject page in Pages) {
            if (page.name.Equals(pageName)) {
                page.SetActive(true);
            }
        }
    }

    public void SendDebugMessage(string message) {
        Debug.Log(message);
    }

    public void SetUIObjectActive(string pName) {
        foreach (RectTransform t in allRectTransform) {
            if (t.name.Equals(pName)) {
                t.gameObject.SetActive(true);
            }
        }
    }

    public void SetUIObjectNotActive(string pName) {
        foreach (RectTransform t in allRectTransform) {
            if (t.name.Equals(pName)) {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateMouseSpeed() {
        foreach (RectTransform t in allRectTransform) {
            if (t.name.Equals("Mouse-speed")) {
                GameManager.Instance.GameSetting.MouseSensitivity = t.GetChild(0).GetComponent<Slider>().value;
            }
        }
    }
    public void UpdateMusicVolume() {
         foreach (RectTransform t in allRectTransform) {
             if (t.name.Equals("Panel-Music")) {
                 GameManager.Instance.GameSetting.MusicVolume = t.GetChild(0).GetComponent<Slider>().value;
             }
         }
    }
    public void UpdateSFXVolume() {
        foreach (RectTransform t in allRectTransform) {
            if (t.name.Equals("Panel-SFX")) {
                GameManager.Instance.GameSetting.SfxVolume = t.GetChild(0).GetComponent<Slider>().value;
            }
        }
    }

    public void UpdateShopSystem(string actionName) {
        ShopSystem shop = FindObjectOfType<ShopSystem>();
        switch (actionName.ToLower()) {
            case "buy" :
                shop.NewRoot();
                break;
            case "left":
                shop.MovePointingRoot(false);
                break;
            case "right":
                shop.MovePointingRoot(true);
                break;
            case "tenacity":
                shop.TenacityLevelUp();
                break;
            case "attack":
                shop.AttackLevelUp();
                break;
            case "speed":
                shop.SpeedLevelUp();
                break;
            case "absorb":
                shop.AbsorbLevelUp();
                break;
            case "fairy":
                shop.NewFairy();
                break;
            case "spirit":
                shop.NewLightSpirit();
                break;
            case "spike":
                shop.SpikeLevelUp();
                break;
            case "bomb":
                shop.BombBulbLevelUp();
                break;
        }
    }

    public void PlaySFX(string clipName) {
        AudioManager.Instance.PlaySfx(clipName);
    }
    
}
