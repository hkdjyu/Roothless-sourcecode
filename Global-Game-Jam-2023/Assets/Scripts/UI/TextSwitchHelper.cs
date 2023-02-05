using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSwitchHelper : MonoBehaviour {
    private List<TMP_Text> TextContents = new List<TMP_Text>();
    private int currentPointer = 0;
    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).name.Contains("Text-Content")) {
                TextContents.Add(transform.GetChild(i).GetComponent<TMP_Text>());
            }
        }

        foreach (var t in TextContents) {
            if (t.name == "Text-Content0") t.enabled = true;
            else t.enabled = false;
        }

        currentPointer = 0;

    }

    public void NextPage() {
        if (currentPointer == TextContents.Count - 1) return;
        currentPointer++;
        foreach (var t in TextContents) {
            t.enabled = false;
        }
        TextContents[currentPointer].enabled = true;
    }
    public void PreviousPage() {
        if (currentPointer == 0) return;
        currentPointer--;
        foreach (var t in TextContents) {
            t.enabled = false;
        }
        TextContents[currentPointer].enabled = true;
    }
}
