using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackMenuButton : MonoBehaviour
{

    public void Play()
    {
        Invoke("Invoke", 0.5f);
    }

    void Invoke()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
