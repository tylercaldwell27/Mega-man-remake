using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseScript : MonoBehaviour {

    CanvasGroup cg;
    public Button btnPause;
    public Button btnMute;
    // Use this for initialization
    bool mute = false;

    player sound;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if (!cg)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0.0f;

        if (btnPause)
            btnPause.onClick.AddListener(PauseGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (btnPause == true && mute == false) {

        }
    
        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();
       

    }

    public void PauseGame()
    {
        if (cg.alpha == 0.0f)
        {
            cg.alpha = 1.0f;

            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;

            Time.timeScale = 1.0f;
        }
    }
   
}
