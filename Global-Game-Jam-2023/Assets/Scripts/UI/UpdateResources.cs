using System;
using TMPro;
using UnityEngine;

public class UpdateResources : MonoBehaviour {
    private TMP_Text darkDustText;
    private TMP_Text meteoriteText;
    private TMP_Text manaWaterText;
    private TMP_Text slimeStoneText;
    private TMP_Text nutriJewelText;
    private Tree tree;
    private void Start() {
        darkDustText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        meteoriteText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        manaWaterText = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        slimeStoneText = transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        nutriJewelText = transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>();
        tree = FindObjectOfType<Tree>();
    }

    private void Update() {
        nutriJewelText.text =  tree.resAmount[GameResourcesType.Nutri_Jewel].ToString("0");
        slimeStoneText.text = tree.resAmount[GameResourcesType.Slime_Stone].ToString("0");
        manaWaterText.text = tree.resAmount[GameResourcesType.Mana_Water].ToString("0");
        meteoriteText.text = tree.resAmount[GameResourcesType.Meteorite].ToString("0");
        darkDustText.text = tree.resAmount[GameResourcesType.Dark_Dust].ToString("0");
    }
    
}
