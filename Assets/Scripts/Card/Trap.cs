using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Cards
{
    public bool avableavailable;
    internal GameObject target;
    private cursorController _cursor;

    private Vector3 _targetTile;
    private void Update()
    {
        if (!avableavailable) return;
        switch(idCard)
        {
            case (1):
                if (target != null) DestroyThings(target);
                CostToUse(10, "Trap");
                avableavailable = false;
                break;
            case (2):
                
                DestroyOnRow();
                CostToUse(10, "Trap");
                avableavailable = false;
                break;
            case (3):
                DestroyEveryThing();
                CostToUse(20, "Trap");
                avableavailable = false;
                break;
            default:
                break;
        }
    }

    public void TrapAction(int num, cursorController cursor, Vector3 targetTile)
    {
        idCard = num;
        _cursor = cursor;
        _targetTile = targetTile;
    }

    public override void DestroyThings(GameObject _target)
    {
        if(_target.GetComponent<Character>() != null )
        {
            
            Destroy(_target, 0.5f);
            target = null;
            Destroy(gameObject, 1.0f);
        }
        
    }

    public void DestroyOnRow()
    {
        Vector3 tilePos = new Vector3(0, 0.4f, 0) + _targetTile;
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(board.Instance.cards[2].prefab[0], tilePos, Quaternion.identity);

            Used();
        }
            
    }

    internal override void CostToUse(int cost, string cardType)
    {
        base.CostToUse(cost, cardType);
        
    }

    void DestroyEveryThing()
    {
        Character[] characters = FindObjectsOfType<Character>();
        for (int c = 0; c < characters.Length; c++)
        {
            if (c < board.Instance.cards[1].prefab.Count)
                if (characters[c].gameObject == board.Instance.cards[1].prefab[c])
                    board.Instance.cards[1].prefab.RemoveAt(c);

            board.Instance.OnChangeToDefualt();
            Destroy(characters[c].gameObject);
        }
    }
   

}
