using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN, 
    ENDTURN
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; set; }

    public BattleState state;

    public bool characterMove;
    public int LimitMoveB;
    
    private float elapsed=0.0f;
    public BattleState previousState;
    void Awake()
    {
        SetUp();
        state = BattleState.PLAYERTURN;
        LimitMoveB = 1;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (state == BattleState.PLAYERTURN)
        {
            PlayerTurn();
        }
        else if (state == BattleState.ENEMYTURN)
        {
            EnemyTurn();
        }

        if (state ==  BattleState.ENDTURN)
        {
            if (TimeEleaped() > 1.2f)
            {
                SetUp();
            }
            
        }

    }

    void SetUp()
    { 
        characterMove = true;
        UI_Script.Instane.currentPoint = 20;
        UI_Script.Instane.roundNum += 1;
        LimitMoveB = 1;

        if (previousState == BattleState.PLAYERTURN && UI_Script.Instane.roundNum != 1)
        {    
            state = BattleState.ENEMYTURN;
        }
        else
        {
            state = BattleState.PLAYERTURN;
        }
        previousState = state;
        elapsed = 0;
    }

    private void PlayerTurn()
    {
        //state = BattleState.ENEMYTURN;
    }

    public void EnemyTurn()
    {
        //state = BattleState.PLAYERTURN;
    }
    public void EndTurn()
    {
        Debug.Log("Turn end");
        state = BattleState.ENDTURN;
       
    }


    private float TimeEleaped()
    {
        elapsed += Time.deltaTime;
        float seconds = elapsed % 60;
        return seconds;
    }
}