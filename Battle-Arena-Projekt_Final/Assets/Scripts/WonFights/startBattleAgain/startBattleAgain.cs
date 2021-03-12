using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBattleAgain : MonoBehaviour
{

    //public List<blabla> allStoryScenes;
    // public string[] scenes;

    int rounds = 3;

    public void click()
    {
        Debug.Log("BUTTON startBattleAgain clicked");
        Debug.Log("BUTTON startBattleAgain playerCharacter.getName() : " + playerCharacter.getName());
        // SceneManager.LoadScene("WonFirstFight");

        for (int i = 0; i <= rounds; i++)
        {
            if (GameManager.instance.playerStats[0].round == 1)
            {
                SceneManager.LoadScene("WonFirstFight");
            }

            if (GameManager.instance.playerStats[0].round == 2)
            {
                SceneManager.LoadScene("WonSecondFight");
            }

            if (GameManager.instance.playerStats[0].round == 3)
            {
                SceneManager.LoadScene("WonThirdFight");
            }
        }
    }
}
