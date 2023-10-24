using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDestroy : MonoBehaviour
{
    [SerializeField] private float range;
    private GameObject hit;
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject _hit = collision.gameObject;
        if (_hit.CompareTag("Enemy"))
        {
            if (_hit.GetComponent<Character>() != null)
            {
                _hit.GetComponent<Character>().Takendamage(damage);
            }
            else
            {
                Debug.Log("Hit guardian");
            }
            
            Destroy(this.gameObject, 0.01f);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward * range));
        RaycastHit hitRay;

        if (Physics.Raycast(ray, out hitRay, range, LayerMask.GetMask("Enemy")))
        {
            Destroy(this.gameObject, 0.8f);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right * range));
    }
}
