using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float waitToLoad = 1;
    public bool shouldLoadAfterFade;

    public string newGameScene;

    public GameObject continueButton;

    public string loadGameScene;
    public void Start()
    {
        AudioManager.instance.StopMusic();
    }
    public void Play()
    {
        Invoke("Invoke", 0.5f);
    }

    void Invoke()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void Exit()
    {
        Invoke("Invoke2", 0.5f);
    }

    void Invoke2()
    {
        Application.Quit();
    }


}

