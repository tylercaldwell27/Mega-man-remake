using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    // slider for the sound
    public Slider Volume;

    
    public Button btnStart;
    public Button quit;
    bool start = false;

    //float for storing the current volume when muted
    public float backupVolume;
    //audio clips for the menu screens
    public AudioSource gamerOver;
    public AudioSource Menu;
    public player p;
    // Use this for initialization
    void Start()
    {
      
            gamerOver.Play();
            Menu.Play();
       
    }

    // Update is called once per frame
    void Update()
    {
        quit.onClick.AddListener(QuitGame);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            btnStart.onClick.AddListener(StartGame);
           
            if (start == true)
            {
                Debug.Log("hello");
                
                SceneManager.LoadScene("SampleScene");
                SceneManager.LoadScene(1);
                Invoke("MainMenu", 1f);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            btnStart.onClick.AddListener(StartGame);

            if (start == true)
            {
                Debug.Log("hello");


                SceneManager.LoadScene(0);
                Invoke("MainMenu", 1f);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            btnStart.onClick.AddListener(StartGame);

            if (start == true)
            {
                Debug.Log("hello");


                SceneManager.LoadScene(0);
                Invoke("MainMenu", 1f);
            }
        }
        volume();

    }
    //method for the value settings
    public void volume()
    {
        Menu.volume = Volume.value;

    }
    //method for muting the game
    public void muteGame()
    {
        backupVolume = Volume.value;
        Volume.value = Volume.minValue;
        Menu.volume = Volume.value;  
       

    }
    //method for unmuting the game
    public void unMuteGame()
    {
        Volume.value = backupVolume;
        Menu.volume = Volume.value;


    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void StartGame()
    {
        start = true;
     
       
       
    }
    public void playagain()
    {
        SceneManager.LoadScene(0);



    }
}
