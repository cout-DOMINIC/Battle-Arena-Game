using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour {

    public bool isPlayer;
    public List<string> movesAvailable;

    public string charName;
    public int currentHp, maxHP, currentMP, maxMP, strength, defence, wpnPower, armrPower, round;
    public bool hasDied;

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

    private bool shouldFade;
    public float fadeSpeed = 1f;

    //ANIMATION
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip animClip;

    // Use this for initialization
    void Start () {

        if (anim == null)
            return; // exit method for example
        if(isPlayer == true)
        {
        anim = GetComponent<Animator>();
            //myAnimationComponent = GetComponent<Animation>();
            animClip = GetComponent<AnimationClip>();
           // anim.Play("Idle", 0, 0);
            // myAnimationComponent.Play("Idle");
            /*
            foreach (AnimationState state in anim)
            {
                state.speed = 0.5F;
            }
            */
       }
    }
	
	// Update is called once per frame
	void Update () {
        if(isPlayer == true)
        {
            //  anim.Play("Shield", 0, 0.25f);

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<Animator>().Play("Sword", -1, 0f);
                Debug.Log("Sword");
            
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Shield");
                anim.Play("Shield");
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Victory");
                anim.Play("Victory");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Defeat");
                anim.Play("Defeat");
            }
        }
        if (shouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
