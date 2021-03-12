using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManageWON : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public DialogueWON dialogueWON;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        //AudioManager.instance.PlayBGM(7);
        StartDialogue(dialogueWON);
    }

    public void StartDialogue (DialogueWON dialogueWON)
    {
        Debug.Log("Starting conversation with " + dialogueWON.name);

        animator.SetBool("IsOpen", true);
        nameText.text = dialogueWON.name;
        sentences.Clear();

        foreach(string sentence in dialogueWON.sentences)
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
            yield return new WaitForSeconds(0.03f);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
        SceneManager.LoadScene("StoryScene24");
    }
}
