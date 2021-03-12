using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public string mainMenuScene;

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlayBGM(11);
	}

    public void QuitToMain()
    {
      
        Application.Quit();
        //SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadCharacterChoose()
    {
        
        SceneManager.LoadScene("ChooseCharacter");


    }
}
