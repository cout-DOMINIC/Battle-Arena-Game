using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpilogueSceneManager15 : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public EpilogueScene15 epilogue15;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue(epilogue15);
    }

    public void StartDialogue (EpilogueScene15 epilogue15)
    {
        Debug.Log("Starting conversation with " + epilogue15.name);

        animator.SetBool("IsOpen", true);
        nameText.text = epilogue15.name;
        sentences.Clear();

        foreach(string sentence in epilogue15.sentences)
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
        SceneManager.LoadScene("Epilogue2");
    }
}
