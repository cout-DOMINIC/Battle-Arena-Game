using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChooseCharacter : MonoBehaviour
{ 
    // public playerCharacter playercharacter;
    public void PressChooseCharacter(Button button)
    {
        Debug.Log("BUTTON " + button.transform.GetComponentInChildren<Text>().text);
        playerCharacter.setName(button.transform.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene("StoryScene");
    }
}
