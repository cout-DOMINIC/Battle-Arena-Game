using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    public static BattleManager Instance { get { return instance; } }
    private bool battleActive;
    // control whether scene is on or off
    public GameObject battleScene;
    // backgrounds
    public Sprite[] BGs;
    // positions
    public Transform[] playerPositions;
    public Transform[] enemyPositions;
    // battleChar-Skripts
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    // list of active Battlers - for multiple battlers
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn; // current nr of turn
    public bool turnWaiting; // used while waiting for turn to end

    public GameObject uiButtonsHolder; // reference to MenuButtons (attack, magic...)

    public BattleMove[] movesList;// script BattleMove - items added manually in BattleManager
    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public GameObject targetMenu; // momentan aus -> nur 1 Gegner
    public BattleTargetButton[] targetButtons; // "

    public GameObject magicMenu; // Anzeige Magic-Attacken
    public BattleMagicSelect[] magicButtons;

    public GameObject stats; // Anzeige unten rechts
    public Text[] playerName, playerHP, playerMP; // für stats

    public BattleNotification battleNotice;
    public string gameOverScene;

    public int rewardXP;
    public string[] rewardItems;

    public string PlayerRoleName; // name given trough ChooseCharacter-Scene
    public int BattleRound; // Rounds inside one battle
    public SpriteRenderer m_SpriteRenderer;

    public GameObject HPValue;
    public GameObject MPValue;
    private bool init = true;
    private bool aMasterDead = false;
    // Use this for initialization

    void Start()
    {
        UIFade.instance.FadeFromBlack();
        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerRoleName = playerCharacter.getName();
        BattleStart(new string[] { "" });
    }
    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKeyDown(KeyCode.T)){}
        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer && activeBattlers[1].currentHp > 0) //set menu active only if it's player's turn (isPlayer -> property of each character)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);
                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
        }
    }
    public void setPlayerRoleName(string playerRN)
    {
        Debug.Log("SET PLAYER ROLE NAME : " + playerRN);
        PlayerRoleName = playerRN;
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        // UNTENAUSKOMMENTIEREN
        // PlayerRoleName = "Krieger";
        aMasterDead = false;
        activeBattlers = new List<BattleChar>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        stats.SetActive(true);
        CharStats thePlayer;
        m_SpriteRenderer.sprite = BGs[11];

        if (playerPositions[0].childCount > 1)
            for (var i = 0; i < playerPositions[0].childCount; i++)
            {
                playerPositions[0].GetChild(i).gameObject.SetActive(false);
            }

        if (enemyPositions[0].childCount > 1)
            for (var i = 0; i < enemyPositions[0].childCount; i++)
            {
                enemyPositions[0].GetChild(i).gameObject.SetActive(false);
            }

        if (!battleActive)
        {
            battleActive = true;
            GameManager.instance.battleActive = true; //inform GameManager
            battleScene.SetActive(true);// activate BattleScene
            AudioManager.instance.PlayBGM(7);// switch to battle music
            for (int j = 0; j < playerPrefabs.Length; j++)
            {

                if (playerPrefabs[j].charName == PlayerRoleName && PlayerRoleName == GameManager.instance.playerStats[j].charName)
                {   // create Character at position "j" , at playerpositon "i", correct rotation  

                    BattleChar newPlayer = Instantiate(playerPrefabs[j], enemyPositions[0].position, enemyPositions[0].rotation); // changed enemy and playerpositions
                    newPlayer.transform.parent = enemyPositions[0]; // in case we want to move during battle
                    activeBattlers.Add(newPlayer); // add to activeBattlers

                    // set Player stats
                    thePlayer = GameManager.instance.playerStats[j];
                    activeBattlers[0].charName = thePlayer.charName;
                    activeBattlers[0].currentHp = thePlayer.currentHP;
                    activeBattlers[0].maxHP = thePlayer.maxHP;
                    activeBattlers[0].currentMP = thePlayer.currentMP;
                    activeBattlers[0].maxMP = thePlayer.maxMP;
                    activeBattlers[0].strength = thePlayer.strength;
                    activeBattlers[0].defence = thePlayer.defence;
                    activeBattlers[0].wpnPower = thePlayer.wpnPwr;
                    activeBattlers[0].armrPower = thePlayer.armrPwr;
                    activeBattlers[0].round = thePlayer.round;

                    if (activeBattlers[0].round == 0)
                    {
                        enemiesToSpawn[0] = "Ghoul";

                        //////////////// SET START ATTACKS OF PLAYERS AT ROUND 0 //////////////////

                        if (activeBattlers[0].charName == "Krieger")
                        {
                            playerPrefabs[0].movesAvailable.Clear();
                            playerPrefabs[0].movesAvailable.Add("Wilder Keulenschlag");
                            playerPrefabs[0].movesAvailable.Add("Zerschmettern");
                            playerPrefabs[0].movesAvailable.Add("Heilung");

                            activeBattlers[0].movesAvailable = playerPrefabs[0].movesAvailable;
                        }
                        else if (activeBattlers[0].charName == "Hexenmeister")
                        {
                            playerPrefabs[1].movesAvailable.Clear();
                            playerPrefabs[1].movesAvailable.Add("Waldgeister");
                            playerPrefabs[1].movesAvailable.Add("Todesblitz");
                            activeBattlers[0].movesAvailable = playerPrefabs[1].movesAvailable;
                        }
                        else if (activeBattlers[0].charName == "Magier")
                        {
                            playerPrefabs[2].movesAvailable.Clear();
                            playerPrefabs[2].movesAvailable.Add("Feuerball");
                            playerPrefabs[2].movesAvailable.Add("Windsphäre");
                            activeBattlers[0].movesAvailable = playerPrefabs[2].movesAvailable;
                        }
                        ///////////////////////////////////////////////////////////////////////
                    }
                    else if (thePlayer.round == 1)
                    {
                        enemiesToSpawn[0] = "A.Leiter";
                    }
                    else if (thePlayer.round == 2)
                    {
                        enemiesToSpawn[0] = "Tod";
                        AudioManager.instance.PlayBGM(9);
                    }
                }
            }

            if (enemiesToSpawn[0] != "")
            { // rest similiar to player config above
                for (int j = 0; j < enemyPrefabs.Length; j++)
                {
                    if (enemyPrefabs[j].charName == enemiesToSpawn[0])
                    {
                        BattleChar Enemy = Instantiate(enemyPrefabs[j], playerPositions[0].position, playerPositions[0].rotation);
                        Enemy.transform.parent = playerPositions[0];
                        activeBattlers.Add(Enemy);

                        foreach (var battler in activeBattlers)
                        {
                            Debug.Log("activeBattlers : " + JsonUtility.ToJson(battler));
                        }
                    }
                }
            }
            Debug.Log("enemyPositions[0].childCount : " + enemyPositions[0].childCount);
            // SCALE THE PLAYER & POSITION
            enemyPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            enemyPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.position = new Vector3(-3f, 1.7f, 1f);

            playerPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            playerPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.position = new Vector3(3f, 1.7f, 1f);
            turnWaiting = true;

            if (activeBattlers[1].charName == "Tod")
            {
                playerPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.position = new Vector3(4f, 1.9f, 1f);
                enemyPositions[0].GetChild(enemyPositions[0].childCount - 1).transform.position = new Vector3(-3.5f, 1.7f, 1f);
            }
            //currentTurn = Random.Range(0, activeBattlers.Count); // beginner randomly selected

            UpdateUIStats();
        }
    }

    public void NextTurn()
    {

        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
            BattleRound++; Debug.Log("BATTLE ROUND : " + BattleRound);
        }

        turnWaiting = true;
        if (aMasterDead == false)
            UpdateBattle(); // check Battle data (deaths)
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        if (activeBattlers != null)
        {
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].currentHp <= 0)
                {

                    //Handle dead battler - set dead sprite
                    if (activeBattlers[i].isPlayer)
                    {
                        //activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                        //activeBattlers[i].EnemyFade();
                        activeBattlers[i].round = 0;
                        stats.SetActive(false);
                    }
                    else
                    {
                        //activeBattlers[i].EnemyFade();
                    }

                }
                else
                {
                    if (activeBattlers[i].isPlayer)
                    {
                        allPlayersDead = false;
                        activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                    }
                    else
                    {
                        allEnemiesDead = false;
                    }
                }
            }
        }
        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                if (activeBattlers[0] != null)
                {
                    ///////////////////////// ADD ATTACKS AT END OF EACH BATTLE ///////////////////////////////////////
                    // KRIEGER
                    if (activeBattlers[0].charName == "Krieger") // KRIEGER: Round: 1 add Plasma
                    {
                        Debug.Log("Krieger TRUE");
                        if (activeBattlers[0].round == 0)
                            playerPrefabs[0].movesAvailable.Add("Sturmgeschmiedete Faust");
                        if (activeBattlers[0].round == 1)
                            playerPrefabs[0].movesAvailable.Add("Verheerende Axt");
                    }
                    //HEXENMEISTER
                    else if (activeBattlers[0].charName == "Hexenmeister") // MAGIER: Round: 1 add Plasma
                    {
                        Debug.Log("HEXENMEISTER TRUE");
                        if (activeBattlers[0].round == 0)
                            playerPrefabs[1].movesAvailable.Add("Dämonisches Aufzehren");
                        if (activeBattlers[0].round == 2)
                            playerPrefabs[1].movesAvailable.Add("Feuer & Schwefel");
                    }
                    // MAGIER
                    else if (activeBattlers[0].charName == "Magier") // MAGIER: Round: 1 add Plasma
                    {
                        Debug.Log("MAGIER TRUE");
                        if (activeBattlers[0].round == 0)
                            playerPrefabs[2].movesAvailable.Add("Komet");
                        if (activeBattlers[0].round == 2)
                            playerPrefabs[2].movesAvailable.Add("Schneesturm");
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////


                    foreach (var player in playerPrefabs)
                    {
                        if (activeBattlers[0].charName == player.charName)
                            player.round++;
                    }

                    foreach (var battler in activeBattlers)
                    {
                        battler.round++;
                    }
                    foreach (var player in GameManager.instance.playerStats)
                    {
                        player.round++;
                    }
                }
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                AudioManager.instance.PlaySFX(14);
                if (activeBattlers[1].charName == "Ghoul") AudioManager.instance.PlaySFX(15);
                activeBattlers[0].GetComponent<Animator>().Play("Defeat", -1, 0f);
                activeBattlers[1].GetComponent<Animator>().Play("Victory", -1, 0f);
                foreach (var player in playerPrefabs)
                {
                    player.round = 0;
                } // died: set round to 0
                foreach (var battler in activeBattlers)
                {
                    battler.round = 0;
                }
                foreach (var player in GameManager.instance.playerStats)
                {
                    player.round = 0;
                }
                StartCoroutine(GameOverCo());
            }
            // TODO CLEAR TURNS IN ATTACKS
            for (int i = 0; i < movesList.Length; i++)
            {
                if (movesList[i].turnsToWait > 0)
                    movesList[i].turns = movesList[i].turnsToWait;
                if (movesList[i].turnsToHurt > 0)
                    movesList[i].turns = movesList[i].turnsToHurt;
            }
            /* battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false; */
        }
        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)// ?????????????
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo() // set short timeout while enemy attacks
    {

        turnWaiting = false;
        yield return new WaitForSeconds(2.5f);

        //////////////////////////* LONG ATTACKS ON PLAYER *///////////////////////
        foreach (var move in movesList)
        {

            if (move.turns < move.turnsToHurt && 0 < move.turnsToHurt) //ATTACK OVER  ROUNDS
            {

                foreach (var mAvailable in activeBattlers[1].movesAvailable)
                {
                    if (move.moveName == mAvailable)
                    {
                        DealDamage(0, move.movePower / 2, move.moveName, move.health, move.turns);
                        Instantiate(move.theEffect, activeBattlers[0].transform.position, activeBattlers[0].transform.rotation);
                        battleNotice.theText.text = move.moveName + " schadet " + activeBattlers[0].charName + " weiterhin!";

                        battleNotice.Activate();
                        yield return new WaitForSeconds(2f);
                        move.turns++;
                    }
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////

        yield return new WaitForSeconds(2f); // 4
        EnemyAttack();
        yield return new WaitForSeconds(2f);

        //////////////////////////* LONG ATTACKS ON ENEMY *///////////////////////
        foreach (var move in movesList)
        {
            if (move.turns < move.turnsToHurt && 0 < move.turnsToHurt) //ATTACK OVER  ROUNDS
            {
                foreach (var mAvailable in activeBattlers[0].movesAvailable)
                {
                    if (move.moveName == mAvailable)
                    {
                        DealDamage(1, move.movePower, move.moveName, move.health, move.turns);
                        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                        battleNotice.theText.text = move.moveName + " schadet " + activeBattlers[1].charName + " weiterhin!";
                        battleNotice.Activate();
                        yield return new WaitForSeconds(2f);
                        move.turns++;
                    }
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////
        yield return new WaitForSeconds(1f);

        NextTurn();
    }
    public void EnemyAttack()
    {
        if (activeBattlers[1].currentHp > 0)
        {
            List<int> players = new List<int>(); // list
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
                {
                    players.Add(i); // add activeBattlers that are players && are alive to new list
                }
            }
            int selectedTarget = players[Random.Range(0, players.Count)]; // set target randomly amongst them

            //activeBattlers[selectedTarget].currentHp -= 30;
            // set random attack of those that are available to the current enemy
            int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Count);
            int movePower = 0;
            string moveN = "";
            int health = 0;
            int turns = 0;
            int turnsToWait = 0;
            int turnsToHurt = 0;
            for (int i = 0; i < movesList.Length; i++)
            {
                //movesList[i].theEffect.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                Debug.Log("MOOVE" + movesList[i].theEffect);
                // move.theEffect.transform.localScale = new Vector3(0.5f, 3.5f, 1f);
                if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
                {
                    health = movesList[i].health;
                    movePower = movesList[i].movePower;
                    if (movesList[i].health > 0)
                    {//movesList[2] == ICE
                        Instantiate(movesList[2].theEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation); // show heal-effect on enemy himself
                        Instantiate(theDamageNumber, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation).SetDamage(health);
                        activeBattlers[1].currentHp += movesList[i].health;
                    }
                    if (movesList[i].movePower > 0)
                    {
                        Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);

                    }
                    moveN = movesList[i].moveName.ToString();
                    health = movesList[i].health;
                    turns = movesList[i].turns;
                    turnsToWait = movesList[i].turnsToWait;
                    turnsToHurt = movesList[i].turnsToHurt;
                    if (0 < movesList[i].turnsToHurt) //ATTACK OVER  ROUNDS
                    {
                        movesList[i].turns = 0;
                    }
                    if (movesList[i].movePower > 0)
                    {
                        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                        activeBattlers[0].GetComponent<Animator>().Play("Defence", -1, 0f);
                    }
                }

            }

            activeBattlers[1].GetComponent<Animator>().Play("Attack", -1, 0f);

            if (activeBattlers[1].charName == "A.Leiter" || activeBattlers[1].charName == "Tod") { AudioManager.instance.PlaySFX(17); }

            if (activeBattlers[1].charName == "Ghoul") { AudioManager.instance.PlaySFX(15); }
            else AudioManager.instance.PlaySFX(Random.Range(4, 8)); // play attackSFXs randomly
            health = 0;
            DealDamage(selectedTarget, movePower, moveN, health, turns);
        }
    }

    public void DealDamage(int target, int movePower, string moveName, int health, int turns)

    {
        // deal health
        activeBattlers[currentTurn].currentHp += health;
        if (health > 0)
        {
            Instantiate(theDamageNumber, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation).SetDamage(health);

            if (activeBattlers[currentTurn].isPlayer)
                activeBattlers[0].GetComponent<Animator>().Play("Defence", -1, 0f); // HEAL ANIMATION
        }

        turns = 0;
        float atkPwr = activeBattlers[currentTurn].strength; // + activeBattlers[currentTurn].wpnPower;
        float defPwr = activeBattlers[target].defence; // + activeBattlers[target].armrPower;

        if (activeBattlers[currentTurn] == activeBattlers[target]) // if enemy attacks hurt over rounds, they are activated while its the players turn, so set atkPower to enemies atkPower not that of player
            atkPwr = activeBattlers[1].strength;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);
        if (moveName == "Slash") // For Attack-BUTTON
            battleNotice.theText.text = activeBattlers[currentTurn].charName + " nutzt seine Handwaffe";
        else
            battleNotice.theText.text = activeBattlers[currentTurn].charName + " nutzt " + moveName;
        battleNotice.Activate();

        if (health == 0) // test
            activeBattlers[target].currentHp -= damageToGive;

        if (damageCalc > 0)
        {
            Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive * -1);

        }
        UpdateUIStats();
    }

    public void UpdateUIStats() // Stats at bottom
    {
        // PLAYER STATS
        BattleChar playerData = activeBattlers[0];
        playerName[0].gameObject.SetActive(true);
        playerName[0].text = playerData.charName;
        playerHP[0].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
        playerMP[0].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;

        // ENEMY STATS
        BattleChar playerData2 = activeBattlers[1];
        playerName[1].gameObject.SetActive(true);
        playerName[1].text = playerData2.charName;
        if (init == true)
        {
            HPValue = GameObject.FindWithTag("HPValueEnemy");
            MPValue = GameObject.FindWithTag("MPValueEnemy");
            init = false;
        }
        HPValue.GetComponent<UnityEngine.UI.Text>().text = Mathf.Clamp(playerData2.currentHp, 0, int.MaxValue) + "/" + playerData2.maxHP;
        MPValue.GetComponent<UnityEngine.UI.Text>().text = Mathf.Clamp(playerData2.currentMP, 0, int.MaxValue) + "/" + playerData2.maxMP;
    }

    public void PlayerAttack(string moveName) //, int selectedTarget (for multiple targets)
    {
        foreach (var move in movesList)
        {

            if (move.turnsToWait > 0 && move.turns < move.turnsToWait) // cooldowns
                move.turns++;
        }
        int movePower = 0;
        int health = 0;
        int turns = 0;
        int turnsToWait = 0;
        int selectedTarget = 0;

        // only one enemy
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }
        if (Enemies.Count == 1) selectedTarget = Enemies[0];

        foreach (var move in movesList)
        {
            if (move.moveName == moveName)
            {
                if (move.health > 0) Instantiate(move.theEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation); // show effect on player himself

                movePower = move.movePower;
                if (movePower > 0) Instantiate(move.theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation); // show effect on enemy
                moveName = move.moveName;
                health = move.health;
                turns = move.turns;
                turnsToWait = move.turnsToWait;

                if (move.turnsToWait > 0 || move.turnsToHurt > 0) // cooldowns
                {
                    move.turns = 0;
                }
                /************** ATTACK ANIMATION OF PLAYER *****************/

                if (activeBattlers[0].charName == "Krieger" && move.movePower > 0)
                {
                    AudioManager.instance.PlaySFX(Random.Range(8, 14)); // play attackSFXs randomly
                    activeBattlers[0].GetComponent<Animator>().Play("Attack", -1, 0f);
                    if (activeBattlers[1].charName != "Tod")
                        activeBattlers[1].GetComponent<Animator>().Play("Attacked", -1, 0f);
                    if (activeBattlers[1].charName == "Ghoul") AudioManager.instance.PlaySFX(15);
                    if (activeBattlers[1].charName == "A.Leiter" || activeBattlers[1].charName == "Tod") AudioManager.instance.PlaySFX(1);
                }
                /***********************************************************/
                if (moveName == "Slash")
                    AudioManager.instance.PlaySFX(16);
            }

        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation); // show Acting-Animation at player - see that he's acting

        DealDamage(selectedTarget, movePower, moveName, health, turns);

        uiButtonsHolder.SetActive(false);
        // targetMenu.SetActive(false);

        NextTurn();
    }

    /*
    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            } else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }
    */
    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Count > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                foreach (var move in movesList)
                {
                    if (move.moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = move.moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator EndBattleCo()
    {
        yield return new WaitForSeconds(2.5f);

        // victory / defeat animation PLAYER WINS
        if (activeBattlers[0].currentHp > 0)
        {
            Debug.Log("ENDBATTLE ");
            activeBattlers[0].GetComponent<Animator>().Play("Victory", -1, 0f);
            activeBattlers[1].GetComponent<Animator>().Play("Defeat", -1, 0f);

            if (activeBattlers[1].charName == "Ghoul") AudioManager.instance.PlaySFX(16);
            if (activeBattlers[1].charName == "A.Leiter")
            {
                AudioManager.instance.PlaySFX(19);
                AudioManager.instance.PlayBGM(2);
                AudioManager.instance.PlaySFX(20);
                stats.SetActive(false);
                aMasterDead = true;
                yield return new WaitForSeconds(14f);
            }
            if (activeBattlers[1].charName == "Tod") AudioManager.instance.PlaySFX(18);
            Debug.Log("ENDBATTLE time");
        }

        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        stats.SetActive(false);

        yield return new WaitForSeconds(.5f);
        // UIFade.stats.FadeToBlack();


        yield return new WaitForSeconds(3.5f);
        UIFade.instance.FadeToBlack();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {

                        GameManager.instance.playerStats[j].round = activeBattlers[i].round;
                    }
                }
            }
            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        battleActive = false;
        activeBattlers = new List<BattleChar>();
        currentTurn = 0;
        GameManager.instance.battleActive = false;
        m_SpriteRenderer.sprite = BGs[10];
        yield return new WaitForSeconds(1.5f);
        // CHANGE MUSIC AFTER BATTLE
        AudioManager.instance.PlayBGM(10);
        // BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
        //yield return new WaitForSeconds(5f);
        //BattleReward.instance.CloseRewardScreen();
        // start next Battle - HERE TALENT SCENE
        //BattleStart(new string[] { "" });

        SceneManager.LoadScene("Reward");
        activeBattlers.Clear();
    }

    public IEnumerator GameOverCo()
    {

        battleActive = false;
        yield return new WaitForSeconds(5.5f);
        m_SpriteRenderer.sprite = BGs[10];
        // UIFade.instance.FadeToBlack();
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }

    void Awake()
    {
        Debug.Log("Awake");
    }
    void OnEnable()
    {
        Debug.Log("OnEnable");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if (scene.name == "Battle")
        {
            Start();
        }

    }
    void OnApplicationQuit()
    {

        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}