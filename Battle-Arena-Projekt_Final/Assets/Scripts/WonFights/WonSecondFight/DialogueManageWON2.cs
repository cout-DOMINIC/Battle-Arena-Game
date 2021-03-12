using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManageWON2 : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public DialogueWON2 dialogueWON2;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue(dialogueWON2);
    }

    public void StartDialogue (DialogueWON2 dialogueWON2)
    {
        Debug.Log("Starting conversation with " + dialogueWON2.name);

        animator.SetBool("IsOpen", true);
        nameText.text = dialogueWON2.name;
        sentences.Clear();

        foreach(string sentence in dialogueWON2.sentences)
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
            yield return new WaitForSeconds(0.04f);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
        SceneManager.LoadScene("StoryScene31_5");
    }
}
