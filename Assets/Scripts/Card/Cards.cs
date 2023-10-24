using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardsType
{
    None = 0,
    Summon = 1,
    Trap = 2,
    Space = 3

}


public class Cards :  MonoBehaviour
{
    public int team;
    public CardsType type;
    public int idCard;
    public int costPoint;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;

    public virtual List<Vector2Int> GetAvailabeMoves(int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        r.Add(new Vector2Int(3, 3));
        r.Add(new Vector2Int(3, 4));
        r.Add(new Vector2Int(4, 3));
        r.Add(new Vector2Int(4, 4));

        return r;
    }

    private void Awake()
    {

    }


    public virtual void TakeDamge()
    {

    }

    public virtual void DestroyThings(GameObject _target)
    {
       
    }

    internal virtual void Used()
    {
        Destroy(gameObject,1.0f);
    }

    internal virtual void CostToUse(int cost, string cardType)
    {
        if (Input.GetMouseButtonUp(0))
        {
            UI_Script.Instane.LostPoint(cost);
            Debug.Log("Cost " + cardType);
        }
            
    }


}
