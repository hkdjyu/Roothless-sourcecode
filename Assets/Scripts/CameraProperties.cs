using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Max after 1120 seconds (or 18m 40s)
        Camera.main.orthographicSize = Mathf.Clamp(8.0f + GameManager.Instance.gameTime * 0.025f, 8.0f, 36.0f);
    }
}
