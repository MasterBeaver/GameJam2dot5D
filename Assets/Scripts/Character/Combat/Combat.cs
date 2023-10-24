using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] public Transform projectilePr;
    [SerializeField] private float force = 1.5f;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Transform soucre;
    [SerializeField] private float angelForRay;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxIncrease;
    [SerializeField] private float duration = 1f;
    private Transform target;    
    private Vector3 velocity;
    public float mRange;
    private Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left};
    private cursorController cur;
    private CharacterMovement character;
    private Vector3 dircBall;
    private Vector3 mPos;

    public int damage;
    public bool ableToAttack = false;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public AutomaticDestroy damageDealer;
    private Summon _summon;
    private Animator anim;
    private Rigidbody rb;
    private Collider col;
    private float _jumpDuration = 1f;
    private bool hitEnemy;
    private GameObject objHit;

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy")
        {
            objHit = collision.gameObject;
            hitEnemy = true;
        }
            
    }

    private void Awake()
    {
        mPos = transform.localPosition;
    }

    void Start()
    {
        isAttacking = false;
        cur = FindObjectOfType<cursorController>();
        character = GetComponent<CharacterMovement>();
        _summon = GetComponent<Summon>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        /*damageDealer = projectilePr.GetComponent<AutomaticDestroy>();
        damageDealer.damage = damage;*/
        //mPos = transform.localPosition;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (hitEnemy)
        {
            objHit.GetComponent<Character>().currentHealth -= damage;
            this.transform.position = mPos;
            hitEnemy = false;
        }
        if (!ableToAttack)
        {
            return;
        }
        board.Instance.isHitTile = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,_layer))
        {
            isAttacking = true;
            
            if (hit.collider != null)
            {  hitEnemy = false;
                
                if (Input.GetMouseButtonDown(0))
                {
                    
                    hitEnemy = false;
                    target = hit.collider.gameObject.transform;
                    if (_summon.idCard== 1)
                    {
                        FireProjectile();
                    }
                    else
                    {
                        board.Instance.isHitTile = true;
                        anim.SetTrigger("attack");
                        //rb.useGravity = false;
                        //rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                        //JumpToTarget();
                        StartCoroutine(AttackAction());
                        
                    }
                    
                }
            }
            else
            {
                character.movable = (character.LimitMove > 0) ?true : false;
               
            }

        }

       
    }


    void FireProjectile()
    {
        Transform pr = Instantiate(projectilePr, soucre.position, Quaternion.Euler(dircBall));
        pr.rotation = projectilePr.rotation;
        projectilePr.GetComponent<AutomaticDestroy>().damage = damage;
        velocity = (target.position- transform.position) * force;

        pr.GetComponent<Rigidbody>().velocity = velocity;
        
        ableToAttack = false;
        cur.elapsed = 0;
        cur.enabled = true;
        isAttacking = false;
        board.Instance.isHitTile = true;
        board.Instance.RemoveHighlightTilees();
    }

    public bool InRange(float range)
    {
        mRange = range;
        for (int i=0; i< directions.Length; i++)
        {
            //Vector3 directionRay = Quaternion.AngleAxis(angelForRay, transform.right) * directions[i];
            
            Ray rayA = new Ray(transform.position, directions[i] * range);

            if (Physics.Raycast(rayA, out RaycastHit hit, range, LayerMask.GetMask("Enemy", "Character")))
            {
                //Debug.Log("Hit enemy or character1");
                dircBall = directions[i];
                if (hit.collider != null) return true;
            }
            
        }
        return false;
    }

    IEnumerator AttackAction()
    {
        
        float startTime = Time.time;
        float endTime = startTime + duration;
        float startYPos = transform.position.y;
        float endYPos = startYPos + maxIncrease;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            float y = Mathf.Lerp(startYPos, endYPos, t); // interpolate position.y between start and end values
            col.isTrigger = true;
            transform.position = new Vector3(transform.position.x, y, transform.position.x);
            yield return null;
        }
        col.isTrigger = false;
        transform.position = new Vector3(transform.position.x + mRange-0.01f, endYPos, transform.position.z + mRange); // make sure position is exactly at the end value

        //target = null; // Clear the target so the character doesn't keep jumping towards it
        Debug.Log("Jump");

            
        ableToAttack = false;
        cur.elapsed = 0;
        cur.enabled = true;
        isAttacking = false;
        board.Instance.isHitTile = true;
        board.Instance.RemoveHighlightTilees();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < directions.Length; i++)
        {
          
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, directions[i] * mRange);
        }

       
    }

    void JumpToTarget()
    {
        Vector3 jumpDirection = target.position - transform.position;
        float jumpDistance = jumpDirection.magnitude;

        // Normalize the jump direction
        jumpDirection.Normalize();
        col.isTrigger = false;
        transform.position = new Vector3(0, 1f, 0) + transform.position;
        // Calculate the jump height based on the jump distance
        float jumpHeight = Mathf.Clamp(jumpDistance * 0.5f, 1f, 10f);
        
        // Calculate the jump velocity based on the jump force and jump height
        float jumpVelocity = Mathf.Sqrt(2f * jumpForce * jumpHeight);

        // Set the jump velocity to the rigidbody
        rb.velocity = jumpDirection * jumpVelocity;

        // Wait for the jump duration
        StartCoroutine(WaitForJumpDuration());
    }

    IEnumerator WaitForJumpDuration()
    {
        yield return new WaitForSeconds(_jumpDuration);
        
        // Enable gravity and unfreeze the position of the rigidbody
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        // Set the isJumping flag to false
    }

    

}
