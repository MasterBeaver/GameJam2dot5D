using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorController : MonoBehaviour
{
    [HideInInspector] public Cards _card;
    [HideInInspector] public bool summon;

    [HideInInspector] public GameObject mTarget;
    [HideInInspector] public GameObject cardTarget;
    
    [HideInInspector]public DragSys darySys;
    public CardsType _cardType;

    [HideInInspector] public Trap _trap;
    [HideInInspector] public Space _spaceCard;
    [HideInInspector] public Summon _summonCard;
   

    Vector3 mousePos;
    [HideInInspector] public Vector3 currentTile;

    [HideInInspector] public float elapsed = 0f;
    // Update is called once per frame

    private void Awake()
    {
        elapsed = 2.0f;
    }

    void Update()
    {
        bool Ittime = TimeEleaped() > 0.85f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile", "Character", "Cards")))
        {
            if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Enemy"))
            {
                if ((Input.GetMouseButtonDown(0) && Ittime))
                {
                    IClicked click = hit.collider.gameObject.GetComponent<IClicked>();
                    if (click != null) click.onClickAction();
                }
                
            }

            #region Card Interection and Card Action
            if (hit.collider.tag == "Card")
            {
                _card = hit.collider.gameObject.GetComponent<Cards>();
                darySys = hit.collider.gameObject.GetComponent<DragSys>();
                darySys.hover = true;
                if (_card == null) return;
                
                if ((hit.collider.gameObject.GetComponent<Cards>().type == CardsType.Summon) && darySys.moving) 
                    summon = true;
                else
                    summon = false;

                cardTarget = hit.collider.gameObject;
                _cardType = hit.collider.gameObject.GetComponent<Cards>().type;
            }
            else
            {
                
                if (_card != null)
                    CardAction(_cardType, cardTarget);
            }

            if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
            {
                GameObject obj = hit.collider.gameObject;
                mTarget = obj;
            }
            

            #endregion

            if (hit.collider.tag == "Highlight")
            {
                currentTile = hit.collider.bounds.center;

                board.Instance.isHitTile = true;
                board.Instance.HoverTile(hit.collider.gameObject, hit.collider.tag);
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Hover");

            }
            else if (hit.collider.tag == "Space")
            {
                currentTile = hit.collider.bounds.center;

                board.Instance.isHitTile = true;
                board.Instance.HoverTile(hit.collider.gameObject, hit.collider.tag);
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Hover");
            }


        }
        
        

    }

    private void CardAction(CardsType _cardType, GameObject targetCard)
    {
        switch (_cardType)
        {
            case (CardsType.Trap):
                _trap = targetCard.GetComponent<Trap>();

                if (darySys.moving && _trap.costPoint >= UI_Script.Instane.currentPoint) _trap.avableavailable = true; //***Don't forget      
                if (!darySys.moving && !_trap.avableavailable) return;
                _trap.target = (_trap.idCard == 1) ? mTarget : null;
                _trap.TrapAction(_trap.idCard, gameObject.GetComponent<cursorController>(), currentTile);

                _trap = null;
                break;
            case (CardsType.Space):
                _spaceCard = targetCard.GetComponent<Space>();
                break;
            case (CardsType.Summon):
                _summonCard = targetCard.GetComponent<Summon>();
                break;
            default:
                break;
        }

    }
    private int TimeEleaped()
    {
        elapsed += Time.deltaTime;
        int seconds = (int)elapsed % 60;
        
        return seconds;
    }
    private void OnDrawGizmos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, mousePos - transform.position);
    }
}

