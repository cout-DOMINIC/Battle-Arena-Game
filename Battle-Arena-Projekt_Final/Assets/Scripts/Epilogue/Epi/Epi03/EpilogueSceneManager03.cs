using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpilogueSceneManager03 : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public EpilogueScene03 epilogue03;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue03(epilogue03);
        AudioManager.instance.PlayBGM(8);
    }

    public void StartDialogue03 (EpilogueScene03 epilogue03)
    {
        Debug.Log("Starting conversation with " + epilogue03.name);

        animator.SetBool("IsOpen", true);
        nameText.text = epilogue03.name;
        sentences.Clear();

        foreach(string sentence in epilogue03.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSenctence(sentence));
    }

    IEnumerator TypeSenctence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
        SceneManager.LoadScene("Epilogue04");
    }
}
