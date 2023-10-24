using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VizObjScript : MonoBehaviour
{
    private Vector3[] directions = {Vector3.right,Vector3.left };
    private Ray ray;
    private Collider col;
    void Awake()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < directions.Length; i++)
        {
            
            ray = new Ray(col.bounds.center, directions[i]);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10.0f, LayerMask.GetMask("Character", "Enemy")))
            {
                if (hit.collider.GetComponent<Collider>() != null)
                {
                    Destroy(hit.collider.gameObject, 0.04f);
                    
                }
            }
        }


        Destroy(gameObject, directions.Length);

    }

    private void OnDrawGizmos()
    {
        col = GetComponent<Collider>();
        for (int i = 0; i < directions.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(col.bounds.center, directions[i] * 10.0f);
        }
            
    }
}
