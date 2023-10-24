using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class CharacterMovement : MonoBehaviour, IClicked
{ 
    [SerializeField ] private float gridSize = 1f; // size of each grid square
    [SerializeField] private float speed = 5f; // speed of movement
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float angelForRay;
    [SerializeField] private Material _materialForGrid;

    public int LimitMove;
    public bool movable;
    public Vector3 targetPosition;
    private Vector3[] direction = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private Animator _animation;
    private Animation _anim;
    private float elapsed = 0f;
    private const float additionalRange = 2.35f;
    private const float additionalAngel = 19.5f;
    private cursorController cur;
    private Summon _summon;

    private Combat _combat;
    private BattleSystem turnBased;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _anim = GetComponent<Animation>();
        _combat = GetComponent<Combat>();
        _summon = GetComponent<Summon>();
        cur = FindObjectOfType<cursorController>();
        turnBased = FindObjectOfType<BattleSystem>();
    }
    private void Start()
    {
        movable = false ;
        //targetPosition = new Vector3(oneDecimal(transform.position.x), oneDecimal(transform.position.y), oneDecimal(transform.position.z));
        targetPosition = this.transform.position;
        LimitMove = turnBased.LimitMoveB;
    }
    private void Update()
    {
        if (turnBased.state == BattleState.ENDTURN)
        {
            LimitMove = turnBased.LimitMoveB;
        } 

        if (!movable ) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
            //return;

        }
        if ((Mathf.Abs(transform.position.x - targetPosition.x) != 0 || Mathf.Abs(transform.position.z - targetPosition.z)!=0) && 
            transform.position != targetPosition)         //(Mathf.Abs(transform.position.x - targetPosition.x) != 0 || Mathf.Abs(transform.position.z - targetPosition.z) != 0)
        {
            board.Instance.isHitTile = true;
            MoveToTarget();

        }

    }

    public void onClickAction()
    {
        cur.enabled = false;
        board.Instance.isHitTile = (LimitMove > 0) ? false : true;
        movable = (LimitMove<=0)? false : true;
       

        board.Instance.getHighlist = true;
         //Edit
        
        for (int r = 0; r < direction.Length; r++)
        {
            Vector3 rayRotation = Vector3.zero;
            Vector3 directionRay = Vector3.zero;
            if (r >= direction.Length / 2)
            {
                directionRay = Quaternion.AngleAxis((r == 5 || r == 6) ? angelForRay - additionalAngel : -(angelForRay - additionalAngel),
                (r == 4 || r == 5) ? transform.right : transform.up) * direction[r] * (gridSize * additionalRange);
 
            }
            else
            {
                rayRotation = Quaternion.AngleAxis((r == 1 || r == 2) ? -angelForRay : angelForRay,
                (r == 0 || r == 1) ? transform.right : transform.up) * direction[r] * (gridSize * 1.5f);
            }
                
            Ray ray = new Ray(transform.position, (r >= direction.Length / 2) ? directionRay : rayRotation);  
            RaycastHit _hit;

            if (Physics.Raycast(ray, out _hit, gridSize * 1.5f, LayerMask.GetMask("Tile", "Hover")))
            {  
                if (_hit.collider != null && _hit.collider.CompareTag("Highlight") || _hit.collider.CompareTag("Space"))
                {
                    
                    if (_combat.InRange(gridSize))
                    {
                        board.Instance.InRangeAttack(_hit.collider.gameObject);
                        _combat.ableToAttack = true;
                    }
                    else
                    {
                        _hit.collider.gameObject.layer = LayerMask.NameToLayer("Highlight");
                        board.Instance.HighlightTilees(_hit.collider.gameObject);

                    }
                }
       

            }

            if (Physics.Raycast(ray, out _hit, gridSize * additionalRange, LayerMask.GetMask("Tile", "Hover")))
            {
                if (_summon.idCard != 1) return;
                if ((_hit.collider != null && _combat.InRange(gridSize * additionalRange)))
                {
                    board.Instance.InRangeAttack(_hit.collider.gameObject);
                    _combat.ableToAttack = true;

                }
                
            }
            
        }

        if (_combat.InRange(gridSize * additionalRange))
        {
            _combat.ableToAttack = true;
        }
    }

    private void SetTargetPosition()
    {
 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Highlight")))
        {
        
            Vector3 toPoint = hit.collider.bounds.center;
            toPoint.y = transform.position.y;


            float dist = Vector3.Distance(toPoint, transform.position);

            dist = Mathf.Abs(dist);
           
            if (dist <= gridSize + (gridSize/2)) //_animation.GetLayerName(0).Length
            {
                Debug.Log(dist);
                targetPosition = toPoint;
                _anim.OnWalkAnimation(dist);
                cur.elapsed = 0;
            }
            
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        if ((transform.position.x == targetPosition.x) && (transform.position.z == targetPosition.z))
        {

            board.Instance.isHitTile = true ;
            board.Instance.RemoveHighlightTilees();
            _anim.OnWalkAnimation(0.0f);
            elapsed = 0;
            cur.enabled = true;
            LimitMove -= 1;

            movable = false;
        }
    }

    #region Math
    private float oneDecimal(float num)
    {
        return Mathf.Round(num * 10.0f) * 0.1f;
    }

    private float TimeEleaped()
    {
        elapsed += Time.deltaTime;
        float seconds = elapsed % 60;
        Debug.Log(seconds);
        return seconds;
    }

    #endregion

    private void OnDrawGizmos()
    {
        for (int r = 0; r < direction.Length; r++)
        {
            Vector3 rayRotation = Vector3.zero;
            Vector3 directionRay = Vector3.zero;
            if (r >= direction.Length / 2)
            {
                directionRay = Quaternion.AngleAxis((r == 5 || r == 6) ? angelForRay - additionalAngel : -(angelForRay - additionalAngel),
                (r == 4 || r == 5) ? transform.right : transform.up) * direction[r] * (gridSize * additionalRange);


                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position, directionRay);
            }
            else
            {
                rayRotation = Quaternion.AngleAxis((r == 1 || r == 2) ? -angelForRay : angelForRay,
                (r == 0 || r == 1) ? transform.right : transform.up) * direction[r] * (gridSize * 1.5f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, rayRotation);
            }

        }

    }
    

   
}
