using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGuardian : Character
{
    public int idCard;
    public Material _material;

    private float range = 1.3f * 5;
    private const float  gridSize = 1.3f ;
    private const float distBox = 2.7f;
    public bool useAbility = false;

    private GameObject player;
    private GameObject enemy;
    private GameObject objs;


    private void Awake()
    {
        
    }

    private void Update()
    {
        Raycasting();
        switch (idCard)
        {
            case (1):  
                if (objs == null) return;
                InccreaseOrdecreaseH(1);
                InccreaseOrdecreaseD(1);
                break;
            case (2):
                if (objs == null) return;
                WofHavenAbility(1);
                break;
            case (3):
                if (objs == null) return;
                WofHavenAbility(2);
                break;
            case (4):
                if (objs == null) return;
                InccreaseOrdecreaseH(2);
                break;
        }

    }


    private void WofHavenAbility(int health)
    {
        if (!useAbility) return;
        if (objs.tag != "Player") return;
        Debug.Log("Abitily");
        objs.GetComponent<Character>().currentHealth += health;
        objs.GetComponent<Combat>().damage += health;
        useAbility = false;

    }

    private void InccreaseOrdecreaseH(int value)
    {
        if (!useAbility) return;
        Debug.Log("Abitily");
        objs.GetComponent<Character>().currentHealth += (objs.tag == "Player") ? value : (-value);
        useAbility = false;
    }

    private void InccreaseOrdecreaseD(int value)
    {
        if (!useAbility) return;
        Debug.Log("Abitily");
        objs.GetComponent<Combat>().damage += (objs.tag == "Player") ? value : (-value);
        useAbility = false;
       
    }

    private void Raycasting()
    {
        Vector3 newZ = new Vector3(0, 0, 1.0f) * gridSize * distBox + transform.position;
        newZ -= new Vector3(0, 0.1f, 0f);
        for (int i = 0; i < 7; i++)
        {
            Ray ray = new Ray(newZ, Vector3.right);

            if (Physics.Raycast(ray, out RaycastHit hit, range, LayerMask.GetMask("Character")))
            {
                objs = hit.collider.gameObject;
            }

            newZ -= new Vector3(0, 0, 1.3f);
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 newZ = new Vector3(0, 0, 1.0f) * gridSize * distBox +  transform.position  ;
        newZ -= new Vector3(0.5f, 0.1f, 0f);

        for (int i = 0; i<7;i++)
        {
            Gizmos.DrawRay(newZ, Vector3.right * range);
             newZ -= new Vector3(0, 0, 1.3f);
        }
    }

}
