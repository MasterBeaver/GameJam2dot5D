using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator anim;
    private float speed;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnWalkAnimation(float hoz)
    {
        if (hoz >0)
        {
            speed = hoz;
            anim.SetFloat((hoz > 0) ? "walk" : "walkback", hoz);
        }
        else
        {
            anim.SetFloat((speed > 0) ? "walk" : "walkback", hoz);
        }
            
    }

}
