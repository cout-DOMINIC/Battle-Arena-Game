using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour {

    public string spellName;
    public int spellCost;
    public Text nameText;
    public Text costText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    public void Press()
    { 
        var result = BattleManager.instance.movesList;
       
       
       
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            
            foreach(var moves in BattleManager.instance.movesList)
            { 
                if(moves.moveName == spellName)
                    
                    if (moves.turns < moves.turnsToWait && moves.turnsToWait > 0)
                    {
                        if((moves.turnsToWait - moves.turns) > 1)
                    BattleManager.instance.battleNotice.theText.text = "Du musst dich für " + (moves.turnsToWait - moves.turns) + " Runden erholen!";
                        if ((moves.turnsToWait - moves.turns) == 1)
                            BattleManager.instance.battleNotice.theText.text = "Du musst dich für " + (moves.turnsToWait - moves.turns) + " Runde erholen!";
                        BattleManager.instance.battleNotice.Activate();
                    BattleManager.instance.magicMenu.SetActive(false);
                    }
                        else
                        {
                            BattleManager.instance.magicMenu.SetActive(false);
                            //BattleManager.instance.OpenTargetMenu(spellName);
                            BattleManager.instance.PlayerAttack(spellName);
                            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
                        }
                }
            
            
        } else
        {
            //let player know there is not enough MP
            BattleManager.instance.battleNotice.theText.text = "Not Enough MP!";
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
