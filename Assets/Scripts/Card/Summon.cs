using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Cards
{
    public void SummonAction(int num, cursorController cursor, Vector3 targetTile)
    {
        idCard = num;
    }

    private void Awake()
    {
        if (team==1)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.flipX = true;

            gameObject.tag = "Enemy";
        }
    }

    internal override void CostToUse(int cost, string cardType)
    {
        base.CostToUse(cost, cardType);
    }
    internal override void Used()
    {
        base.Used();
    }

}
