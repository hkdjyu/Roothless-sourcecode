using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UpdateShopUI : MonoBehaviour {
    private ShopSystem shop;
    private List<TMP_Text> allTMPTexts = new List<TMP_Text>();
    private void Start() {
        shop = FindObjectOfType<ShopSystem>();
        shop.pointingRootUpdate += OnPointingRootUpdate;
        shop.newRootUpdate += OnNewRootUpdate;
        shop.tenacityLevelUpdate += OnTenacityLevelUpdate;
        shop.attackLevelUpdate += OnAttackLevelUpdate;
        shop.speedLevelUpdate += OnSpeedLevelUpdate;
        shop.absorbLevelUpdate += OnAbsorbLevelUpdate;
        shop.newFairyUpdate += OnNewFairyUpdate;
        shop.newLightSpiritLevelUpdate += OnNewLightSpiritLevelUpdate;
        shop.spikeLevelUpdate += OnSpikeLevelUpdate;
        shop.bombBulbLevelUpdate += OnBombBulbLevelUpdate;

        InitAllTMPTexts();
        foreach (var str in allPrefixStrings) {
            UpdateFiveResources(str);
        }
    }

    void InitAllTMPTexts() {
        // 11 -> 34 where 11 is special 13
        for (int i = 11; i < 35; i++) {
            if ( i != 13 ) allTMPTexts.Add(transform.GetChild(i).GetComponent<TMP_Text>());
            else {
                transform.GetChild(13).GetChild(0).GetComponent<TMP_Text>();
            }
        }
    }

    private string[] allPrefixStrings = new[] { "NewRoot" , "Tenacity", "Attack", "Speed", "Absorb", "Fairy", "Spirit", "Spike", "Bomb"};
    private void OnPointingRootUpdate(int obj) {
        transform.GetChild(13).GetChild(0).GetComponent<TMP_Text>().text = "Root " + (obj+1).ToString();
        foreach (var str in allPrefixStrings) {
            UpdateFiveResources(str);
        }
    }

    private void UpdateFiveResources(string prefix) {
        List<TMP_Text> temp = new List<TMP_Text>();
        switch (prefix) {
                case "NewRoot" :
                    shop.NewRoot(true);
                    break;
                case "Tenacity":
                    shop.TenacityLevelUp(true);
                    break;
                case "Attack":
                    shop.AttackLevelUp(true);
                    break;
                case "Speed":
                    shop.SpeedLevelUp(true);
                    break;
                case "Absorb":
                    shop.AbsorbLevelUp(true);
                    break;
                case "Fairy":
                    shop.NewFairy(true);
                    break;
                case "Spirit":
                    shop.NewLightSpirit(true);
                    break;
                case "Spike":
                    shop.SpikeLevelUp(true);
                    break;
                case "Bomb":
                    shop.BombBulbLevelUp(true);
                    break;
        }
        foreach (var t in allTMPTexts) {
            if(t.transform.name.Contains(prefix+"-"))
                temp.Add(t);
        }
        foreach (var t in temp) {
            string suffix = t.name.Substring(prefix.Length + 1, 2);
            switch (suffix) {
                case "NJ": // NutriJewel
                    t.text = Mathf.Floor(shop.Cost_NutriJewel).ToString();
                    break;
                case "SL": // SlimeStone
                    t.text = Mathf.Floor(shop.Cost_SlimeStone).ToString();
                    break;
                case "WT": // ManaWater
                    t.text = Mathf.Floor(shop.Cost_ManaWater).ToString();
                    break;
                case "MT": // Meteorite
                    t.text = Mathf.Floor(shop.Cost_Meteorite).ToString();
                    break;
                case "DD": //DarkDust
                    t.text = Mathf.Floor(shop.Cost_DarkDust).ToString();
                    break;
            }
        }

    }

    private void OnBombBulbLevelUpdate(int obj) {
        UpdateFiveResources("Bomb");
    }

    private void OnSpikeLevelUpdate(int obj) {
        UpdateFiveResources("Spike");
    }

    private void OnNewLightSpiritLevelUpdate(int obj) {
        UpdateFiveResources("Spirit");
    }

    private void OnNewFairyUpdate(int obj) {
        UpdateFiveResources("Fairy");
    }

    private void OnAbsorbLevelUpdate(int obj) {
        UpdateFiveResources("Absorb");
    }

    private void OnSpeedLevelUpdate(int obj) {
        UpdateFiveResources("Speed");
    }

    private void OnAttackLevelUpdate(int obj) {
        UpdateFiveResources("Attack");
    }

    private void OnTenacityLevelUpdate(int obj) {
        UpdateFiveResources("Tenacity");
    }

    private void OnNewRootUpdate() {
        UpdateFiveResources("NewRoot");
    }
}
